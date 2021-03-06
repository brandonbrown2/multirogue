﻿using Microsoft.Xna.Framework;
using SadConsole.Consoles;
using System;
using Console = SadConsole.Consoles.Console;
using SadConsole.Input;
using TechDemo1.Entities;

namespace TechDemo1
{
    class GameScreen : SadConsole.Consoles.ConsoleList
    {
        public GameMapConsole ViewConsole;
        public StatsPanel StatsConsole;
        public ChatPanel MessageConsole;
        private int keyWaitCounter, keyWait = 5;

        private Console messageHeaderConsole;
        private bool firstKey;

        public GameScreen() : this(new System.Random())
        {

        }
        public GameScreen(System.Random r)
        {
            keyWaitCounter = 0;
            firstKey = true;
            StatsConsole = new StatsPanel(26, 18);
            ViewConsole = new GameMapConsole(63, 18, 150, 150, r);
            //ViewConsole.FillWithRandomGarbage(); // Temporary so we can see where the console is on the screen
            MessageConsole = new ChatPanel(63, 11);

            // Setup the message header to be as wide as the screen but only 1 character high
            messageHeaderConsole = new Console(90, 1);
            messageHeaderConsole.DoUpdate = false;
            messageHeaderConsole.CanUseKeyboard = true;
            messageHeaderConsole.CanUseMouse = false;

            SadConsole.Engine.ActiveConsole = this;
            SadConsole.Engine.UseMouse = true;

            // Draw the line for the header
            messageHeaderConsole.Fill(Color.White, Color.Black, 196, null);
            messageHeaderConsole.SetGlyph(63, 0, 193); // This makes the border match the character console's left-edge border

            // Print the header text
            messageHeaderConsole.Print(2, 0, " Messages ");

            // Move the rest of the consoles into position (ViewConsole is already in position at 0,0)
            StatsConsole.Position = new Point(63, 0);
            MessageConsole.Position = new Point(0, 19);
            messageHeaderConsole.Position = new Point(0, 18);

            // Add all consoles to this console list.
            Add(messageHeaderConsole);
            Add(StatsConsole);
            Add(ViewConsole);
            Add(MessageConsole);
            CanUseKeyboard = true;
            CanUseMouse = true;

            // Placeholder stuff for the stats screen
            StatsConsole.CharacterName = "Fred";
            StatsConsole.MaxHealth = 200;
            StatsConsole.Health = 100;
        }
        public override bool ProcessKeyboard(KeyboardInfo info)
        {
            int moveScale = 0;
            PlayerCharacterLocal player = ViewConsole.Player;
            if (player != null)
            {
                if (keyWaitCounter >= keyWait)
                {
                    keyWaitCounter = 0;
                    moveScale = 1;
                    if (firstKey)
                    {
                        keyWaitCounter = -keyWait * 2;
                        firstKey = false;
                    }
                }

                if (info.KeysDown.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.LeftShift)))
                {
                    moveScale = moveScale * 5;
                }

                if (info.KeysDown.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down)))
                {
                    keyWaitCounter++;
                    player.ShiftDestination(new Point(0, moveScale));
                }
                else if (info.KeysDown.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up)))
                {
                    keyWaitCounter++;
                    player.ShiftDestination(new Point(0, -moveScale));
                }
                else if (info.KeysDown.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right)))
                {
                    keyWaitCounter++;
                    player.ShiftDestination(new Point(moveScale, 0));
                }
                else if (info.KeysDown.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left)))
                {
                    keyWaitCounter++;
                    player.ShiftDestination(new Point(-moveScale, 0));
                }
                else
                {
                    firstKey = true;
                    keyWaitCounter = keyWait;
                }

                if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.RightShift)))
                {
                    player.MoveTo(player.target.Position);
                }
            }

            return false;
        }
        public override bool ProcessMouse(MouseInfo info)
        {
            base.ProcessMouse(info);
            if (info.LeftClicked)
            {
                MessageConsole.PrintMessage(info.WorldLocation.ToString());
            }
            return false;
        }
    }
}
