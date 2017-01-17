using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Consoles.Console;
using SadConsole.Consoles;
using TechDemo1.NetworkClient;
using Microsoft.Xna.Framework;

namespace TechDemo1
{
    class Program
    {
        public static GameWindow window;
        static void Main(string[] args)
        {
            NetworkClient.NetworkClient net = new NetworkClient.NetworkClient();
            net.seedRecived += StartYourEngine;

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
            }
        }

        public static void StartYourEngine(object sender, int seed) {
            GameWorld.seed = seed;

            // Setup the engine and creat the main window.
            SadConsole.Engine.Initialize("Assets/Fonts/IBM.font", 180, 48, (g) => {
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
            // Clear the default console
            SadConsole.Engine.ConsoleRenderStack.Clear();
            SadConsole.Engine.ActiveConsole = null;

            GameWorld.Start(window);
        }

        private static void Engine_EngineUpdated(object sender, EventArgs e)
        {

        }
    }
}
