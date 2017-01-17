using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;

namespace TechDemo1.Entities
{
    class PlayerCharacterLocal : Character
    {
        public SadConsole.Game.GameObject target;

        public PlayerCharacterLocal(GameMapConsole ParentConsole) : base(ParentConsole)
        {
            isFocus = true;
        }

        public override void Move(Point amount)
        {
            // Get the position the player will be at
            Point newPosition = ParentConsole.Position + amount;
            // Check to see if the position is within the map
            if (ParentConsole.isWithinMap(newPosition))
            {
                // Move the player
                Position += amount;
                ParentConsole.CenterViewOn(newPosition);
            }
        }
        public void MoveTargetTo(Point newPosition)
        {
            if (ParentConsole.isWithinMapAndScreen(newPosition))
            {
                target.Position = newPosition;
            }
        }
        public void MoveTargetBy(Point amount)
        {
            MoveTargetTo(target.Position + amount);
        }
        public void Teleport()
        {
            RogueSharp.Random.IRandom random = new RogueSharp.Random.DotNetRandom();
            Rectangle renderArea = ParentConsole.TextSurface.RenderArea;
            while (true)
            {
                int x = random.Next(ParentConsole.Width - 1);
                int y = random.Next(ParentConsole.Height - 1);
                if (ParentConsole.rogueMap.IsWalkable(x, y))
                {
                    Position = new Point(x, y);

                    // Center the veiw area
                    ParentConsole.TextSurface.RenderArea = new Rectangle(Position.X - (renderArea.Width / 2),
                                                           Position.Y - (renderArea.Height / 2),
                                                           renderArea.Width, renderArea.Height);
                    break;
                }
            }
            target.Position = Position;
            ParentConsole.CenterViewOn(Position);
        }
    }
}
