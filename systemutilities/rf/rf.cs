using System;
using System.IO;
using JFG.SysUtils;

// l:620, s:55, j:85, f:245

// usage:
// rf DIRECTORY {pix|mov|all}

namespace renamer
{
    public enum RenameEnum
    {
        undefined = -1,
        extension = 0,
        photos = 1,
        films = 2,
        tout = 3
    };

    internal class renommePhotos
    {
        private static void Main(string[] args)
        {
            RenameEnum rE = RenameEnum.undefined;
            string sExtension = null;
            string sCurrentDirectory = Directory.GetCurrentDirectory();

            if (args.Length < 2)
                showHelp();

            switch (args[1].ToLower())
            {
                case "-pix":
                case "-photos":
                    rE = RenameEnum.photos;
                    break;

                case "-mov":
                case "-films":
                    rE = RenameEnum.films;
                    break;

                case "-all":
                case "-tout":
                    rE = RenameEnum.tout;
                    break;

                case "-ext":
                    rE = RenameEnum.extension;
                    if (args.Length < 3)
                        showHelp();
                    else
                        sExtension = args[2].ToLower();
                    break;
            }
            go(args[0], rE, sExtension);
            Directory.SetCurrentDirectory(sCurrentDirectory);
        }

        private static void showHelp()
        {
            Console.Clear();
            Console.WriteLine("USAGE: renommePhotos DIRECTORY {-photos|-films|-tout|-ext .extension}");
            //Console.WriteLine("Note: .extension doit debuter par le point (ex: .jpg, .mpeg");
            Environment.Exit(0);
        }

        private static void go(string repertoire, RenameEnum re, string extension)
        {
            string sDirname;
            int newIndex = 0;

            if (Directory.Exists(repertoire) == false)
            {
                Console.WriteLine("Le repertoire {0} n'existe pas", repertoire);
                return;
            }
            Directory.SetCurrentDirectory(repertoire);
            int index = repertoire.LastIndexOf(@"\");
            if (index == -1)
                sDirname = repertoire;
            else
                sDirname = repertoire.Substring(++index);

            if (extension != null)
            {
                index = extension.LastIndexOf(".");
                string sExt = index == -1 ? extension : extension.Substring(++index);
                batchRename(ref newIndex, sDirname, sExt);
                return;
            }

            switch (re)
            {
                case RenameEnum.films:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "mov");
                    batchRename(ref newIndex, sDirname, "mpeg");
                    batchRename(ref newIndex, sDirname, "mp4");
                    batchRename(ref newIndex, sDirname, "mpg");
                    batchRename(ref newIndex, sDirname, "avi");
                    break;

                case RenameEnum.photos:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "jpeg");
                    batchRename(ref newIndex, sDirname, "jpg");
                    break;

                case RenameEnum.tout:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "jpeg");
                    batchRename(ref newIndex, sDirname, "jpg");
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "mov");
                    batchRename(ref newIndex, sDirname, "mpeg");
                    batchRename(ref newIndex, sDirname, "mp4");
                    batchRename(ref newIndex, sDirname, "mpg");
                    batchRename(ref newIndex, sDirname, "avi");
                    break;
            }
        }

        private static void batchRename(ref int newIndex, string dir, string ext)
        {
            //Directory.SetCurrentDirectory(dir);
            string[] files = Directory.GetFiles(".", "*." + ext);

            Array.Sort(files);
            foreach (string f in files)
            {
                string intermediateName = getDigits(ref newIndex, f);
                if (intermediateName == null)
                    continue;
                //if (f.ToLower().StartsWith(".\\img_") == true)
                {
                    string sNewName = dir.ToLower() + " - " + intermediateName + "." + ext;
                    Console.WriteLine(f.ToLower().Substring(2) + " -> " + sNewName);
                    File.Move(f, sNewName);
                }
            }
        }

        private static string getDigits(ref int index, string file)
        {
            int nPos = 0, nExtPos = file.LastIndexOf("."), nRealIndex;
            string ff = file, strDigits = "";
            bool charEncountered = false;

            // stripping off of the extension
            if (nExtPos > 0) // has an extension
                ff = file.Substring(0, nExtPos);

            // ok, on commence le travail...
            string reverseFile = sysutils.Reverse(ff);

            while (nPos < reverseFile.Length && !charEncountered)
            {
                if (reverseFile[nPos] >= '0' && reverseFile[nPos] <= '9')
                    strDigits += reverseFile[nPos++];
                else
                    charEncountered = true;
            }

            if (nPos == 0)
                nRealIndex = ++index;
            else
                return sysutils.Reverse(strDigits);

            return nRealIndex.ToString();
        }
    }
}