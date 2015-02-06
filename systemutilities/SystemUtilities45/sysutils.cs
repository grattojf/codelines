/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * sysutils.cs : fonctions utilitaires generiques
*/

using System;
using System.Text;

namespace JFG.SysUtils
{
    /// <summary>
    /// Core static system utilities
    /// <para>Those are the very first methods that constituted SysUtils in C++, way back then, in 1993</para>
    /// </summary>
    public static class sysutils
    {
        #region SI

        /// <summary>
        /// International numbering (3 digits, a comma, 3 digits)
        /// Original code in C by Jean-François Gauthier, 1993
        /// </summary>
        /// <param name="iNombre"> Number to "recode"</param>
        /// <returns> The number in International numbering convention</returns>
        public static string SI(int iNombre)
        {
            string strN;
            StringBuilder strbR = new StringBuilder("");
            int nLen, nPos = 0;

            strN = iNombre.ToString();
            reverse(ref strN);
            nLen = strN.Length;

            while (nPos < nLen)
            {
                if (nPos != 0 && (nPos % 3 == 0))
                {
                    strbR.Append(',');
                    strbR.Append(strN[nPos]);
                }
                else
                    strbR.Append(strN[nPos]);
                ++nPos;
            }

            strN = strbR.ToString();
            reverse(ref strN);

            return strN;
        }

        /// <summary>
        /// International numbering (3 digits, a comma, 3 digits)
        /// Original code in C by Jean-François Gauthier, 1993
        /// </summary>
        /// <param name="iNombre"> Number to "recode"</param>
        /// <returns> The number in International numbering convention</returns>
        public static string SI(ulong iNombre)
        {
            string strN;
            StringBuilder strbR = new StringBuilder("");
            int nPos = 0;

            strN = iNombre.ToString();
            reverse(ref strN);
            int nLen = strN.Length;

            while (nPos < nLen)
            {
                if (nPos != 0 && (nPos % 3 == 0))
                {
                    strbR.Append(',');
                    strbR.Append(strN[nPos]);
                }
                else
                    strbR.Append(strN[nPos]);
                ++nPos;
            }

            strN = strbR.ToString();
            reverse(ref strN);

            return strN;
        }

        #endregion SI

        #region STRING UTILITIES
		private static void reverse(ref string forward)
        {
            string fwd = forward;

            forward = Reverse(fwd);
        }
        					 
        /// <summary>
        /// Returns a reversed string (spelled backwards)
        /// Thus, ABCDE becomes EDCBA
        /// </summary>
        /// <param name="fwdString"> String to reverse</param>
        /// <returns> Reversed string</returns>
        public static string Reverse(string fwdString)
        {
            if (String.IsNullOrEmpty(fwdString))
                return null;

            string revString;

            int nLen = fwdString.Length - 1;

            revString = "";
            while (nLen >= 0)
                revString += fwdString[nLen--];

            return revString;
        }

        /// <summary>
        /// Keeps a given number of character of the string in argument, starting from the left
        /// </summary>
        /// <param name="nLongueur"> Number of characters to keep</param>
        /// <param name="full"> String to take the characters off</param>
        /// <returns> The kept characters</returns>
        public static string Left(int nLongueur, string full)
        {
            if (String.IsNullOrEmpty(full) || nLongueur < 0)
                return null;

            if (nLongueur >= full.Length)
                return full;

            return full.Substring(0, nLongueur);
        }

        /// <summary>
        /// Keeps a given number of characters off the given string in argument, starting from the right
        /// </summary>
        /// <param name="nLongueur"> Number of characters to keep</param>
        /// <param name="full"> The string to take the characers off</param>
        /// <returns> The resulting string</returns>
        public static string Right(int nLongueur, string full)
        {
            if (string.IsNullOrEmpty(full) || nLongueur < 0)
                return null;

            if (nLongueur >= full.Length)
                return full;

            return full.Substring(full.Length - nLongueur, nLongueur);
        }

        #endregion STRING UTILITIES
    }
}