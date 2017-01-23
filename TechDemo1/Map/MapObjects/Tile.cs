using Microsoft.Xna.Framework;
using SadConsole;
using TechDemo1.UI;

namespace TechDemo1.MapObjects
{
    public class Tile : CellAppearance
    {
        public Tile() : base(Color.SlateGray, new Color(.1f,.1f,.1f), 4)
        {
        }
    }
}
