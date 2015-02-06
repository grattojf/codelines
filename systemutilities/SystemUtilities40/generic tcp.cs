/*  SysUtils v1.2.2
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *  
 * generic tcp.cs : fonctions de reseautique
*/

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace JFG.SysUtils.Networking
{
    #region DNS

    public class dnsResolver
    {
        public static string hostname2ip(string host)
        {
            string ip = null;

            try
            {
                ip = Dns.GetHostEntry(host).AddressList[0].ToString();
            }

            catch (System.Net.Sockets.SocketException sEx)
            {
                ip = sEx.Message;
            }

            catch (ArgumentException aEx)
            {
                ip = aEx.Message;
            }

            catch (Exception e)
            {
                ip = e.Message;
            }

            return ip;
        }

        public static string ip2hostname(string ip)
        {
            string host = null;

            try
            {
                host = Dns.GetHostEntry(ip).HostName;
            }

            catch (System.Net.Sockets.SocketException sEx)
            {
                host = sEx.Message;
            }

            catch (ArgumentException aEx)
            {
                host = aEx.Message;
            }

            catch (Exception e)
            {
                host = e.Message;
            }

            return host;
        }
    }

    #endregion DNS

    #region PING

    public class Ping
    {
        private PingOptions options = new PingOptions();
        private System.Net.NetworkInformation.Ping pong = new System.Net.NetworkInformation.Ping();
        private int timeout;
        private string host_address;
        private bool bDisplayInfo;

        public Ping(string host, ref PingOptions pingoptions, int nTO = 120,
            bool simpleDisplay = true)
            : base()
        {
            options = pingoptions;
            timeout = nTO;
            host_address = host;
            bDisplayInfo = simpleDisplay;
        }

        public Ping(string host, int ttl = 128, int nTO = 120, bool bFrag = true,
            bool simpleDisplay = true)
            : base()
        {
            options.Ttl = ttl;
            options.DontFragment = bFrag;
            timeout = 120;
            host_address = host;
            bDisplayInfo = simpleDisplay;
        }

        public bool ping()
        {
            string data = "SysUtils.Networking.Ping.pingJFG";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply reply;
            //bool bRes;

            try
            {
                reply = pong.Send(host_address, timeout, buffer, options);
                if (bDisplayInfo == true)
                {
                    Console.WriteLine("Address: {0}", reply.Address.ToString());
                    Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                    Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                    Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                    Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                }

                if (reply.Status == IPStatus.Success)
                    return true;
                else
                    return false;
            }

            catch (PingException pEx)
            {
                Console.Write("PingException: ");
                Console.WriteLine(pEx.InnerException.Message);
                return false;
            }
        }
    }

    #endregion PING
}