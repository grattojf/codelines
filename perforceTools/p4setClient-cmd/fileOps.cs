// perforceTools : p4setClient-cmd
// Écrit par : jfgratton (), 2014.03.23 @ 18:26
// 
// fileOps.cs : lecture du fichier de config, ecriture de .p4config

using System;
using System.IO;

namespace JFG.Perforce
{
    public static partial class P4SetClient
    {
        private static bool ReadConfigFile(string cfgFile, ref string p4ClientExecutable, ref DBConnectStruct dbc, ref P4ConnectStruct p4c)
        {
            string s, ligne;

            if (File.Exists(cfgFile) == false)
                return false;
            var sr = new StreamReader(cfgFile);

            dbc.nPort = 3306;

            while ((ligne = sr.ReadLine()) != null)
            {
                switch (ligne.ToLower())
                {
                    case "[p4user]":
                        if ((p4c.P4USER = sr.ReadLine()) == null)
                            return false;
                        break;
                    case "[p4clientexec]":
                        if ((p4ClientExecutable = sr.ReadLine()) == null)
                            return false;
                        break;
                    case "[serveur]":
                    case "[server]":
                    case "[host]":
                        if ((dbc.sHost = sr.ReadLine()) == null)
                            return false;
                        break;
                    case "[db]":
                    case "[bd]":
                    case "[database]":
                        if ((dbc.sDB = sr.ReadLine()) == null)
                            return false;
                        break;
                    case "[port]":
                        if ((s = sr.ReadLine()) == null)
                            return false;
                        if (Int32.TryParse(s, out dbc.nPort) == false)
                            dbc.nPort = 3306;
                        break;
                    case "[user]":
                        if ((dbc.sUser = sr.ReadLine()) == null)
                            return false;
                        break;
                    case "[passwd]":
                    case "[password]":
                        if ((dbc.sPasswd = sr.ReadLine()) == null)
                            return false;
                        break;
                }
            }
            if (sr != null)
                sr.Close();
            return !String.IsNullOrWhiteSpace(dbc.sHost) && !String.IsNullOrWhiteSpace(dbc.sDB) &&
                !String.IsNullOrWhiteSpace(dbc.sUser) && !String.IsNullOrWhiteSpace(dbc.sPasswd);
        }

        // p4passwd:q1w2e3
        // length = 15; passwd length = 6; index = 8
        //private static string CutLine(string ligne)
        //{
        //    int a = ligne.IndexOf(':', 0);
        //    return ligne.Substring(a + 1, ligne.Length - a);
        //}

        private static void ReadPasswordFile(string cfgfile, ref P4ConnectStruct p4c)
        {
            string l;
            var sr = new StreamReader(cfgfile);

            if ((l = sr.ReadLine()) != null)
                p4c.P4PASSWD = l;

            if (sr != null)
                sr.Close();
        }

        //public struct P4ConnectStruct
        //{
        //    public String /*p4passwdfile, */P4NAME, P4CLIENT, P4PASSWD, P4PORT, p4InstanceOwner;
        //};
        private static string WriteConfig(P4ConnectStruct p4c)
        {
            string sErreur = "";

            StreamWriter sw = new StreamWriter(Environment.GetEnvironmentVariable("HOME"));

            try
            {
                sw.WriteLine("P4PORT={0}", p4c.P4PORT);
                if (String.IsNullOrWhiteSpace(p4c.P4USER) == false)
                    sw.WriteLine("P4USER={0}", p4c.P4USER);
                if (String.IsNullOrWhiteSpace(p4c.P4PASSWD) == false)
                    sw.WriteLine("P4PASSWD={0}", p4c.P4PASSWD);
                if (String.IsNullOrWhiteSpace(p4c.P4CLIENT) == false)
                    sw.WriteLine("P4CLIENT={0}", p4c.P4CLIENT);
                if (String.IsNullOrWhiteSpace(p4c.P4NAME) == false)
                    sw.WriteLine("P4NAME={0}", p4c.P4NAME);
            }
            catch (IOException ioex)
            {
                sErreur = ioex.Message;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
            return sErreur;
        }
    }
}