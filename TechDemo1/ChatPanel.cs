using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1
{
    class ChatPanel : SadConsole.Consoles.Console
    {
        public ChatPanel(int width, int height) : base(width, height)
        {
            /*
            // Draw the side bar
            SadConsole.Shapes.Line line = new SadConsole.Shapes.Line();
            line.EndingLocation = new Point(width - 1, 0);
            line.CellAppearance.GlyphIndex = 196;
            line.UseEndingCell = false;
            line.UseStartingCell = false;
            line.Draw(this);*/
        }
        public void PrintMessage(string text)
        {
            ShiftDown(1);
            VirtualCursor.Print(text).CarriageReturn();
        }

        public void PrintMessage(ColoredString text)
        {
            ShiftDown(1);
            VirtualCursor.Print(text).CarriageReturn();
        }
    }
}
