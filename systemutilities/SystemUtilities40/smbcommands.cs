/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * smbCommands.cs : gestion des shares SMB
*/

using System.Diagnostics;

namespace JFG.SysUtils.SMB
{
    internal class smbCommands
    {
        public smbCommands() { ; }

        public void netView(string outputFile)
        {
            ProcessStartInfo PInfo;
            Process P;
            PInfo = new ProcessStartInfo("cmd", @"/c net view > " + outputFile);
            PInfo.CreateNoWindow = false; 	//nowindow
            PInfo.UseShellExecute = true;	//use shell
            P = Process.Start(PInfo);
            P.WaitForExit(5000); //give it some time to finish
            P.Close();
        }

        public bool netUse() { return true; }

        public bool netUse(string drive, string share, string username, string password, string options)
        { return true; }

        public bool netDelete(string drive) { return true; }
    }
}