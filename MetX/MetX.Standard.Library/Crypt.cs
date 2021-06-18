using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace MetX.Standard.Library
{
    public static class Crypt
    {
        private static readonly object SyncRoot = new();

        private static SymmetricAlgorithm _cryptoService;
        private static byte[] _key;
        private static byte[] _vector;

        private static ICryptoTransform _encryptorToday;
        private static ICryptoTransform _decryptorToday;
        private static ICryptoTransform _decryptorYesterday;

        private static ICryptoTransform _encryptorFixed;
        private static ICryptoTransform _decryptorFixed;

        public static readonly byte[] DefaultKeyBytes = new byte[] {41, 12, 51, 6, 54, 8, 111, 89, 150, 12, 80, 10, 8, 12, 1, 170};
        public static byte[] KeyBytes = DefaultKeyBytes;

        public static string CryptKey { get; set; }
        public static string CryptVector { get; set; }

        public static void Reset()
        {
            lock (SyncRoot)
            {
                if (KeyBytes == null || KeyBytes.Length != 16)
                    throw new CryptographicException("16 byte key required. Set the KeyBytes field");
                
                var bytes = new byte[16];
                Array.Copy(KeyBytes, bytes, 16);
                    
                using (SymmetricAlgorithm sa = new RijndaelManaged())
                {
                    sa.BlockSize = 128;
                    sa.Key = bytes;
                    sa.IV = sa.Key;
                    _encryptorFixed = sa.CreateEncryptor();
                    _decryptorFixed = sa.CreateDecryptor();
                }

                _cryptoService = new RijndaelManaged {KeySize = 256};
                _key = Encoding.ASCII.GetBytes(CryptKey
                        ?? throw new InvalidOperationException());
                
                var theVector = CryptVector;
                _vector = Encoding.ASCII.GetBytes(
                    (theVector != null && theVector.Length > _cryptoService.BlockSize / 8
                        ? theVector.Substring(0, _cryptoService.BlockSize / 8)
                        : theVector)
                    ?? throw new InvalidOperationException());
                InternalSetup(true);
                InternalSetup(false);
            }
        }

        private static void InternalSetup(bool today)
        {
            _cryptoService.Key = _key;
            _cryptoService.IV = _vector;
            var dt = today ? DateTime.UtcNow : DateTime.UtcNow.AddDays(-1);
            var v = (byte) (Math.Abs(dt.DayOfYear - dt.Day + (int) dt.DayOfWeek) + 1);
            for (var i = 0; i < _cryptoService.Key.Length; i++)
                _cryptoService.Key[i] = (byte) ((_cryptoService.Key[i] + v) % 254);
            for (var i = 0; i < _cryptoService.IV.Length; i++)
                _cryptoService.IV[i] = (byte) ((_cryptoService.IV[i] + v) % 254);
            if (today)
            {
                _decryptorToday = _cryptoService.CreateDecryptor();
                _encryptorToday = _cryptoService.CreateEncryptor();
            }
            else
            {
                _decryptorYesterday = _cryptoService.CreateDecryptor();
            }
        }

        public static string ToBase64Fixed(string source)
        {
            if (_encryptorFixed == null) Reset();

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var bytIn = Encoding.ASCII.GetBytes(source);
            // create a MemoryStream so that the process can be done without I/O files
            var ms = new MemoryStream();

            // create Crypto Stream that transforms a stream using the encryption
            var cs = new CryptoStream(ms, _encryptorFixed 
                    ?? throw new InvalidOperationException(), 
                CryptoStreamMode.Write);
             
            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // convert into Base64 so that the result can be used in xml
            var returnValue = Convert.ToBase64String(ms.ToArray(), 0, (int) ms.Length);
            cs.Close();
            ms.Close();
            return returnValue;
        }

        public static string ToBase64(string source)
        {
            if (_encryptorFixed == null) Reset();

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var bytIn = Encoding.ASCII.GetBytes(source);
            // create a MemoryStream so that the process can be done without I/O files
            using var ms = new MemoryStream();

            string returnValue;
            using (var cs = new CryptoStream(ms, _encryptorToday, CryptoStreamMode.Write))
            {
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();

                // convert into Base64 so that the result can be used in xml
                returnValue = Convert.ToBase64String(ms.ToArray(), 0, (int) ms.Length);
                cs.Close();
            }
            ms.Close();

            return returnValue;
        }

        public static string FromBase64(string encryptedSource)
        {
            if (_encryptorFixed == null) Reset();

            if (string.IsNullOrEmpty(encryptedSource))
                return string.Empty;

            if (encryptedSource.Contains("<"))
                encryptedSource = encryptedSource
                    .Substring(0, encryptedSource.IndexOf("<", StringComparison.Ordinal) - 1).Trim();
            if (encryptedSource.Contains(" "))
                encryptedSource = encryptedSource.Replace(" ", "+");

            var bytIn = Convert.FromBase64String(encryptedSource);
            string returnValue = null;
            using var ms = new MemoryStream(bytIn, 0, bytIn.Length);
            for (var i = 0; i < 2 && returnValue == null; i++)
            {
                try
                {

                    // create Crypto Stream that transforms a stream using the decryption
                    using (var cs = new CryptoStream(ms, i == 0 ? _decryptorToday : _decryptorYesterday, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            returnValue = sr.ReadToEnd();
                            sr.Close();
                        }
                        cs.Close();
                    }
                    ms.Close();
                }
                catch
                {
                    // Ignored
                }
            }

            return returnValue;
        }

        public static string FromBase64Fixed(string encryptedSource)
        {
            if (_encryptorFixed == null) Reset();

            if (string.IsNullOrEmpty(encryptedSource))
                return string.Empty;

            var bytIn = Convert.FromBase64String(encryptedSource);
            string returnValue = null;

            using var memoryStream = new MemoryStream(bytIn, 0, bytIn.Length);
            try
            {
                // create Crypto Stream that transforms a stream using the decryption
                using (var cs = new CryptoStream(memoryStream, _decryptorFixed, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        returnValue = sr.ReadToEnd();
                        sr.Close();
                    }
                    cs.Close();
                }
                memoryStream.Close();
            }
            catch
            {
                // Ignored
            }

            return returnValue;
        }

        public static NameValueCollection NameValueFromBase64(string encryptedSource)
        {
            if (encryptedSource == null) throw new ArgumentNullException(nameof(encryptedSource));
            if (_encryptorFixed == null) Reset();

            var ret = new NameValueCollection();
            if (string.IsNullOrEmpty(encryptedSource) && encryptedSource.Length < 1024)
                return ret;
            encryptedSource = FromBase64(encryptedSource);
            if (!string.IsNullOrEmpty(encryptedSource))
            {
                var items = encryptedSource.Lines();
                for (var i = 0; i < items.Length - 1; i += 2)
                    ret.Add(WebUtility.UrlDecode(items[i]), WebUtility.UrlDecode(items[i + 1]));
            }

            return ret;
        }
    }
}