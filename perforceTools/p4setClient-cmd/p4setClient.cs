// perforceTools : p4setClient-cmd
// p4setClient.cs : main file
// 2014.02.21 13:10, Jean-Francois Gratton

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using MySql.Data.MySqlClient;


 /* Workflow :
 * 1) find the given instance in PerforceDB.
 * 2) if more than one instance on the same port (eg. 1666), offer a choice.
 * 3) fetch info on that instance
 * 4) populate variables
 * 5) create .p4config file with proper info
 * 6) close all connections
*/

// command-line arguments :
// p4setClient -cfg configfile [-p4 p4passwdfile] {-p port|-l like|-i instance}

namespace JFG.Perforce
{
    public struct DBConnectStruct
    {
        public String sUser, sPasswd, sHost, sDB;
        public int nPort;
    };

    public struct P4ConnectStruct
    {
        public String /*p4passwdfile, */P4USER, P4NAME, P4CLIENT, P4PASSWD, P4PORT, p4InstanceOwner;
    };

    public static partial class P4SetClient
    {
        private static void Main(string[] args)
        {
            DBConnectStruct dbConnection = new DBConnectStruct();
            P4ConnectStruct p4Connection = new P4ConnectStruct();
            MySqlConnection _conn;
            string sSelect, sP4PwdFile, sConfigFile, sErreur, p4ClientExecutable;
            sSelect = sP4PwdFile = sConfigFile = p4ClientExecutable = "";
            ArrayList alResultats;
            
            if (ParseCommandLine(args, ref sSelect, ref sConfigFile, ref sP4PwdFile) == false)
            {
                ShowHelp();
                Environment.Exit(0);
            }
            dbConnection.nPort = 3306;  //default value, in case the parameter is not in the config file
            #region MySQL connection config
            if (ReadConfigFile(sConfigFile, ref p4ClientExecutable, ref dbConnection, ref p4Connection) == false)
            {
                ConsoleKeyInfo cki;
                Console.WriteLine("{0} cannot continue as it needs a valid MySQL connection config file",
                    Assembly.GetExecutingAssembly().GetName().Name);
                Console.Write("Press (1) to manually input the pertinent info, (2) to abort ? [1/2]");
                do
                {
                    cki = Console.ReadKey(true);
                } while (cki.Key != ConsoleKey.D1 && cki.Key != ConsoleKey.D2);
                if (cki.Key == ConsoleKey.D1)
                    GetConnectionInfo(ref dbConnection);
                else
                    Environment.Exit(0);
            }
            #endregion MySQL connection config

            if (String.IsNullOrWhiteSpace(sP4PwdFile) == false && File.Exists(sP4PwdFile))
                ReadPasswordFile(sP4PwdFile, ref p4Connection);

            _conn = new MySqlConnection("User Id=" + dbConnection.sUser + ";Password=" + dbConnection.sPasswd + ";Host=" +
                dbConnection.sHost + ";Database=" + dbConnection.sDB + ";Port=" + dbConnection.nPort);
            if ((_conn = TryDBConnect(dbConnection, out sErreur)) == null)
            {
                Console.WriteLine("Connection Error : {0}", sErreur);
                Environment.Exit(0);
            }

            if (GoFetchData(_conn, sSelect, ref p4Connection, out sErreur, out alResultats) == false)
            {
                Console.WriteLine("Error fetching data : {0}", sErreur);
                Environment.Exit(0);
            }

            if (alResultats.Count == 0)
            {
                Console.WriteLine("No instance match the given criterias. Exiting.");
                Environment.Exit(0);
            }

            WriteP4Config (p4ClientExecutable, SelectCorrectInstance(_conn, alResultats));
            Console.WriteLine("Config file created / config applied.");

            _conn.Close();
        }
    }
}