using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1.UI;

namespace TechDemo1
{
    static class GameWorld
    {
        public static GameScreen MainScreen;
        public static GameWindow window;
        public static int seed;
        
        /// <summary>
        /// Called one time to initiate everything. Assumes SadConsole has been setup and is ready to go.
        /// </summary>
        public static void Start(GameWindow window)
        {
            UIConstants.init();
            GameWorld.window = window;
            window.AllowUserResizing = true;

            MainScreen = new GameScreen(new RogueSharp.Random.DotNetRandom(seed));
            SadConsole.Engine.ConsoleRenderStack.Add(MainScreen);
            MainScreen.MessageConsole.PrintMessage("Welcome to THE GAME...");
        }
    }
}
