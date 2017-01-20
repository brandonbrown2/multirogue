using Microsoft.Xna.Framework;
using SadConsole;
using TechDemo1.UI;

namespace TechDemo1.MapObjects
{
    public class Grass : CellAppearance
    {
        public Grass() : base(Color.LightGreen, Color.Transparent, 46)
        {
            this.GlyphIndex = 176;
            Foreground = new Color(0, (float)(.35 + UIConstants.rand.NextDouble() * .2 ), 0);
        }
    }
}
