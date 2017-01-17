using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1;


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
            //TODO: Create Map
            //TODO: Init Server thread
            //TODO: Init Incoming Data Handler thread
            //TODO: Init Outgoing Data Handler thread
            //TODO: Init Simulation(?)
        }
    }
}
