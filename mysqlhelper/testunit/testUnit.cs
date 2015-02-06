// MySQLhelper : TestUnit
// testUnit.cs : 
// 2013.05.29 07:27, Jean-Francois Gratton

using System;
using JFG.mySQLhelper;

namespace TestUnit
{
    internal class testUnit
    {
        private static void Main(string[] args)
        {
            int nCount;
            ConnectionHelper connHelp = new ConnectionHelper("User ID=tester;Password=tests;Host=localhost;Port=3306;Database=mysql;");
            connHelp.OpenConnection();

            Console.Clear();
            Console.Write("CheckIfDatabaseExists('f1')  ");
            //Console.WriteLine(connHelp.CheckIfDatabaseExists("f1") ? "*Success*" : "Failure");
            Console.Write("CheckIfDatabaseExists('f11')  ");
            //Console.WriteLine(connHelp.CheckIfDatabaseExists("f11") ? "Success" : "*Failure*");

            Console.Write("CreateDB('f34')  ");
            Console.WriteLine(connHelp.CreateDB("f34") ? "*Success*" : "Failure");
            Console.Write("CreateDB('f34')  ");
            Console.WriteLine(connHelp.CreateDB("f34") ? "Success" : "*Failure*");

            Console.Write("DropDB('f34')  ");
            Console.WriteLine(connHelp.DropDB("f34") ? "*Success*" : "Failure");
            Console.Write("DropDB('f34')  ");
            Console.WriteLine(connHelp.DropDB("f34") ? "Success" : "*Failure*");

            //Create a dummy db and table to perform table tests
            Console.Write("CreateDB('jfg1808')  ");
            Console.WriteLine(connHelp.CreateDB("jfg1808") ? "*Success*" : "Failure");
            Console.Write("CreateTable 'jfg1808.jfg'  ");
            Console.WriteLine(connHelp.CreateTable("jfg1808.jfg",
                                                   @"(`ID` int(11) NOT NULL AUTO_INCREMENT,`name` char(35) NOT NULL DEFAULT '',
                PRIMARY KEY (`ID`)) ENGINE=MyISAM AUTO_INCREMENT=4080 DEFAULT CHARSET=latin1")
                                  ? "*Success*"
                                  : "Failure");
            Console.Write("Execute Insert");
            Console.WriteLine(connHelp.Execute("INSERT INTO jfg1808.jfg (name) VALUES ('jfg')") ? "*Success*" : "Failure");
            Console.Write("Execute Insert");
            Console.WriteLine(connHelp.Execute("INSERT INTO jfg1808.jfg (name) VALUES ('jfg2')") ? "*Success*" : "Failure");
            Console.Write("Execute Insert");
            Console.WriteLine(connHelp.Execute("INSERT INTO jfg1808.jfg (name) VALUES ('jfg3')") ? "*Success*" : "Failure");

            Console.Write("CheckIfTableExists jfg1808,jfg  ");
            Console.WriteLine(connHelp.CheckIfTableExists("jfg1808", "jfg") ? "*Success*" : "Failure");
            Console.Write("CheckIfTableExists jfg1808,jfg2  ");
            Console.WriteLine(connHelp.CheckIfTableExists("jfg1808", "jfg2") ? "Success" : "*Failure*");
            Console.Write("CreateTable 'jfg1808.jfg2'  ");
            Console.WriteLine(connHelp.CreateTable("jfg1808.jfg2", @"(`ID` int(11) NOT NULL AUTO_INCREMENT,`name` char(35) NOT NULL DEFAULT '',
                PRIMARY KEY (`ID`)) ENGINE=MyISAM AUTO_INCREMENT=4080 DEFAULT CHARSET=latin1") ? "*Success*" : "Failure");
            Console.Write("DropTable jfg1808.jfg2");
            Console.WriteLine(connHelp.DropTable("jfg1808.jfg2") ? "*Success*" : "Failure");
            Console.Write("DropTable jfg1808.jfg2");
            Console.WriteLine(connHelp.DropTable("jfg1808.jfg2") ? "Success" : "*Failure*");
            Console.Write("GetColumnCount(jfg1808, jfg)");
            Console.Write("GetRowCount (jfg, jfg1808)");
            nCount = connHelp.GetRowCount("jfg", "jfg1808");
            if (nCount != -1)
                Console.WriteLine("Success : {0} rows", nCount);
            else
                Console.WriteLine("Failure : " + connHelp.GetExceptionMessage());
        }
    }
}