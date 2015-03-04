using System;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MetX.Security
{
    public class PartnerCrypt : IDisposable
    {
        private SymmetricAlgorithm CryptoService;
        private ICryptoTransform Encryptor;
        private ICryptoTransform Decryptor;

        public string Key;
        public string Vector;

        public void Dispose()
        {
            if (Encryptor != null) Encryptor.Dispose();
            if (Decryptor != null) Decryptor.Dispose();
        }

		public PartnerCrypt(string TheKey, string TheVector)
        {
            CryptoService = new RijndaelManaged();
            CryptoService.KeySize = 256;
            Key = TheKey;
            Vector = TheVector;
            InternalSetup();
        }

		public PartnerCrypt()
		{
			CryptoService = new RijndaelManaged();
			CryptoService.KeySize = 256;
			Key = ConfigurationManager.AppSettings["Crypt.Key"];
			Vector = ConfigurationManager.AppSettings["Crypt.Vector"];
			InternalSetup();
		}

        private void InternalSetup()
        {
            CryptoService.Key = Encoding.ASCII.GetBytes(Key);
            if (Vector.Length > CryptoService.BlockSize / 8)
                CryptoService.IV = Encoding.ASCII.GetBytes(Vector.Substring(0, CryptoService.BlockSize / 8));
            else
                CryptoService.IV = Encoding.ASCII.GetBytes(Vector);
            byte v = (byte)(Math.Abs(DateTime.UtcNow.DayOfYear - DateTime.UtcNow.Day + (int)DateTime.UtcNow.DayOfWeek) + 1);
            for (int i = 0; i < CryptoService.Key.Length; i++)
                CryptoService.Key[i] = (byte)((CryptoService.Key[i] + v) % 254);
            for (int i = 0; i < CryptoService.IV.Length; i++)
                CryptoService.IV[i] = (byte)((CryptoService.IV[i] + v) % 254);
            Decryptor = CryptoService.CreateDecryptor();
            Encryptor = CryptoService.CreateEncryptor();
        }

        public string ToBase64(string Source)
        {
            if (Source == null || Key == null || Source.Length == 0 || Key.Length == 0)
                return string.Empty;

            byte[] bytIn = Encoding.ASCII.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, Encryptor, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            string ReturnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return ReturnValue;
        }

        public string FromBase64(string EncryptedSource)
        {
            if (EncryptedSource == null || Key == null || EncryptedSource.Length == 0 || Key.Length == 0)
                return string.Empty;

            // convert from Base64 to binary
            byte[] bytIn = Convert.FromBase64String(EncryptedSource);
            // create a MemoryStream with the input
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

            // create Crypto Stream that transforms a stream using the decryption
            CryptoStream cs = new CryptoStream(ms, Decryptor, CryptoStreamMode.Read);

            // read out the result from the Crypto Stream
            StreamReader sr = new StreamReader(cs);
            string ReturnValue = sr.ReadToEnd();
            sr.Close(); sr = null;
            cs.Close(); cs = null;
            ms.Close(); ms = null;
            return ReturnValue;
        }

        public string NameValuesToBase64(System.Collections.Specialized.NameValueCollection NameValuePairs)
        {
            if (NameValuePairs == null || NameValuePairs.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < NameValuePairs.Count; i++)
            {
                sb.AppendLine(System.Web.HttpUtility.UrlEncode((NameValuePairs.GetKey(i) != null ? NameValuePairs.GetKey(i) : string.Empty)));
                sb.AppendLine(System.Web.HttpUtility.UrlEncode((NameValuePairs[i] != null ? NameValuePairs[i] : string.Empty)));
            }
            return ToBase64(sb.ToString());
        }

        public System.Collections.Specialized.NameValueCollection NameValueFromBase64(string EncryptedSource)
        {
            System.Collections.Specialized.NameValueCollection ret = new System.Collections.Specialized.NameValueCollection();
            if (string.IsNullOrEmpty(EncryptedSource))
                return ret;
            string sItems = FromBase64(EncryptedSource);
            if (!string.IsNullOrEmpty(sItems))
            {
                string[] items = sItems.Split(new[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < items.Length - 1; i += 2)
                    ret.Add(System.Web.HttpUtility.UrlDecode(items[i]), System.Web.HttpUtility.UrlDecode(items[i + 1]));
            }
            return ret;
        }
    }
}
