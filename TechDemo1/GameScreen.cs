﻿using Microsoft.Xna.Framework;
using SadConsole.Consoles;
using System;
using Console = SadConsole.Consoles.Console;
using SadConsole.Input;

namespace TechDemo1
{
    class GameScreen : SadConsole.Consoles.ConsoleList
    {
        public GameMapConsole ViewConsole;
        public StatsPanel StatsConsole;
        public ChatPanel MessageConsole;

        private Console messageHeaderConsole;

        public GameScreen()
        {
            StatsConsole = new StatsPanel(52, 37);
            ViewConsole = new GameMapConsole(127, 37, 300, 300);
            //ViewConsole.FillWithRandomGarbage(); // Temporary so we can see where the console is on the screen
            MessageConsole = new ChatPanel(127, 11);

            // Setup the message header to be as wide as the screen but only 1 character high
            messageHeaderConsole = new Console(180, 1);
            messageHeaderConsole.DoUpdate = false;
            messageHeaderConsole.CanUseKeyboard = true;
            messageHeaderConsole.CanUseMouse = false;

            SadConsole.Engine.ActiveConsole = this;

            // Draw the line for the header
            messageHeaderConsole.Fill(Color.White, Color.Black, 196, null);
            messageHeaderConsole.SetGlyph(127, 0, 193); // This makes the border match the character console's left-edge border

            // Print the header text
            messageHeaderConsole.Print(2, 0, " Messages ");

            // Move the rest of the consoles into position (ViewConsole is already in position at 0,0)
            StatsConsole.Position = new Point(127, 0);
            MessageConsole.Position = new Point(0, 38);
            messageHeaderConsole.Position = new Point(0, 37);

            // Add all consoles to this console list.
            Add(messageHeaderConsole);
            Add(StatsConsole);
            Add(ViewConsole);
            Add(MessageConsole);
            CanUseKeyboard = true;

            // Placeholder stuff for the stats screen
            StatsConsole.CharacterName = "Fred";
            StatsConsole.MaxHealth = 200;
            StatsConsole.Health = 100;
        }
        public override bool ProcessKeyboard(KeyboardInfo info)
        {
            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down)))
            {
                ViewConsole.MovePlayerBy(new Point(0, 1));
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up)))
            {
                ViewConsole.MovePlayerBy(new Point(0, -1));
            }

            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right)))
            {
                ViewConsole.MovePlayerBy(new Point(1, 0));
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left)))
            {
                ViewConsole.MovePlayerBy(new Point(-1, 0));
            }

            return false;
        }
    }
}
