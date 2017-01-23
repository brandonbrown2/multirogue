using Microsoft.Xna.Framework;
using SadConsole;
using System;
using TechDemo1.Map;
using TechDemo1.UI;

namespace TechDemo1.MapObjects
{
    public class Tree : CellAppearance, IDeterministicCellAppearance
    {
        public Tree() : base(Color.DarkGreen, Color.Transparent, 5)
        {
        }

        public void Randomize(Random r)
        {
            Foreground = new Color(0, (float)(.35 + r.NextDouble() * .2), 0);
        }
    }
}
