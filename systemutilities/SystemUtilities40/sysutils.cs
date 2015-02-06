/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * sysutils.cs : fonctions utilitaires generiques
*/

using System;
using System.Text;

namespace JFG.SysUtils
{
    public static class sysutils
    {
        #region SI

        // SI : retourne (en string) le nombre iNombre, en notation internationale,
        // c-a-d 45893123 devient 45,893,123
        static public string SI(int iNombre)
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

        static public string SI(ulong iNombre)
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

        private static void reverse(ref string forward)
        {
            string fwd = forward;

            forward = Reverse(fwd);
        }

        #endregion SI

        #region STRING UTILITIES

        // Reverse() :
        // Inverse les lettres de la string passee en entree
        // In : string a modifier
        // Out : string modifiee
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

        // Left() :
        // Garde un nombre defini de caracteres de la chaine passee en parametres, en partant de la gauche
        // In : nombre de caracteres a garder, string
        // Out : la string retournee
        public static string Left(int nLongueur, string full)
        {
            if (String.IsNullOrEmpty(full) || nLongueur < 0)
                return null;

            if (nLongueur >= full.Length)
                return full;

            return full.Substring(0, nLongueur);
        }

        // Right() :
        // Garde un nombre defini de caracteres de la chaine passee en parametres, en partant de la droite
        // In : nombre de caracteres a garder, string
        // Out : la string retournee
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