// Solution = sysutils . Projet = SysUtils
// Écrit par : Jean-François Gratton. 2013.03.29 (08:52)
// cryptoutils-decryption.cs : Méthode de décryption

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JFG.SysUtils.CryptoLib
{
    public partial class CryptoUtils
    {
        public static string DecryptString(string Value)
        {
            string str = "";
            _mCsp = SetEnc();
            string iv = "PenS8UCVF7s=";
            _mCsp.IV = Convert.FromBase64String(iv);
            string key = SetLengthString(_key.ToString(), 32);
            _mCsp.Key = Convert.FromBase64String(key);
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            Byte[] byt = new byte[64];
            try
            {
                ct = _mCsp.CreateDecryptor(_mCsp.Key, _mCsp.IV);

                byt = Convert.FromBase64String(Value);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                str = Encoding.UTF8.GetString(ms.ToArray());
            }

            catch (Exception)
            {
                throw (new Exception("An error occurred while decrypting string"));
            }

            return str;
        }
    }
}