using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechDemo1.Map;
using Lidgren;
using Lidgren.Network;
using RogueSharp.Random;

namespace GameServer
{
    class Program
    {
        static private int port;
        static private NetServer server;
        static private Thread clientHandler;
        static private int mapSeed;
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: GameServer.exe [port]");
                Console.ReadKey();
                return;
            }
            
            if (!Int32.TryParse(args[0], out port))
            {
                Console.WriteLine("Bad port argument!");
                Console.ReadKey();
            }
            else if (port > 0)
            {
                //TODO: Create Map
                //TODO: Init Outgoing Data Handler thread
                //TODO: Init Simulation(?)
                Console.WriteLine("Good port!");
                var randomgen = new RogueSharp.Random.DotNetRandom();
                mapSeed = randomgen.Next(Int32.MaxValue - 1);
                Console.WriteLine("Map Generated!");
                var config = new NetPeerConfiguration("Multirogue Server") { Port = port };
                server = new NetServer(config);
                server.Start();
                Console.WriteLine("Server Initialized!");
                ClientHandler c = new ClientHandler(server, mapSeed);
                clientHandler = new Thread(c.ClientMessageRecieverThread);
                Console.WriteLine("Client Handler Initialized!");
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
