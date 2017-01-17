using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1
{
    static class GameWorld
    {
        public static GameScreen MainScreen;
        public static GameWindow window;
        
        /// <summary>
        /// Called one time to initiate everything. Assumes SadConsole has been setup and is ready to go.
        /// </summary>
        public static void Start(GameWindow window)
        {
            GameWorld.window = window;
            window.AllowUserResizing = true;
            MainScreen = new GameScreen(new RogueSharp.Random.DotNetRandom(/*Put Seed Here*/));
            SadConsole.Engine.ConsoleRenderStack.Add(MainScreen);
            MainScreen.MessageConsole.PrintMessage("Welcome to THE GAME...");
        }
    }
}
