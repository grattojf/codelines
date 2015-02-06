// Solution = sysutils . Projet = SysUtils
// Écrit par : Jean-François Gratton. 2013.03.29 (08:45)
// cryptoutils-encryption.cs : Méthode d'encryption symétrique, et encryption par MD5

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JFG.SysUtils.CryptoLib
{
    public static partial class CryptoUtils
    {
        private static object _key;
        private static SymmetricAlgorithm _mCsp;

        public static bool HasKey()
        {
            return (!(_key == null  || _key.ToString()=="" ));
        }

        private static string SetLengthString(string str, int length)
        {
            while (length > str.Length)
            {
                str += str;
            }
            if (str.Length > length)
            {
                str = str.Remove(length);
            }
            return str;
        }

        private static SymmetricAlgorithm SetEnc()
        {
            return new TripleDESCryptoServiceProvider();
        }

        public static string EncryptString(string Value, string k = "", string ivVal = "")
        {
            _mCsp = SetEnc();
            string iv;

            _key = String.IsNullOrWhiteSpace(k) ? "12345678" : k;

            iv = string.IsNullOrWhiteSpace(ivVal) ? "PenS8UCVF7s=" : ivVal;
            _mCsp.IV = Convert.FromBase64String(iv);
            string key = SetLengthString(_key.ToString(), 32);
            _mCsp.Key = Convert.FromBase64String(key);
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            Byte[] byt = new byte[64];

            try
            {
                ct = _mCsp.CreateEncryptor(_mCsp.Key, _mCsp.IV);

                byt = Encoding.UTF8.GetBytes(Value);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {
                throw (new Exception("An error occurred while encrypting string"));
            }
        }

        #region MD5 ENCRYPTION
        
        private static byte[] ToByteArray(object value)
        {
            byte[] result = new byte[] { };
            string val = value as string;
            if (val != null)
                result = new UnicodeEncoding().GetBytes(val);
            return result;
        }
        public static string Tomd5(string value)
        {
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(ToByteArray(value));
            string result = BitConverter.ToString(hash);
            result = result.Replace("-", "");
            return result;
        }
        #endregion MD5 ENCRYPTION
    }
}