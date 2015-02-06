// perforceTools : p4setClient-cmd
// Écrit par : jfgratton (), 2014.03.30 @ 16:52
// 
// InstanceOps.cs


using System;
using System.Collections;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace JFG.Perforce
{
    public static partial class P4SetClient
    {
        private static P4ConnectStruct SelectCorrectInstance(MySqlConnection _conn, ArrayList alResultats)
        {
            if (alResultats.Count == 1)
                return (P4ConnectStruct) alResultats[0];

            Console.Clear();
            Console.WriteLine("{0} Instances match your criteria.\n", alResultats.Count);

            for (int i = 0; i < alResultats.Count; i++)
            {
                P4ConnectStruct p4Instance = (P4ConnectStruct) alResultats[i];
                Console.WriteLine("{0}. {1} (owner = {2})\n", i, p4Instance.P4PORT, p4Instance.p4InstanceOwner);
            }

            Console.WriteLine("Which one are talking about (1-{0}, 0 to abort): ");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
            } while (Char.ToLower(cki.KeyChar) < '0' || Char.ToLower(cki.KeyChar) > alResultats.Count - 1);

            if (cki.KeyChar == '0')
                Environment.Exit(0);
            return (P4ConnectStruct) alResultats[cki.KeyChar - '0'];
        }


        private static void WriteP4Config(string p4Exec, P4ConnectStruct p4c)
        {
            string sErreur = "";
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                sErreur = WriteConfig(p4c);
            else
            {
                Process proc = new Process();
                proc.StartInfo.FileName = String.IsNullOrWhiteSpace(p4Exec) ? "p4.exe" : p4Exec;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                
                // go throught all P4 variables
                proc.StartInfo.Arguments = "set P4PORT=" + p4c.P4PORT;
                proc.Start();

                if (String.IsNullOrWhiteSpace(p4c.P4USER) == false)
                {
                    proc.StartInfo.Arguments = "set P4USER=" + p4c.P4USER;
                    proc.Start();
                }
                if (String.IsNullOrWhiteSpace(p4c.P4PASSWD) == false)
                {
                    proc.StartInfo.Arguments = "set P4PASSWD=" + p4c.P4PASSWD;
                    proc.Start();
                }
                if (String.IsNullOrWhiteSpace(p4c.P4CLIENT) == false)
                {
                    proc.StartInfo.Arguments = "set P4CLIENT=" + p4c.P4CLIENT;
                    proc.Start();
                }
            }

            if (String.IsNullOrWhiteSpace(sErreur) == false)
                Console.WriteLine("Error : {0}", sErreur);
    //? Environment.GetEnvironmentVariable("HOME")
    //: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");t
        }
    }
}