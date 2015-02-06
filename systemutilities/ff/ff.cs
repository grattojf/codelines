using System;
using JFG.SysUtils.Files;

/*
public enum sourceRecherche_enum
{
    None = 0, CurrentDrive = 1, AllFixed, AllRemovable,
    AllNetworked, AllFixedAndNetworked, AllFixedAndRemovable, AllRemovableAndNetworked, AllDrives
};
*/

namespace ff
{
    internal class ff
    {
        private static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ff typeRecherche fichier");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("type de recherche:");
            Console.WriteLine("-cdrv = disque actuel");
            Console.WriteLine("-arem = tout les disques éjectables (floppy, usb)");
            Console.WriteLine("-afix = tout les disques fixes");
            Console.WriteLine("-afxr = -afix + -arem");
            Console.WriteLine("-anet = tout les disques réseau");
            Console.WriteLine("-afxn = -afix + -anet");
            Console.WriteLine("-armn = -arem + -anet");
            Console.WriteLine("-all = tout les disques, sans exception");
            Console.WriteLine(Environment.NewLine);
            Environment.Exit(0);
        }

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ff v1 -- Écrit par J.F.Gratton, 2011");
            Console.ForegroundColor = ConsoleColor.White;

            if (args.Length < 2 || args[0].ToLower() == "/h" || args[0].ToLower() == "-h" ||
                args[0] == "/?" || args[1] == "-?")
                Help();

            string[] sFichiersTrouves = null;
            string sFichier = args[1];
            bool bPrintDirs = false;
            SourceRechercheEnum source = SourceRechercheEnum.None;

            if (args[args.Length - 1].ToLower() == "-print")
                bPrintDirs = true;

            switch (args[0].ToLower())
            {
                case "-cdrv":
                    source = SourceRechercheEnum.CurrentDrive;
                    break;

                case "-arem":
                    source = SourceRechercheEnum.AllRemovable;
                    break;

                case "-afix":
                    source = SourceRechercheEnum.AllFixed;
                    break;

                case "-afxr":
                    source = SourceRechercheEnum.AllFixedAndRemovable;
                    break;

                case "-anet":
                    source = SourceRechercheEnum.AllNetworked;
                    break;

                case "-afxn":
                    source = SourceRechercheEnum.AllFixedAndNetworked;
                    break;

                case "-armn":
                    source = SourceRechercheEnum.AllRemovableAndNetworked;
                    break;

                case "-all":
                    source = SourceRechercheEnum.AllDrives;
                    break;

                default:
                    Help();
                    break;
            }
            if (source != SourceRechercheEnum.None)
                sFichiersTrouves = FileUtils.FindFile(source, sFichier, bPrintDirs);

            if (sFichiersTrouves != null)
            {
                Array.Sort(sFichiersTrouves);
                foreach (string fichier in sFichiersTrouves)
                    Console.WriteLine(fichier);
            }
        }
    }
}