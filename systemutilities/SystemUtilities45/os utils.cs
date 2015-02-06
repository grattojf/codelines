/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * generic OS Utils.cs : OS-specific utilities
*/

using System;
using System.IO;

namespace JFG.SysUtils.OSutils
{
    /// <summary>
    /// Drive info structure : some info such as volume space, drive type, name, etc
    /// </summary>
    public struct di_struct
    {
		/// <summary> totalSpace : total drive space </summary>
	    public ulong totalSpace;
		/// <summary> usedSpace : total used space </summary>
		public ulong usedSpace;
		/// <summary> freeSpace : available space to use </summary>
		public ulong freeSpace;
	    /// <summary> driveType : type of media </summary>
	    public string driveType;
	    /// <summary> volumeName : self-explanatory </summary>
	    public string volumeName;
		/// <summary> driveName : self-explanatory </summary>
		public string driveName;
    };

    /// <summary>
    /// Various information on disk/volumes.
    /// Note : this class is barely usefull as it is.
    /// </summary>
    public class DiskInformation
    {
        private string _driveLetter;

        #region INIT

        /// <summary>
        /// Simple constructor to get the drive letter correctly
        /// </summary>
        /// <param name="letter"> The drive letter (string)</param>
        public DiskInformation(string letter)
        {
            if (String.IsNullOrEmpty(letter))
                _driveLetter = "";
            else
                _driveLetter = letter[0] + @":\";
        }

        #endregion INIT

        /// <summary>
        /// Fetches information on the volume, such as avail. space, free space, volume type, etc
        /// </summary>
        /// <param name="driveLetter"> Drive to fetch info from</param>
        /// <param name="di"> drive information (<see cref="di_struct"/>)</param>
        /// <returns> True if completed succesfully, false otherwise</returns>
        public bool GetInfo(string driveLetter, ref di_struct di)
        {
            if (String.IsNullOrEmpty(driveLetter) == true)
                return false;

            DriveInfo drvnfo = new DriveInfo(driveLetter);
            if (drvnfo.IsReady == false)
                return false;

            di.volumeName = drvnfo.VolumeLabel;
            di.driveName = drvnfo.Name;

            switch (drvnfo.DriveType)
            {
                case DriveType.CDRom:
                    di.driveType = "Lecteur optique";
                    break;
                case DriveType.Fixed:
                    di.driveType = "HDD";
                    break;
                case DriveType.Network:
                    di.driveType = "Réseau";
                    break;
                case DriveType.Ram:
                    di.driveType = "RAM";
                    break;
                case DriveType.Removable:
                    di.driveType = "Amovible";
                    break;
                default:
                    di.driveType = "Inconnu / autre";
                    break;
            }
            di.totalSpace = (ulong)drvnfo.TotalSize;
            di.freeSpace = (ulong)drvnfo.TotalFreeSpace;
            di.usedSpace = di.totalSpace - di.freeSpace;

            return true;
        }
    }
}