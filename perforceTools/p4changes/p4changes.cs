using System;
using System.Collections.Generic;
using System.Linq;
using Perforce.P4;

namespace JFG.Perforce
{
    class p4changes
    {
        static void Main(string[] args)
        {
            // initialize the connection variables
            // note: this is a connection without using a password
            string user, ws_client, user_pw;
            string uri = "oslo.famillegratton.net:1818";
            user = ws_client = "devtest";
            user_pw = "d3vtest0";


            // define the server, repository and connection
            Server server = new Server(new ServerAddress(uri));
            Repository rep = new Repository(server);
            Connection con = rep.Connection;


            // use the connection varaibles for this connection
            con.UserName = user;
            con.Client = new Client();
            con.Client.Name = ws_client;


            // connect to the server
            con.Connect(null);
            if (con.Login(user_pw) == null)
            {
                Console.WriteLine("Failed");
                Environment.Exit(0);
            }

            //string clientName = ws_client;
            int maxItems = 50;
            Options opts = new Options(ChangesCmdFlags.LongDescription,
                    null, maxItems, ChangeListStatus.None, null);


            // run the command against the current repository
            IList<Changelist> changes = rep.GetChangelists(opts);
            con.Disconnect(null);

            IList<Changelist> CL = changes.Reverse().ToArray();
            //int x = 0;
            int x = CL.Count - 1;
            while (x >= 0)
            {
                Console.WriteLine("#{0}: Change {1} on {2} by {3}@{4} : {5}", x, CL[x].Id, CL[x].ModifiedDate, CL[x].OwnerName, CL[x].ClientId, CL[x].Description);
                --x;
            }

        }
    }
}