using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1.Map;
using Lidgren;
using Lidgren.Network;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: GameServer.exe [port]");
                Console.ReadKey();
                return;
            }

            int port;
            if (!Int32.TryParse(args[0], out port))
            {
                Console.WriteLine("Bad port argument!");
                Console.ReadKey();
            }
            else if (port > 0)
            {
                //TODO: Create Map
                //TODO: Init Incoming Data Handler thread
                //TODO: Init Outgoing Data Handler thread
                //TODO: Init Simulation(?)
                var config = new NetPeerConfiguration("Multirogue Server") { Port = port };
                var server = new NetServer(config);
                server.Start();
                Console.WriteLine("Good port!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Port must be greater than zero!");
                Console.ReadKey();
            }
        }
    }
}
