/*  SysUtils v1.3.1
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * fileutils.cs : fonctions de systeme de fichiers
*/

using System;
using System.Diagnostics;
using System.IO;

namespace JFG.SysUtils.Files
{
	/// <summary>
	/// SourceRechercheEnum : type of drives that can be searched
	/// </summary>
	public enum SourceRechercheEnum
	{
		/// <summary> None = no media to search at</summary>
		None = 0,
		/// <summary> CurrentDrive = self-explanatory</summary>
		CurrentDrive = 1,
		/// <summary>AllFixed = Everything but Removable and Networked</summary>
		AllFixed = 100,
		/// <summary>AllRemovable = cdroms/dvds, floppies, usb drives, etc</summary>
		AllRemovable,
		/// <summary>AllNetworked = network shares</summary>
		AllNetworked,
		/// <summary> AllFixedAndNetworked = self-explanatory</summary>
		AllFixedAndNetworked,
		/// <summary>AllFixedAndRemovable = self-explanatory</summary>
		AllFixedAndRemovable,
		/// <summary>AllRemovableAndNetworked = self-explanatory</summary>
		AllRemovableAndNetworked,
		/// <summary>AllDrives = self-explanatory</summary>
		AllDrives = 100000

	};

	/// <summary>
	/// fi_struct : file information structure.
	/// Contains various stats regarding a file
	/// </summary>
	public struct fi_struct
	{
		/// <summary> DtCreated : date created </summary>
		public DateTime DtCreated;
		/// <summary> DtAccessed : date accessed </summary>
		public DateTime DtAccessed;
		/// <summary>  DtWritten : date written </summary>
		public DateTime DtWritten;
		/// <summary> Diagnostics : undocumented </summary>
		public string Diagnostics;
		/// <summary> FileVersion : binary version </summary>
		public string FileVersion;
		/// <summary> FileSpecialBuild: special build version number </summary>	
		public string FileSpecialBuild;
		/// <summary> FilePrivateBuild : private build version number </summary>
		public string FilePrivateBuild;
		/// <summary> OriginalFilename : filename at build time </summary>
		public string OriginalFilename;
		/// <summary>  LegalTrademarks : trademark  </summary>
		public string LegalTrademarks;
		/// <summary> LegalCopyright : copyright info </summary>
		public string LegalCopyright;
		/// <summary> ProductVersion : binary version </summary>
		public string ProductVersion;
		/// <summary> FileDescription : self-explanatory </summary>
		public string FileDescription;
		/// <summary> CompanyName : self-explanatory </summary>
		public string CompanyName;
		/// <summary>  Comments : self-explanatory </summary>
		public string Comments;
		/// <summary> Taille : file size </summary>
		public long Taille;
		/// <summary> ReadOnly : is the r/o flag set ? </summary>
		public bool ReadOnly;
		/// <summary> DebugVersion : is this a debug binary ? </summary>
		public bool DebugVersion;
	};
    public static partial class FileUtils
    {
        /// <summary>
        /// LowerDir : renames files in a given directory to lowercase
        /// </summary>
        /// <param name="directory"> Directory to rename files from</param>
        /// <returns> Number of files renamed</returns>
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

        /// <summary>
        /// UpperDir : renames files in a given directory to uppercase
        /// </summary>
        /// <param name="directory"> Directory to rename files from</param>
        /// <returns> Number of files renamed</returns>
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

        /// <summary>
        /// FindFile : locate a specific file and optionally prints search progress on the console
        /// </summary>
        /// <param name="source"> Source media : dvd, network drive, etc</param>
        /// <param name="sFile"> File to search, default = *</param>
        /// <param name="bPrintProgress">Display progress on the console, default is no</param>
        /// <returns> string array including all paths where the file was found</returns>
        public static string[] FindFile(SourceRechercheEnum source, string sFile = "*", bool bPrintProgress = false)
        {
            DriveInfo[] drv = null;
            string[] allDrivesResults = null;

            if (source == SourceRechercheEnum.CurrentDrive)
            {
                DriveInfo di = new DriveInfo(Environment.CurrentDirectory[0] + ":\\");
                return ParcoureArborescence(di.RootDirectory, sFile, bPrintProgress);
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
                            partialDrives = ParcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
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
                            partialDrives = ParcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
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
                            partialDrives = ParcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;

                    case DriveType.Removable:
                        if (d.IsReady && (source == SourceRechercheEnum.AllRemovable ||
                                source >= SourceRechercheEnum.AllFixedAndRemovable))
                        {
                            partialDrives = ParcoureArborescence(d.RootDirectory, sFile, bPrintProgress);
                            if (partialDrives == null || partialDrives[0] == "EXCEPTION")
                                return partialDrives;
                            allDrivesResults = buildReturnArray(partialDrives, allDrivesResults);
                        }
                        break;
                }
            }
            return allDrivesResults;
        }

        /// <summary>
        /// FileInformation : fetches file info such as binary version, date created, etc
        /// </summary>
        /// <param name="sFichier"> File to fetch info from</param>
        /// <returns> The file information</returns>
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