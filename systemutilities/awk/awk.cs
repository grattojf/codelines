using System;
using JFG.SysUtils.UNIX;

namespace awk
{
    internal class awk
    {
        /* parametres:
            awk infile, separateur, champ(s)
        */

        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Clear();
                Console.WriteLine("Awk v1.00. Ecrit par J.F.Gratton, 2011");
                Console.WriteLine("awk inFile [-o outFile] [-F separateur] champs");
                return;
            }

            char separator = ' ';
            string infile = args[0], outfile;

            if (args[1].ToLower() == "-o")
                outfile = args[2];

            if (args[3] == "-F")
                separator = args[4][0];

            string[] champs = new string[args.Length - 4];
            for (int i = 4; i < args.Length; champs[i - 4] = args[i++]) ;

        }
    }
}