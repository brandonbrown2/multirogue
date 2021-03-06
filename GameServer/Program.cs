﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechDemo1.Map;
using Lidgren;
using Lidgren.Network;

namespace GameServer
{
    class Program
    {
        static public int port = 2025;
        static private NetServer server;
        static private Thread clientHandler;
        static private Simulation game;
        static void Main(string[] args)
        {
            
            //TODO: Create Map
            //TODO: Init Outgoing Data Handler thread
            //TODO: Init Simulation(?)
            Console.WriteLine("Good port!");
            game = new GameServer.Simulation();
            Console.WriteLine("Map Generated!");
            var config = new NetPeerConfiguration("Multirogue") { Port = port };
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("Server Initialized!");
            ClientHandler c = new ClientHandler(server, game);
            clientHandler = new Thread(c.ClientMessageRecieverThread);
            clientHandler.Start();
            Console.WriteLine("Client Handler Initialized!");
        }
    }
}
