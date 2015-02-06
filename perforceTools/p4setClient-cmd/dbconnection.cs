// perforceTools : p4setClient-cmd
// Écrit par : jfgratton (), 2014.03.23 @ 17:15
// 
// dbconnection.cs : gestion de la connexion vers la bd mysql


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace JFG.Perforce
{

    public static partial class P4SetClient
    {
        private static void GetConnectionInfo(ref DBConnectStruct dbc)
        {
            Console.Write("Post [localhost]: ");
            if (String.IsNullOrWhiteSpace((dbc.sHost = Console.ReadLine())))
                dbc.sHost = "localhost";
            Console.Write("Port [3306]:");
            if (Int32.TryParse(Console.ReadLine(), out dbc.nPort) == false)
                dbc.nPort = 3306;
            Console.Write("Database [perforce_web]: ");
            if (String.IsNullOrWhiteSpace((dbc.sDB = Console.ReadLine())))
                dbc.sDB = "perforce_web";
            Console.Write("User [perforce]: ");
            if (String.IsNullOrWhiteSpace((dbc.sUser = Console.ReadLine())))
                dbc.sUser = "perforce";
            Console.Write("Password [perforce]: ");
            if (String.IsNullOrWhiteSpace((dbc.sPasswd = ReadPassword())))
                dbc.sPasswd = "perforce";
        }

        // taken from http://www.dotfusion.net/tag/c-console-password-masking
        private static string ReadPassword(bool bShowStars = true)
        {
            Stack<string> pass = new Stack<string>();

            for (ConsoleKeyInfo consKeyInfo = Console.ReadKey(true);
              consKeyInfo.Key != ConsoleKey.Enter; consKeyInfo = Console.ReadKey(true))
            {
                if (consKeyInfo.Key == ConsoleKey.Backspace)
                {
                    try
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        pass.Pop();
                    }
                    catch (InvalidOperationException)
                    {
                        /* Nothing to delete, go back to previous position */
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else
                {
                    if (bShowStars)
                        Console.Write("*");
                    pass.Push(consKeyInfo.KeyChar.ToString(CultureInfo.InvariantCulture));
                }
            }
            String[] password = pass.ToArray();
            Array.Reverse(password);
            return string.Join(string.Empty, password);
        }


        private static MySqlConnection TryDBConnect(DBConnectStruct dbc, out string sErreur)
        {
            sErreur = "";
            MySqlConnection _conn = new MySqlConnection("User Id=" + dbc.sUser + ";Password=" + dbc.sPasswd + ";Host=" + dbc.sHost +
                ";Database=" + dbc.sDB + ";Port=" + dbc.nPort);

            try
            {
                _conn.Open();
            }

            catch (MySqlException mEX)
            {
                sErreur = mEX.Message;
                _conn = null;
            }
            
            //Console.WriteLine("SUCCESS");
            return _conn;
        }

        //sWhereClause = "SELECT name,alias,port FROM p4_instances WHERE ";
        private static bool GoFetchData(MySqlConnection conn, string selectClause, ref P4ConnectStruct p4c, 
            out string errorMsg, out ArrayList alRes)
        {
            MySqlCommand cmd = new MySqlCommand(selectClause, conn);
            MySqlDataReader rdr = null;

            errorMsg = "";
            alRes = new ArrayList();
            bool resultats = true;

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    p4c.P4NAME = rdr[0].ToString();
                    p4c.P4PORT = (rdr[1].ToString()) + ":" + (rdr[2].ToString());
                    p4c.p4InstanceOwner = rdr[3].ToString();
                    alRes.Add(p4c);
                }
            }
            catch (MySqlException mEX)
            {
                errorMsg = mEX.Message;
                resultats = false;
            }

            return resultats;
        }
    }
}