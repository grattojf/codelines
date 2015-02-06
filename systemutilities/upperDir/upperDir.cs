using System;
using System.IO;
using JFG.SysUtils.Files;

namespace upperDir
{
    internal class upperdir
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("UpperDir v1 -- Ecrit par J.F.Gratton, 2011");
                Console.WriteLine("upperdir repertoire");
                Console.WriteLine("repertoire = repertoire contenant les fichiers a modifier");
                return;
            }

            if (Directory.Exists(args[0]) == false)
            {
                Console.WriteLine("{0} n'existe pas", args[0]);
            }
            int nNumber = FileUtils.UpperDir(args[0]);

            if (args.Length > 1)
                if (args[1] == "1" || args[1].ToLower() == "/v" || args[1].ToLower() == "-v")
                    Console.WriteLine("Nombre de fichiers modifies: {0}", nNumber);
        }
    }
}