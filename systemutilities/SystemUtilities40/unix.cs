/*  SysUtils v5.00.00
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * unix.cs : fonctions & utilitaires UNIX
*/

using System;
using System.IO;

namespace JFG.SysUtils.UNIX
{
    public class unixCommands
    {
        #region GREP

        // grep : retourne affiche la ligne si la ligne contient le pattern
        // premiere version : le pattern, suivi du fichier a verifier, toggle -v, et toggle -i
        public static string grep(string pattern, string fichier, bool bExclude, bool bCaseSensitive)
        {
            bool bInclude = !bExclude;

            FileInfo fex = new FileInfo(fichier);
            if (!fex.Exists)
                return "";

            string ligne;
            FileStream fs = new FileStream(fichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while ((ligne = sr.ReadLine()) != null)
            {
                bool bEnd, bStart;
                string match = pattern;
                bEnd = bStart = false;

                if (pattern.StartsWith("^"))
                {
                    bStart = true;
                    match = pattern.Remove(0, 1);
                }
                if (pattern.EndsWith("$"))
                {
                    bEnd = true;
                    match = pattern.Remove(pattern.Length, 1);
                }

                if (bEnd == true && bStart == true)
                    break;

                if (bStart == true && ((bCaseSensitive == true && ligne.StartsWith(match) == bInclude) ||
                    (bCaseSensitive == false && ligne.ToLower().StartsWith(match.ToLower()) == bInclude)))
                    Console.WriteLine(ligne);

                if (bEnd == true && ((bCaseSensitive == true && ligne.EndsWith(match) == bInclude) ||
                    (bCaseSensitive == false && ligne.ToLower().EndsWith(match.ToLower()) == bInclude)))
                    Console.WriteLine(ligne);

                if (bStart == false && bEnd == false && ((bCaseSensitive == true && ligne.Contains(match) == bInclude) ||
                (bCaseSensitive == false && ligne.ToLower().Contains(match.ToLower()) == bInclude)))
                    Console.WriteLine(ligne);
            }

            sr.Close();
            return "";
        }

        // seconde version : le pattern, le fichier a verifier, le fichier d'output, toggles -v et -i
        public static string grep(string pattern, string source, string dest, bool bExclude, bool bCaseSensitive)
        {
            bool bInclude = !bExclude;

            FileInfo fex = new FileInfo(source);
            if (!fex.Exists)
                return "";

            string ligne;
            FileStream fsR = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fsW = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            StreamReader sr = new StreamReader(fsR);
            StreamWriter wr = new StreamWriter(fsW);

            while ((ligne = sr.ReadLine()) != null)
            {
                bool bEnd, bStart;
                string match = pattern;
                bEnd = bStart = false;

                if (pattern.StartsWith("^"))
                {
                    bStart = true;
                    match = pattern.Remove(0, 1);
                }
                if (pattern.EndsWith("$"))
                {
                    bEnd = true;
                    match = pattern.Remove(pattern.Length, 1);
                }

                if (bEnd == true && bStart == true)
                    break;

                if (bStart == true && ((bCaseSensitive == true && ligne.StartsWith(match) == bInclude) ||
                    (bCaseSensitive == false && ligne.ToLower().StartsWith(match.ToLower()) == bInclude)))
                    wr.WriteLine(ligne);

                if (bEnd == true && ((bCaseSensitive == true && ligne.EndsWith(match) == bInclude) ||
                    (bCaseSensitive == false && ligne.ToLower().EndsWith(match.ToLower()) == bInclude)))
                    wr.WriteLine(ligne);

                if (bStart == false && bEnd == false && ((bCaseSensitive == true && ligne.Contains(match) == bInclude) ||
                (bCaseSensitive == false && ligne.ToLower().Contains(match.ToLower()) == bInclude)))
                    wr.WriteLine(ligne);
            }

            sr.Close(); wr.Close();
            fsR.Dispose(); fsW.Dispose();
            return "";
        }

        // troisieme version : le pattern, le fichier a verifier, #occurences, toggles -v et -i
        public static void grep(string pattern, string fichier, out int nCount, bool bExcl, bool bCaseSensitive)
        {
            bool bIncl = !bExcl;

            FileInfo fex = new FileInfo(fichier);
            if (!fex.Exists)
                nCount = -1;

            string ligne;
            FileStream fs = new FileStream(fichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            nCount = 0;
            while ((ligne = sr.ReadLine()) != null)
            {
                if ((bCaseSensitive == true && ligne.Contains(pattern) == bIncl) ||
                (bCaseSensitive == false && ligne.ToLower().Contains(pattern.ToLower()) == bIncl))
                    ++nCount;
            }

            sr.Close();
            fs.Dispose();
        }

        #endregion GREP

        #region AWK

        // Premiere version: un seul champs a prendre

        // Dans cet exemple:
        // ligne = "123 456 789 000"
        // field = 3 (789)
        // separator = ' '
        // nLen = 15
        // 123 456 789 000
        // 012345678901234
        // 000000000011111
        public static void awk(string infile, int field, char separator = ' ')
        {
            int nPos, nLen, nInit, nNext, nCount;
            string ligne;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while ((ligne = sr.ReadLine()) != null)
            {
                nLen = ligne.Length;
                nPos = nNext = nInit = nCount = 0;
                while (nPos < nLen)
                {
                    if (ligne[nPos] == separator)
                    {
                        ++nCount;
                        if (nCount == field - 1)
                            nInit = nPos;
                        if (nCount == field)
                            nNext = nPos;
                    }
                    ++nPos;
                }
                if (nInit != 0)
                    Console.WriteLine(ligne.Substring(nInit + 1, nNext - nInit - 1));
            }

            sr.Close(); sr.Dispose();
            fs.Close(); fs.Dispose();
        }

        // seconde version : plusieurs champs
        public static void awk(string infile, int[] fields, char separator = ' ')
        {
            int nFLen = fields.Length;
            int[] nPos = new int[nFLen]; int[] nInit = new int[nFLen];
            int[] nNext = new int[nFLen]; int[] nCount = new int[nFLen];

            string ligne;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while ((ligne = sr.ReadLine()) != null)
            {
                int x = 0;
                while (x < nFLen)
                {
                    int nLength = ligne.Length;
                    nPos[x] = nNext[x] = nInit[x] = nCount[x] = 0;
                    while (nPos[x] < nLength)
                    {
                        if (ligne[nPos[x]] == separator)
                        {
                            ++nCount[x];
                            if (nCount[x] == fields[x] - 1)
                                nInit[x] = nPos[x];
                            if (nCount[x] == fields[x])
                                nNext[x] = nPos[x];
                        }
                        ++nPos[x];
                    }
                    if (nInit[x] != 0)
                        Console.Write(ligne.Substring(nInit[x] + 1, nNext[x] - nInit[x] - 1));
                    ++x;
                }
                Console.WriteLine(Environment.NewLine);
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
            }
        }

        #endregion AWK

        #region WC-L

        public static int wcl(string infile)
        {
            int nCount = 0;
            string ligne;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while ((ligne = sr.ReadLine()) != null)
            {
                ++nCount;
            }

            sr.Close(); sr.Dispose();
            fs.Close(); fs.Dispose();

            return nCount;
        }

        #endregion WC-L
    }
}