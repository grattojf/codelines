// p4setClient-cmd 
// Written by jfgratton , 2014-03-22
// 
// commandLineHelpers.cs : gestion des options au commandline

using System;
using System.Reflection;

namespace JFG.Perforce
{
    public static partial class P4SetClient
    {
        private static bool ParseCommandLine(string []args, ref string sWhereClause, ref String configfile, ref String p4pwdfile)
        {
            int n = 2, nPortArg;
            bool bResult = true;
            
            if (args.Length == 0 || args[0].ToLower() != "-cfg")
                return false;
            
            configfile = args[1];

            sWhereClause = "SELECT name,alias,port,owner FROM p4_instances WHERE ";
            // command-line arguments :
            // p4setClient -cfg configfile [-p4 p4passwdfile] {-p port|-l like|-i instance}
            while (n < args.Length)
            {
                switch (args[n].ToLower())
                {
                    case "-p":
                        if (Int32.TryParse(args[n + 1], out nPortArg) == false)
                            nPortArg = 1666;
                        sWhereClause += "port = " + nPortArg;
                        
                        n = args.Length;
                        break;
                    case "-l":
                        sWhereClause += "name LIKE '%"+args[n+1]+"%'";
                        n = args.Length;
                        break;
                    case "-i":
                        sWhereClause += "name = '" + args[n+1]+"'";
                        n = args.Length;
                        break;
                    case "-p4":
                        p4pwdfile = args[n + 1];
                        ++n;
                        break;
                    default:
                        ++n;
                        break;
                }
            }
            return bResult;
        }

        private static void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("{0} v{1}\nWritten in 2014 by J.F.Gratton (jean-francois.gratton@ubisoft.com)\n",
                Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("{0} -cfg configfile [-p4 p4passwdfile] [-p port|-l like|-i instance]\n", Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("-cfg configfile : configuration settings for the mySQL database");
            Console.WriteLine("-p4 p4passwdfile : perforce pasword file");
            Console.WriteLine("-p perforce instance port number");
            Console.WriteLine("-l perforce instance approximate name");
            Console.WriteLine("-i perforce instance exact name");
        }
    }
}