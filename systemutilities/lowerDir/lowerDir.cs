using System;
using System.IO;
using JFG.SysUtils.Files;

namespace lowerDir
{
    internal class lowerdir
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("LowerDir v1 -- Ecrit par J.F.Gratton, 2011");
                Console.WriteLine("lowerdir repertoire");
                Console.WriteLine("repertoire = repertoire contenant les fichiers a modifier");
                return;
            }

            if (Directory.Exists(args[0]) == false)
            {
                Console.WriteLine("{0} n'existe pas", args[0]);
            }
            int nNumber = FileUtils.LowerDir(args[0]);

            if (args.Length > 1)
                if (args[1] == "1" || args[1].ToLower() == "/v" || args[1].ToLower() == "-v")
                    Console.WriteLine("Nombre de fichiers modifies: {0}", nNumber);
        }
    }
}