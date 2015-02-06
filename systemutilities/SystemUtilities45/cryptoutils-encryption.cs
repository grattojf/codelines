// Solution = sysutils . Projet = SysUtils
// Écrit par : Jean-François Gratton. 2013.03.29 (08:45)
// cryptoutils-encryption.cs : Méthode d'encryption symétrique, et encryption par MD5

using System;
using System.Security.Cryptography;
using System.Text;

namespace JFG.SysUtils.CryptoLib
{
    /// <summary>
    /// JFG.SysUtils.CryptoLib.CryptoUtils : various encryption utilities
    /// </summary>
    public static class CryptoUtils
    {
        private static object _key;
        private static SymmetricAlgorithm _mCsp;

       #region MD5 ENCRYPTION

        /// <summary>
        /// Converts any object into a byte array
        /// </summary>
        /// <param name="value"> The object</param>
        /// <returns> The byte array</returns>
        public static byte[] ToByteArray(object value)
        {
            byte[] result = new byte[] { };
            string val = value as string;
            if (val != null)
                result = new UnicodeEncoding().GetBytes(val);
            return result;
        }
        /// <summary>
        /// Converts a given string to its MD5 representation
        /// </summary>
        /// <param name="value"> The string to convert</param>
        /// <returns> The MD5 value</returns>
        public static string ToMD5(string value)
        {
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(ToByteArray(value));
            string result = BitConverter.ToString(hash);
            result = result.Replace("-", "");
            return result;
        }
        #endregion MD5 ENCRYPTION

        #region HASHES
        /// <summary>
        /// Computes a hash string out of a password string
        /// </summary>
        /// <param name="pwd"> Actual password hash will be computed from</param>
        /// <returns> The hash string computed from</returns>
        public static string CalculateHashForPassword(string pwd)
        {
            HashAlgorithm ha = HashAlgorithm.Create("SHA512Managed");
            if (ha != null)
            {
                byte[] hash = ha.ComputeHash(Encoding.Default.GetBytes(pwd));

                return BitConverter.ToString(hash);
            }

            return pwd; // seulement si ha == null
        }
        #endregion #HASHES
    }
}