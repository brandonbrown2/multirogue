using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1.UI;

namespace TechDemo1.Entities
{
    class PlayerCharacterRemote : Character
    {
        private Random r;
        public PlayerCharacterRemote(GameMapConsole ParentConsole) : base(ParentConsole)
        {
            Font = Engine.DefaultFont;
            Animation = UIConstants.SecondPlayerAnimation;
            Position = new Point(1, 1);
        }
    }
}
