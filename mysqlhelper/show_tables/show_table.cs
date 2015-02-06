using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JFG.mySQLhelper;

namespace show_tables
{
    class show_table
    {
        static void Main(string[] args)
        {
            ArrayList alListe = new ArrayList();
            ConnectionHelper connHelp = new ConnectionHelper("User ID=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;");
            connHelp.OpenConnection();
            Console.Clear();

            if (connHelp.ShowTables(ref alListe, "comptabilite") == false)
                Console.WriteLine(connHelp.GetExceptionMessage());
            else
            {
                alListe.Sort();
                Console.WriteLine("TABLES");
                foreach (string s in alListe)
                    Console.WriteLine(s);
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            if (connHelp.ShowTables(ref alListe, "comptabilite", "201%") == false)
                Console.WriteLine(connHelp.GetExceptionMessage());
            else
            {
                alListe.Sort();
                Console.WriteLine("TABLES");
                foreach (string s in alListe)
                    Console.WriteLine(s);
            }
        }
    }
}