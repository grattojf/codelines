using System;
using JFG.SysUtils.Networking;

namespace resolver
{
    internal class resolver
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("resolver {host|ip address}");
                return;
            }

            string hostOrAddress = args[0].ToLower();
            string sDNSResult;
            char cFirstDigit = hostOrAddress[0];

            if (cFirstDigit >= 'a' && cFirstDigit <= 'z')
                sDNSResult = DNSResolver.Hostname2IP(hostOrAddress);
            else
                sDNSResult = DNSResolver.IP2Hostname(hostOrAddress);

            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0}", hostOrAddress);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" résolve sur: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}", sDNSResult);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}