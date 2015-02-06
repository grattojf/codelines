/*  SysUtils v1.3.1
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * fileutils-search.cs : methodes pour FindFile()
*/

using System;
using System.IO;

namespace JFG.SysUtils.Files
{
    /// <summary>
    /// JFG.SysUtils.Files.FileUtils : file helpers
    /// </summary>
    public static partial class FileUtils
    {
        private static string[] ParcoureArborescence(DirectoryInfo rootDir, string fichierATrouver = "*", bool bPrintProgress = false)
        {
            FileInfo[] fi = null;
            DirectoryInfo[] di = null;
            string[] sFichiers = null;

            try
            {
                fi = rootDir.GetFiles(fichierATrouver);
            }
            catch (UnauthorizedAccessException)
            {
                // pour le moment on ne fait rien avec cette exception, mais qui sait, un jour...
            }
            catch (DirectoryNotFoundException)
            {
                // same goes here   
            }

            catch (Exception eee)
            {
                sFichiers = new string[2];
                sFichiers[0] = "EXCEPTION";
                sFichiers[1] = eee.Message;
            }

            if (fi != null)
            {
                int n = 0, sz = fi.Length;
                string[] sPartialFileFound = new string[sz];

                if (sz > 0)
                {
                    foreach (FileInfo f in fi)
                        sPartialFileFound[n++] = f.FullName;
                    sFichiers = buildReturnArray(sPartialFileFound, sFichiers);
                }
            }
            try
            {
                di = rootDir.GetDirectories();
            }

            catch (UnauthorizedAccessException)
            {
                // pour le moment on ne fait rien avec cette exception, mais qui sait, un jour...
            }
            catch (DirectoryNotFoundException)
            {
                // same goes here   
            }

            catch (Exception eee)
            {
                sFichiers = new string[2];
                sFichiers[0] = "EXCEPTION";
                sFichiers[1] = eee.Message;
            }
            if (di != null)
            {
                foreach (DirectoryInfo directoryInfo in di)
                {
                    if (bPrintProgress == true)
                        Console.WriteLine(directoryInfo.FullName);
                    string[] sPart = ParcoureArborescence(directoryInfo, fichierATrouver, bPrintProgress);
                    sFichiers = buildReturnArray(sPart, sFichiers);
                }
            }

            return sFichiers;
        }

        private static string[] buildReturnArray(string[] partial, string[] full)
        {
            if (partial == null)
                return full;

            if (full == null)
                return partial;

            int p = partial.Length, f = full.Length;

            string[] complete = new string[p + f];

            partial.CopyTo(complete, 0);
            full.CopyTo(complete, p);
            return complete;
        }
    }
}