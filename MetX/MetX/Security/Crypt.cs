using System;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using System.Configuration;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MetX.Library;

namespace MetX.Security
{
    public static class Crypt // : IDisposable
    {
        private static readonly object _SyncRoot = new object();

        private static SymmetricAlgorithm CryptoService;
        private static byte[] Key;
        private static byte[] Vector;

		private static ICryptoTransform EncryptorToday;
		private static ICryptoTransform DecryptorToday;
        private static ICryptoTransform DecryptorYesterday;

        private static ICryptoTransform EncryptorFixed;
        private static ICryptoTransform DecryptorFixed;

        private const string CacheKey = "Crypt.Check";

        public static void Reset()
        {
            lock (_SyncRoot)
            {
                using (SymmetricAlgorithm sa = new RijndaelManaged())
                {
                    sa.BlockSize = 128;
                    sa.Key = new byte[] { 141, 125, 54, 46, 254, 82, 171, 89, 151, 12, 180, 150, 58, 132, 61, 70 }; sa.IV = sa.Key;
                    EncryptorFixed = sa.CreateEncryptor();
                    DecryptorFixed = sa.CreateDecryptor();
                }

                CryptoService = new RijndaelManaged();
                CryptoService.KeySize = 256;
                Key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Crypt.Key"]);
                string TheVector = ConfigurationManager.AppSettings["Crypt.Vector"];
                if (TheVector.Length > CryptoService.BlockSize / 8)
                    Vector = Encoding.ASCII.GetBytes(TheVector.Substring(0, CryptoService.BlockSize / 8));
                else
                    Vector = Encoding.ASCII.GetBytes(TheVector);
                InternalSetup(true);
                InternalSetup(false);

                if(HttpContext.Current != null)
                    HttpContext.Current.Cache.Add(CacheKey, "Still.Fresh",
                        new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("web.config")),
                        DateTime.Today.AddDays(1).AddSeconds(1), System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
        }

        private static void InternalSetup(bool Today)
        {
            CryptoService.Key = Key;
            CryptoService.IV = Vector;
            DateTime dt = (Today ? DateTime.UtcNow : DateTime.UtcNow.AddDays(-1));
            byte v = (byte) (Math.Abs(dt.DayOfYear - dt.Day + (int) dt.DayOfWeek) + 1);
            for (int i = 0; i < CryptoService.Key.Length; i++)
                CryptoService.Key[i] = (byte) ((CryptoService.Key[i] + v) % 254);
            for (int i = 0; i < CryptoService.IV.Length; i++)
                CryptoService.IV[i] = (byte)((CryptoService.IV[i] + v) % 254);
            if (Today)
            {
                DecryptorToday = CryptoService.CreateDecryptor();
                EncryptorToday = CryptoService.CreateEncryptor();
            }
            else
                DecryptorYesterday = CryptoService.CreateDecryptor();
        }

        public static string ToBase64Fixed(string Source)
        {
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(Source))
                return string.Empty;

            byte[] bytIn = Encoding.ASCII.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, EncryptorFixed, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            string ReturnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return ReturnValue;
        }

        public static string ToBase64(string Source)
        {
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(Source))
                return string.Empty;

            byte[] bytIn = Encoding.ASCII.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, EncryptorToday, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            string ReturnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return ReturnValue;
        }

        public static string FromBase64(string EncryptedSource)
        {
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(EncryptedSource))
                return string.Empty;

            if (EncryptedSource.Contains("<"))
                EncryptedSource = EncryptedSource.Substring(0, EncryptedSource.IndexOf("<") - 1).Trim();
            if (EncryptedSource.Contains(" "))
                EncryptedSource = EncryptedSource.Replace(" ", "+");

            byte[] bytIn = Convert.FromBase64String(EncryptedSource);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

            string ReturnValue = null;
            bool HaveTriedYesterday = false;
TryAgain:
            try
            {
                // create Crypto Stream that transforms a stream using the decryption
                CryptoStream cs = new CryptoStream(ms, (HaveTriedYesterday ? DecryptorToday : DecryptorYesterday), CryptoStreamMode.Read);

                // read out the result from the Crypto Stream
                StreamReader sr = new StreamReader(cs);
                ReturnValue = sr.ReadToEnd();
                sr.Close(); sr = null;
                cs.Close(); cs = null;
                ms.Close(); ms = null;
            } catch { }

            if (ReturnValue == null && !HaveTriedYesterday)
            {
                HaveTriedYesterday = true;
                goto TryAgain;
            }

            return ReturnValue;
        }

        public static string FromBase64Fixed(string EncryptedSource)
        {
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (string.IsNullOrEmpty(EncryptedSource))
                return string.Empty;

            byte[] bytIn = Convert.FromBase64String(EncryptedSource);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            string ReturnValue = null;
            try
            {
                // create Crypto Stream that transforms a stream using the decryption
                CryptoStream cs = new CryptoStream(ms, DecryptorFixed, CryptoStreamMode.Read);

                // read out the result from the Crypto Stream
                StreamReader sr = new StreamReader(cs);
                ReturnValue = sr.ReadToEnd();
                sr.Close(); sr = null;
                cs.Close(); cs = null;
                ms.Close(); ms = null;
            }
            catch { }
            return ReturnValue;
        }

        public static string NameValuesToBase64(System.Collections.Specialized.NameValueCollection NameValuePairs)
        {
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

            if (NameValuePairs == null || NameValuePairs.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < NameValuePairs.Count; i++)
            {
                sb.AppendLine(HttpUtility.UrlEncode(NameValuePairs.GetKey(i).AsString()));
                sb.AppendLine(HttpUtility.UrlEncode(NameValuePairs[i].AsString()));
            }
            return ToBase64(sb.ToString());
        }

        public static System.Collections.Specialized.NameValueCollection NameValueFromBase64(string encryptedSource)
        {
            if (encryptedSource == null)
            {
                throw new ArgumentNullException("encryptedSource");
            }
            if (EncryptorFixed == null || (HttpContext.Current != null && HttpContext.Current.Cache[CacheKey] == null)) Reset();

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
