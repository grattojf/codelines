// fileinformation v1.50
// ecrit par J.F.Gratton, septembre 2011 - janvier 2012

using System;
using JFG.SysUtils;
using JFG.SysUtils.Files;


// fi [{-s | --short} | {-f | --full}] filename
namespace fi
{
    internal static class Fi
    {
        private static void help()
        {
            Console.WriteLine("usage: ");
            Console.WriteLine("fi {[-v | --verbose] [-vv | --loud]} fichier");
        }

        private static void Main(string[] args)
        {
            int nNiveauVerbosite = 0;
            string sFichier = "";
            Console.WriteLine("fi (File Info) v1.50.00");
            Console.WriteLine("Ecrit par J.F.Gratton, 2011 - 2012");
            Console.WriteLine();

            if (args.Length < 1)
            {
                Console.Clear();
                Console.WriteLine("usage:");
                Console.WriteLine("fi [[-v|--verbose]|[-vv|--loud] nom_de_fichier");
                return;
            }
            switch (args[0].ToLower())
            {
                case "-v":
                case "--verbose":
                    nNiveauVerbosite = 1;
                    sFichier = args[1];
                    break;
                case "-vv":
                case "--loud":
                    nNiveauVerbosite = 2;
                    sFichier = args[1];
                    break;
                default:
                    nNiveauVerbosite = 0;
                    sFichier = args[0];
                    break;
            }
            
            fi_struct fiStruct = FileUtils.FileInformation(sFichier);

            if (fiStruct.Diagnostics.EndsWith("n'existe pas"))
            {
                Console.WriteLine("Le fichier {0} n'existe pas", sFichier);
                return;
            }

            Console.WriteLine("Nom : {0}", fiStruct.OriginalFilename);
            Console.Write("Taille : {0} octets (", sysutils.SI((ulong)fiStruct.Taille));
            Console.WriteLine(fiStruct.ReadOnly ? "Read only)" : "Read/write)");

            if (nNiveauVerbosite == 0 || String.IsNullOrWhiteSpace(fiStruct.FileVersion))
                return;

            if (nNiveauVerbosite > 0)
            {
                Console.WriteLine("Créé le : {0}", fiStruct.DtCreated.ToString());
                Console.WriteLine("Modifié le : {0}", fiStruct.DtWritten.ToString());
            }

            if (nNiveauVerbosite > 1)
            {
                Console.WriteLine("Version du logiciel : {0}", fiStruct.ProductVersion);
                Console.WriteLine("Copyright : {0}", fiStruct.LegalCopyright);
                Console.WriteLine("Compagnie : {0}", fiStruct.CompanyName);
            }
        }
    }
}