using Microsoft.Xna.Framework;
using SadConsole;
using TechDemo1.UI;

namespace TechDemo1.MapObjects
{
    public class EmptyCell : CellAppearance
    {
        public EmptyCell() : base(Color.Transparent, Color.Transparent, 0)
        {
        }
    }
}
