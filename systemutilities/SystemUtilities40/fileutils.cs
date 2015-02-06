/*  SysUtils v1.3.1
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * fileutils.cs : fonctions de systeme de fichiers
*/

using System;
using System.Diagnostics;
using System.IO;

public enum SourceRechercheEnum
{
    None = 0, CurrentDrive = 1, AllFixed = 100, AllRemovable,
    AllNetworked, AllFixedAndNetworked, AllFixedAndRemovable, AllRemovableAndNetworked, AllDrives = 100000
};

public struct fi_struct
{
    public DateTime DtCreated, DtAccessed, DtWritten;
    public string Diagnostics, FileVersion, FileSpecialBuild, FilePrivateBuild;
    public string OriginalFilename, LegalTrademarks, LegalCopyright, ProductVersion;
    public string FileDescription, CompanyName, Comments;
    public long Taille;
    public bool ReadOnly, DebugVersion;
};

namespace JFG.SysUtils.Files
{
    public static partial class FileUtils
    {
        public static int LowerDir(string directory)
        {
            DirectoryInfo dInfo = new DirectoryInfo(directory);
            FileInfo[] fiArray = dInfo.GetFiles();
            int nChanged = 0;
            string d = directory;

            if (d.EndsWith("\\") == false)
                d = directory + "\\";

            foreach (FileInfo fi in fiArray)
            {
                if (string.Compare(fi.Name, fi.Name.ToLower()) != 0)
                {
                    string s = fi.Name + ".new";
                    File.Move(d + fi.Name, d + s);
                    if (File.Exists(d + fi.Name.ToLower()))
                        File.Move(d + s, d + fi.Name.ToLower() + nChanged.ToString());
                    else
                        File.Move(d + s, d + fi.Name.ToLower());
                    ++nChanged;
                }
            }
            return nChanged;
        }

        public static int UpperDir(string directory)
        {
            DirectoryInfo dInfo = new DirectoryInfo(directory);
            FileInfo[] fiArray = dInfo.GetFiles();
            int nChanged = 0;
            string d = directory;

            if (d.EndsWith("\\") == false)
                d = directory + "\\";

            foreach (FileInfo fi in fiArray)
            {
                if (string.Compare(fi.Name, fi.Name.ToUpper()) != 0)
                {
                    string s = fi.Name + ".new";
                    File.Move(d + fi.Name, d + s);
                    if (File.Exists(d + fi.Name.ToUpper()))
                        File.Move(d + s, d + fi.Name.ToUpper() + nChanged.ToString());
                    else
                        File.Move(d + s, d + fi.Name.ToUpper());
                    ++nChanged;
                }
            }
            return nChanged;
        }

        public static string[] FindFile(SourceRechercheEnum source, string sFile, bool bPrintProgress = false)
        {
            //DirectoryInfo rootDir;
            //string[] foundFiles = null;
            DriveInfo[] drv = null;
            string[] allDrivesResults = null;

            if (source == SourceRechercheEnum.CurrentDrive)
            {
                DriveInfo di = new DriveInfo(Environment.CurrentDirectory[0] + ":\\");
                return parcoureArborescence(di.RootDirectory, sFile, bPrintProgress);
            }

            drv = DriveInfo.GetDrives();

            foreach (DriveInfo d in drv)
            {
                string[] partialDrives = null;
                switch (d.DriveType)
                {
                    case DriveType.CDRom:
                        if (d.IsReady && (source == SourceRechercheEnum.AllRemovable ||
                            source == SourceRechercheEnum.AllRemovableAndNetworked ||
                            source >= SourceRechercheEnum.AllFixedAndRemovable))
                        {
                            partialDrives = parcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;

                    case DriveType.Fixed:
                        if (d.IsReady && (source == SourceRechercheEnum.AllFixed ||
                                source == SourceRechercheEnum.AllFixedAndNetworked ||
                                source == SourceRechercheEnum.AllFixedAndRemovable ||
                                source == SourceRechercheEnum.AllDrives))
                        {
                            partialDrives = parcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;

                    case DriveType.Network:
                        if (d.IsReady && (source == SourceRechercheEnum.AllFixedAndNetworked ||
                                source == SourceRechercheEnum.AllNetworked ||
                                source >= SourceRechercheEnum.AllRemovableAndNetworked))
                        {
                            partialDrives = parcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;

                    case DriveType.Removable:
                        if (d.IsReady && (source == SourceRechercheEnum.AllRemovable ||
                                source >= SourceRechercheEnum.AllFixedAndRemovable))
                        {
                            partialDrives = parcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;
                }
            }

            //if (allDrivesResults != null)
            //{
            //    Array.Sort(allDrivesResults);
            //    Array.Reverse(allDrivesResults);
            //}
            return allDrivesResults;
        }

        public static fi_struct FileInformation(string sFichier)
        {
            fi_struct informationStruct = new fi_struct();

            if (File.Exists(sFichier) == false)
            {
                informationStruct.Diagnostics = sFichier + "n'existe pas.";
                return informationStruct;
            }

            try
            {
                FileInfo fi = new FileInfo(sFichier);
                FileVersionInfo fvInfo = FileVersionInfo.GetVersionInfo(sFichier);

                informationStruct.DtAccessed = fi.LastAccessTime;
                informationStruct.DtCreated = fi.CreationTime;
                informationStruct.DtWritten = fi.LastWriteTime;
                informationStruct.Taille = fi.Length;
                informationStruct.ReadOnly = fi.IsReadOnly;

                informationStruct.Diagnostics = "fichier non-binaire";
                if (fi.Extension.ToLower() == ".exe" || fi.Extension.ToLower() == ".dll")
                {
                    informationStruct.Diagnostics = "exécutable";
                    informationStruct.Comments = fvInfo.Comments;
                    informationStruct.CompanyName = fvInfo.CompanyName;
                    informationStruct.DebugVersion = fvInfo.IsDebug;
                    informationStruct.FileDescription = fvInfo.FileDescription;
                    informationStruct.FilePrivateBuild = fvInfo.PrivateBuild;
                    informationStruct.FileSpecialBuild = fvInfo.SpecialBuild;
                    informationStruct.FileVersion = fvInfo.FileVersion;
                    informationStruct.LegalCopyright = fvInfo.LegalCopyright;
                    informationStruct.LegalTrademarks = fvInfo.LegalCopyright;
                    informationStruct.OriginalFilename = fvInfo.OriginalFilename;
                    informationStruct.ProductVersion = fvInfo.ProductVersion;
                }
                else
                    informationStruct.OriginalFilename = sFichier;
            }
            catch (Exception e)
            {
                informationStruct.Diagnostics = e.Message;
            }
            return informationStruct;
        }
    }
}