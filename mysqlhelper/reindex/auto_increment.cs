using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JFG.mySQLhelper;

namespace reindex
{
    class auto_increment
    {
        static void Main(string[] args)
        {
            ConnectionHelper connHelp = new ConnectionHelper("User ID=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;");
            connHelp.OpenConnection();
            Console.Clear();

            bool bOK = connHelp.ResetAutoIncrement("User ID=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;", "comptabilite", "TEST", "pid");

            Console.WriteLine(bOK);
        }
    }
}
