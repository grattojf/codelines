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

    /// <summary>
    /// Very simple DNS resolver (IP to hostname, hostname to IP)
    /// </summary>
    public class DNSResolver
    {
        /// <summary>
        /// Resolves the hostname to its IP address
        /// </summary>
        /// <param name="host"> Hostname to resolve (string)</param>
        /// <returns> The host's IP address (string)</returns>
        /// <exception cref="System.Net.Sockets.SocketException"></exception>
        public static string Hostname2IP(string host)
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

        /// <summary>
        /// Resolves an IP address to its hostname
        /// </summary>
        /// <param name="ip"> The host's IP address (string)</param>
        /// <returns> The hostname (string)</returns>
        /// <exception cref="System.Net.Sockets.SocketException"></exception>
        public static string IP2Hostname(string ip)
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

    /// <summary>
    /// Good old Ping utility
    /// </summary>
    public class Ping
    {
        private PingOptions options = new PingOptions();
        private System.Net.NetworkInformation.Ping pong = new System.Net.NetworkInformation.Ping();
        private int timeout;
        private string host_address;
        private bool bDisplayInfo;

        /// <summary>
        /// First constructor
        /// </summary>
        /// <param name="host"> Hostname to ping</param>
        /// <param name="pingoptions"> Various ping options <see cref="System.Net.NetworkInformation.PingOptions"/></param>
        /// <param name="nTO"> Timeout value in ms</param>
        /// <param name="simpleDisplay"> Show basic information</param>
        public Ping(string host, ref PingOptions pingoptions, int nTO = 120, bool simpleDisplay = true) : base()
        {
            options = pingoptions;
            timeout = nTO;
            host_address = host;
            bDisplayInfo = simpleDisplay;
        }

        /// <summary>
        /// Sedond constructor
        /// </summary>
        /// <param name="host"> Hostname to ping</param>
        /// <param name="ttl"> Time to live in ms</param>
        /// <param name="nTO"> Timeout value in ms</param>
        /// <param name="bFrag"> Allows packet fragmentation</param>
        /// <param name="simpleDisplay"> Show basic information</param>
        public Ping(string host, int ttl = 128, int nTO = 120, bool bFrag = true, bool simpleDisplay = true) : base()
        {
            options.Ttl = ttl;
            options.DontFragment = bFrag;
            timeout = 120;
            host_address = host;
            bDisplayInfo = simpleDisplay;
        }

        /// <summary>
        /// The ubiquitous ping command
        /// </summary>
        /// <returns> Host is responding or not</returns>
        /// <exception cref="System.Net.NetworkInformation.PingException"></exception>
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