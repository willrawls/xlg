using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable UnusedType.Global

namespace MetX.Standard.Library
{
    public class PartnerCrypt : IDisposable
    {
        private readonly SymmetricAlgorithm _cryptoService;
        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        public readonly string Key;
        public readonly string Vector;

        public void Dispose()
        {
            _encryptor?.Dispose();
            _decryptor?.Dispose();
        }

		public PartnerCrypt(string theKey, string theVector)
        {
            _cryptoService = new RijndaelManaged {KeySize = 256};
            Key = theKey;
            Vector = theVector;
            InternalSetup();
        }

		public PartnerCrypt()
		{
            _cryptoService = new RijndaelManaged {KeySize = 256};
            Key = Crypt.CryptKey;
			Vector = Crypt.CryptVector;
			InternalSetup();
		}

        private void InternalSetup()
        {
            _cryptoService.Key = Encoding.ASCII.GetBytes(Key);
            _cryptoService.IV = Encoding.ASCII.GetBytes(Vector.Length > _cryptoService.BlockSize / 8 ? Vector.Substring(0, _cryptoService.BlockSize / 8) : Vector);
            
            var v = (byte)(Math.Abs(DateTime.UtcNow.DayOfYear - DateTime.UtcNow.Day + (int)DateTime.UtcNow.DayOfWeek) + 1);
            for (var i = 0; i < _cryptoService.Key.Length; i++)
                _cryptoService.Key[i] = (byte)((_cryptoService.Key[i] + v) % 254);
            for (var i = 0; i < _cryptoService.IV.Length; i++)
                _cryptoService.IV[i] = (byte)((_cryptoService.IV[i] + v) % 254);
            _decryptor = _cryptoService.CreateDecryptor();
            _encryptor = _cryptoService.CreateEncryptor();
        }

        public string ToBase64(string source)
        {
            if (source == null || Key == null || source.Length == 0 || Key.Length == 0)
                return string.Empty;

            var bytIn = Encoding.ASCII.GetBytes(source);
            // create a MemoryStream so that the process can be done without I/O files
            var ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            var cs = new CryptoStream(ms, _encryptor, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            var returnValue = Convert.ToBase64String(ms.ToArray(), 0, (int)ms.Length);
            cs.Close();
            ms.Close();
            return returnValue;
        }

        public string FromBase64(string encryptedSource)
        {
            if (encryptedSource == null || Key == null || encryptedSource.Length == 0 || Key.Length == 0)
                return string.Empty;

            // convert from Base64 to binary
            var encryptedBytes = Convert.FromBase64String(encryptedSource);
            // create a MemoryStream with the input
            string returnValue;
            using var memoryStream = new MemoryStream(encryptedBytes, 0, encryptedBytes.Length);
            using (var cryptoStream = new CryptoStream(memoryStream, _decryptor, CryptoStreamMode.Read))
            {
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    returnValue = streamReader.ReadToEnd();
                    streamReader.Close(); 
                }

                cryptoStream.Close(); 
            }

            memoryStream.Close();

            return returnValue;
        }

        public string NameValuesToBase64(System.Collections.Specialized.NameValueCollection nameValuePairs)
        {
            if (nameValuePairs == null || nameValuePairs.Count == 0)
                return string.Empty;
            var sb = new StringBuilder();
            for (var i = 0; i < nameValuePairs.Count; i++)
            {
                sb.AppendLine(WebUtility.UrlEncode(nameValuePairs.GetKey(i) != null ? nameValuePairs.GetKey(i) : string.Empty));
                sb.AppendLine(WebUtility.UrlEncode(nameValuePairs[i] != null ? nameValuePairs[i] : string.Empty));
            }
            return ToBase64(sb.ToString());
        }

        public System.Collections.Specialized.NameValueCollection NameValueFromBase64(string encryptedSource)
        {
            var ret = new System.Collections.Specialized.NameValueCollection();
            if (string.IsNullOrEmpty(encryptedSource))
                return ret;
            var sItems = FromBase64(encryptedSource);
            if (string.IsNullOrEmpty(sItems)) return ret;
            
            var items = sItems.Split(new[] { "\r\n" }, StringSplitOptions.None);
            for (var i = 0; i < items.Length - 1; i += 2)
                ret.Add(WebUtility.UrlDecode(items[i]), WebUtility.UrlDecode(items[i + 1]));
            return ret;
        }
    }
}
