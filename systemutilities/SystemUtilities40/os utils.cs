/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * generic OS Utils.cs : OS-specific utilities
*/

using System;
using System.IO;

namespace JFG.SysUtils.OSutils
{
    public struct di_struct
    {
        public ulong totalSpace, usedSpace, freeSpace;
        public string driveType, volumeName, driveName;
    };

    public class diskInformation
    {
        private string driveLetter;

        #region INIT

        public diskInformation(string letter)
        {
            if (String.IsNullOrEmpty(letter))
                driveLetter = "";
            else
                driveLetter = letter[0] + @":\";
        }

        #endregion INIT

        public bool getInfo(string driveLetter, ref di_struct di)
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