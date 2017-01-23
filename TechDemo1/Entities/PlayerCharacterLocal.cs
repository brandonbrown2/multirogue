using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;
using SadConsole.Game;
using SadConsole;
using TechDemo1.UI;

namespace TechDemo1.Entities
{
    public class PlayerCharacterLocal : Character
    {
        public SadConsole.Game.GameObject target;

        public PlayerCharacterLocal(GameMapConsole ParentConsole) : base(ParentConsole)
        {
            Font = Engine.DefaultFont;
            Animation = UIConstants.playerAnimation;
            Position = new Point(1, 1);

            target = new GameObject(Engine.DefaultFont);
            target.Animation = UIConstants.targetAnimation;
            target.Position = new Point(1, 1);
            isFocus = true;
        }
        public override void Render()
        {
            lock (positionLock) lock (destinationLock)
                {
                    base.Render();
                    if (target.Position != position)
                    {
                        target.Render();
                    }
                }
        }
        public override void SetRenderOffset(Point offset)
        {
            RenderOffset = offset;
            target.RenderOffset = offset;
        } 
        public override void Shift(Point amount)
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
        public override void SetDestination(Point newDestination)
        {
            if (ParentConsole.isWithinMapAndScreen(newDestination))
            {
                target.Position = newDestination;
                Destination = newDestination;
            }
        }
        public void ShiftDestination(Point amount)
        {
            SetDestination(target.Position + amount);
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
