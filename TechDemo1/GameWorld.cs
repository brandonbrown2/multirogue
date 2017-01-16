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

        /// <summary>
        /// Called one time to initiate everything. Assumes SadConsole has been setup and is ready to go.
        /// </summary>
        public static void Start()
        {
            MainScreen = new GameScreen();
            SadConsole.Engine.ConsoleRenderStack.Add(MainScreen);
            MainScreen.MessageConsole.PrintMessage("Welcome to THE GAME...");

        }
    }
}
