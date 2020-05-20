using System;
using System.IO;
using System.Text;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using MetX.Library;

namespace MetX.Security
{
    public static class Crypt // : IDisposable
    {
        private static readonly object SyncRoot = new object();

        private static SymmetricAlgorithm _cryptoService;
        private static byte[] _key;
        private static byte[] _vector;

		private static ICryptoTransform _encryptorToday;
		private static ICryptoTransform _decryptorToday;
        private static ICryptoTransform _decryptorYesterday;

        private static ICryptoTransform _encryptorFixed;
        private static ICryptoTransform _decryptorFixed;

        private const string CacheKey = "Crypt.Check";

        public static void Reset()
        {
            lock (SyncRoot)
            {
                using (SymmetricAlgorithm sa = new RijndaelManaged())
                {
                    sa.BlockSize = 128;
                    sa.Key = new byte[] { 141, 125, 54, 46, 254, 82, 171, 89, 151, 12, 180, 150, 58, 132, 61, 70 }; sa.IV = sa.Key;
                    _encryptorFixed = sa.CreateEncryptor();
                    _decryptorFixed = sa.CreateDecryptor();
                }

                _cryptoService = new RijndaelManaged();
                _cryptoService.KeySize = 256;
                _key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Crypt.Key"]);
                string theVector = ConfigurationManager.AppSettings["Crypt.Vector"];
                if (theVector.Length > _cryptoService.BlockSize / 8)
                    _vector = Encoding.ASCII.GetBytes(theVector.Substring(0, _cryptoService.BlockSize / 8));
                else
                    _vector = Encoding.ASCII.GetBytes(theVector);
                InternalSetup(true);
                InternalSetup(false);

                if(HttpContext.Current != null)
                    HttpContext.Current.Cache.Add(CacheKey, "Still.Fresh",
                        new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("web.config")),
                        DateTime.Today.AddDays(1).AddSeconds(1), System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
        }

        private static void InternalSetup(bool today)
        {
            _cryptoService.Key = _key;
            _cryptoService.IV = _vector;
            DateTime dt = (today ? DateTime.UtcNow : DateTime.UtcNow.AddDays(-1));
            byte v = (byte) (Math.Abs(dt.DayOfYear - dt.Day + (int) dt.DayOfWeek) + 1);
            for (int i = 0; i < _cryptoService.Key.Length; i++)
                _cryptoService.Key[i] = (byte) ((_cryptoService.Key[i] + v) % 254);
            for (int i = 0; i < _cryptoService.IV.Length; i++)
                _cryptoService.IV[i] = (byte)((_cryptoService.IV[i] + v) % 254);
            if (today)
            {
                _decryptorToday = _cryptoService.CreateDecryptor();
                _encryptorToday = _cryptoService.CreateEncryptor();
            }
            else
                _decryptorYesterday = _cryptoService.CreateDecryptor();
        }

        public static string ToBase64Fixed(string source)
        {
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            byte[] bytIn = Encoding.ASCII.GetBytes(source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, _encryptorFixed, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            string returnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return returnValue;
        }

        public static string ToBase64(string source)
        {
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            byte[] bytIn = Encoding.ASCII.GetBytes(source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, _encryptorToday, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            string returnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return returnValue;
        }

        public static string FromBase64(string encryptedSource)
        {
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(encryptedSource))
                return string.Empty;

            if (encryptedSource.Contains("<"))
                encryptedSource = encryptedSource.Substring(0, encryptedSource.IndexOf("<") - 1).Trim();
            if (encryptedSource.Contains(" "))
                encryptedSource = encryptedSource.Replace(" ", "+");

            byte[] bytIn = Convert.FromBase64String(encryptedSource);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

            string returnValue = null;
            bool haveTriedYesterday = false;
TryAgain:
            try
            {
                // create Crypto Stream that transforms a stream using the decryption
                CryptoStream cs = new CryptoStream(ms, (haveTriedYesterday ? _decryptorToday : _decryptorYesterday), CryptoStreamMode.Read);

                // read out the result from the Crypto Stream
                StreamReader sr = new StreamReader(cs);
                returnValue = sr.ReadToEnd();
                sr.Close(); sr = null;
                cs.Close(); cs = null;
                ms.Close(); ms = null;
            } catch { }

            if (returnValue == null && !haveTriedYesterday)
            {
                haveTriedYesterday = true;
                goto TryAgain;
            }

            return returnValue;
        }

        public static string FromBase64Fixed(string encryptedSource)
        {
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(encryptedSource))
                return string.Empty;

            byte[] bytIn = Convert.FromBase64String(encryptedSource);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            string returnValue = null;
            try
            {
                // create Crypto Stream that transforms a stream using the decryption
                CryptoStream cs = new CryptoStream(ms, _decryptorFixed, CryptoStreamMode.Read);

                // read out the result from the Crypto Stream
                StreamReader sr = new StreamReader(cs);
                returnValue = sr.ReadToEnd();
                sr.Close(); sr = null;
                cs.Close(); cs = null;
                ms.Close(); ms = null;
            }
            catch { }
            return returnValue;
        }

        public static string NameValuesToBase64(System.Collections.Specialized.NameValueCollection nameValuePairs)
        {
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (nameValuePairs == null || nameValuePairs.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nameValuePairs.Count; i++)
            {
                sb.AppendLine(HttpUtility.UrlEncode(nameValuePairs.GetKey(i).AsString()));
                sb.AppendLine(HttpUtility.UrlEncode(nameValuePairs[i].AsString()));
            }
            return ToBase64(sb.ToString());
        }

        public static System.Collections.Specialized.NameValueCollection NameValueFromBase64(string encryptedSource)
        {
            if (encryptedSource == null)
            {
                throw new ArgumentNullException("encryptedSource");
            }
            if (_encryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            System.Collections.Specialized.NameValueCollection ret = new System.Collections.Specialized.NameValueCollection();
            if (string.IsNullOrEmpty(encryptedSource) && encryptedSource.Length < 1024)
                return ret;
            encryptedSource = FromBase64(encryptedSource);
            if (!string.IsNullOrEmpty(encryptedSource))
            {
                string[] items = encryptedSource.Lines();
                for (int i = 0; i < items.Length - 1; i += 2)
                    ret.Add(HttpUtility.UrlDecode(items[i]), HttpUtility.UrlDecode(items[i + 1]));
            }
            return ret;
        }
    }
}
