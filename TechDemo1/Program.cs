﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Consoles.Console;
using SadConsole.Consoles;
using TechDemo1.NetworkClient;
using Microsoft.Xna.Framework;
using TechDemo1.Entities;
using System.Threading;
using TechDemo1.UI;
using SadConsole;
using TechDemo1.Entities.Wrappers;

namespace TechDemo1
{
    class Program
    {
        public static GameWindow window;
        public static CharacterInstanceWrapper secondPlayer;
        public static bool singleplayer = true;
        private static int myEntityID = 0;
        private static Point myStartingPoint = new Point(1, 1);
        public static NetworkClient.NetworkClient net;
        private static IDictionary<int, CharacterInstanceWrapper> PlayerDictionary;
        static void Main(string[] args)
        {
            net = new NetworkClient.NetworkClient();
            net.seedReceived += StartYourEngine;
            net.spawnMe += SetMultiPlayerData;
            net.gameType += SetSinglePlayerStatus;
            net.addPlayer = addRemotePlayer;

            System.Console.Write("IP : ");
            String ip = System.Console.ReadLine();
            System.Console.Write("Port : ");
            String port = System.Console.ReadLine();

            try
            {
                net.ConnectToServer(ip, int.Parse(port));
            } catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.ReadKey();
                StartYourEngine(null, new Random().Next());
            }
        }

        public static void StartYourEngine(object sender, int seed) {
            GameWorld.seed = seed;

            // Setup the engine and creat the main window.
            SadConsole.Engine.Initialize("Assets/Fonts/IBM2x.font", 90, 24, (g) => {
                window = g.Window;
            });

            // Hook the start event so we can add consoles to the system.
            SadConsole.Engine.EngineStart += Engine_EngineStart;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Engine.EngineUpdated += Engine_EngineUpdated;

            // Start the game.

            SadConsole.Engine.Run();
        }

        private static void Engine_EngineStart(object sender, EventArgs e)
        {
            UIConstants.init();
            // Clear the default console
            SadConsole.Engine.ConsoleRenderStack.Clear();
            SadConsole.Engine.ActiveConsole = null;

            GameWorld.Start(window);
            MakeSinglePlayerStuff();
        }

        private static void Engine_EngineUpdated(object sender, EventArgs e)
        {

        }

        public static void SetMultiPlayerData(object sender, int entityID, Point startLoc)
        {
            myEntityID = entityID;
            myStartingPoint = startLoc;
        }

        public static void MakeSinglePlayerStuff()
        {
            EntityGenerator.GenerateLocalCharacter(myEntityID, myStartingPoint);
            if (singleplayer)
            {
                secondPlayer = EntityGenerator.GenerateRemoteCharacter();
                new Thread(() =>
                {
                    Random r = new Random();
                    while (true)
                    {
                        secondPlayer.SetTarget(new Point(r.Next(30), r.Next(30)));
                        Thread.Sleep(r.Next(500, 2500));
                    }
                }).Start();
            }
            else
            {
                net.RequestEntityList();
            }
        }

        public static void SetSinglePlayerStatus(object sender, bool gameType)
        {
            singleplayer = gameType;
        }

        public static void addRemotePlayer(object sender, int entityID, Point current, Point destination)
        {
            if(myEntityID != entityID)
            {
                EntityGenerator.GenerateRemoteCharacter(entityID, current, destination);
            }
        }

    }
}
