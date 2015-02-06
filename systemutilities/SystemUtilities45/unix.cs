/*  SysUtils v5.00.00
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * unix.cs : fonctions & utilitaires UNIX
*/

using System;
using System.IO;

namespace JFG.SysUtils.UNIX
{
    /// <summary> Some basic Unix utils such as grep, awk, wc . More to come ... :-) </summary>
    public class unixCommands
    {
        #region GREP

        /// <summary> Unix utility 'grep' . First variant : grep with -v and -i toggles </summary>
        /// <param name="pattern"> Pattern to look for</param>
        /// <param name="fichier"> File to look at</param>
        /// <param name="bExclude"> Toggle -v</param>
        /// <param name="bCaseSensitive"> Toggle -i</param>
        /// <exception cref="IOException"></exception>
        public static void grep(string pattern, string fichier, bool bExclude, bool bCaseSensitive)
        {
            bool bInclude = !bExclude;

            FileInfo fex = new FileInfo(fichier);
            if (!fex.Exists)
            {
                Console.WriteLine("{0} n'existe pas");
                return;
            }
            string ligne;
            FileStream fs = new FileStream(fichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            try
            {
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

                    if (bEnd && bStart)
                        break;

                    if (bStart && ((bCaseSensitive && ligne.StartsWith(match) == bInclude) ||
                                   (bCaseSensitive == false && ligne.ToLower().StartsWith(match.ToLower()) == bInclude)))
                        Console.WriteLine(ligne);

                    if (bEnd && ((bCaseSensitive && ligne.EndsWith(match) == bInclude) ||
                                 (bCaseSensitive == false && ligne.ToLower().EndsWith(match.ToLower()) == bInclude)))
                        Console.WriteLine(ligne);

                    if (bStart == false && bEnd == false && ((bCaseSensitive && ligne.Contains(match) == bInclude) ||
                                                             (bCaseSensitive == false &&
                                                              ligne.ToLower().Contains(match.ToLower()) == bInclude)))
                        Console.WriteLine(ligne);
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
            finally
            {
                sr.Close();
            }
        }

		/// <summary> <para> Unix utility 'grep' . Second variant : grep with -v and -i toggles</para>
        /// <para> The command's output will be sent to a file</para>
        /// </summary>
        /// <param name="pattern"> Pattern to look for</param>
        /// <param name="source"> File to look at</param>
        /// <param name="dest"> File to send output at</param>
        /// <param name="bExclude"> Toggle -v</param>
        /// <param name="bCaseSensitive"> Toggle -i</param>
        /// <exception cref="IOException"></exception>
        public static void grep(string pattern, string source, string dest, bool bExclude, bool bCaseSensitive)
        {
            bool bInclude = !bExclude;

            FileInfo fex = new FileInfo(source);
            if (!fex.Exists)
            {
                Console.WriteLine("{0} n'existe pas");
                return;
            }

            string ligne;
            FileStream fsR = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fsW = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            StreamReader sr = new StreamReader(fsR);
            StreamWriter wr = new StreamWriter(fsW);

            try
            {
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

                    if (bEnd && bStart)
                        break;

                    if (bStart && ((bCaseSensitive && ligne.StartsWith(match) == bInclude) ||
                        (bCaseSensitive == false && ligne.ToLower().StartsWith(match.ToLower()) == bInclude)))
                        wr.WriteLine(ligne);

                    if (bEnd && ((bCaseSensitive && ligne.EndsWith(match) == bInclude) ||
                        (bCaseSensitive == false && ligne.ToLower().EndsWith(match.ToLower()) == bInclude)))
                        wr.WriteLine(ligne);

                    if (bStart == false && bEnd == false && ((bCaseSensitive && ligne.Contains(match) == bInclude) ||
                         (bCaseSensitive == false && ligne.ToLower().Contains(match.ToLower()) == bInclude)))
                        wr.WriteLine(ligne);
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }

            finally
            {
                sr.Close(); wr.Close();
                fsR.Dispose(); fsW.Dispose();
            }
        }

        /// <summary> Unix utility 'grep' . Third variant : grep with -v and -i toggles and occurence count </summary>
        /// <param name="pattern"> Pattern to look for</param>
        /// <param name="fichier"> File to look at</param>
        /// <param name="nCount"> Number of occurences</param>
        /// <param name="bExcl"> Toggle -v</param>
        /// <param name="bCaseSensitive"> Toggle -i</param>
        public static void grep(string pattern, string fichier, out int nCount, bool bExcl, bool bCaseSensitive)
        {
            bool bIncl = !bExcl;

            FileInfo fex = new FileInfo(fichier);
            if (!fex.Exists)
            {
                Console.WriteLine("{0} n'existe pas");
                nCount = -1;
                return;
            }

            string ligne;
            FileStream fs = new FileStream(fichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            nCount = 0;
            try
            {
                while ((ligne = sr.ReadLine()) != null)
                {
                    if ((bCaseSensitive && ligne.Contains(pattern) == bIncl) ||
                        (bCaseSensitive == false && ligne.ToLower().Contains(pattern.ToLower()) == bIncl))
                        ++nCount;
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
            finally
            {
                sr.Close();
                fs.Dispose();
            }
        }

        #endregion GREP

        #region AWK
        
        /// <summary> Unix utility 'awk'. First version : fetches a single field from the line </summary>
        /// <param name="infile"> Source file</param>
        /// <param name="field"> Field to fetch</param>
        /// <param name="separator"> Field separator (default is ' ')</param>
        /// <exception cref="IOException"></exception>
        /// <para>Dans cet exemple:
        /// ligne = "123 456 789 000"
        /// field = 3 (789)
        /// separator = ' '
        /// nLen = 15
        /// 123 456 789 000
        /// 012345678901234
        /// 000000000011111</para>
        public static void awk(string infile, int field, char separator = ' ')
        {
            int nPos, nLen, nInit, nNext, nCount;
            string ligne;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            try
            {
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
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
            finally
            {
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
            }
        }

        /// <summary>  Unix utility 'awk'. Second version : fetches multiple fields from the line </summary>
        /// <param name="infile"> Source file</param>
        /// <param name="fields"> Fields to fetch</param>
        /// <param name="separator"> Field separator (default is ' ')</param>
        /// <exception cref="IOException"></exception>
        public static void awk(string infile, int[] fields, char separator = ' ')
        {
            int nFLen = fields.Length;
            int[] nPos = new int[nFLen]; int[] nInit = new int[nFLen];
            int[] nNext = new int[nFLen]; int[] nCount = new int[nFLen];

            string ligne;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            try
            {
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
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
            finally
            {
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
            }
        }

        #endregion AWK

        #region WC-L

        /// <summary>
        /// Unix utility 'wc -l' : line count
        /// </summary>
        /// <param name="infile"> File to count lines from</param>
        /// <returns> Number of lines in the file, -1 if errors</returns>
        /// <exception cref="IOException"></exception>
        public static int wcl(string infile)
        {
            int nCount = 0;
            FileStream fs = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            try
            {
                while ((sr.ReadLine()) != null)
                    ++nCount;
            }
            catch (IOException)
            {
                nCount = -1;
            }
            finally
            {
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
            }
            return nCount;
        }

        #endregion WC-L
    }
}