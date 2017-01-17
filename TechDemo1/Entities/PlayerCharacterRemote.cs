using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1.Entities
{
    class PlayerCharacterRemote : Character
    {
        private Random r;
        public PlayerCharacterRemote(GameMapConsole ParentConsole) : base(ParentConsole)
        {
            r = new Random();
        }
        public override void Update()
        {
            base.Update();
            if (r.Next(200) == 1){
                RandomMove();
            }
        }

        public void RandomMove()
        {
            Point newTarget;
            Rectangle renderArea = ParentConsole.TextSurface.RenderArea;
            while (true)
            {
                int x = r.Next(ParentConsole.Width - 1);
                int y = r.Next(ParentConsole.Height - 1);
                if (ParentConsole.rogueMap.IsWalkable(x, y))
                {
                    newTarget = new Point(x, y);
                    break;
                }
            }
            ParentConsole.pathCharacterTo(this, newTarget);
        }
    }
}
