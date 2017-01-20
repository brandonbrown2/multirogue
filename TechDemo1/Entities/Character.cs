using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RogueSharp;
using SadConsole.Game;

namespace TechDemo1.Entities
{
    class Character : GameObject
    {
        public Path Path;
        public Point Destination;
        public GameMapConsole ParentConsole;
        public bool isMoving, isFocus;
        protected int currentTicks, maxTicks;


        public Character(GameMapConsole ParentConsole)
        {
            isFocus = false;
            currentTicks = 0;
            maxTicks = 5;
            isMoving = false;
            this.ParentConsole = ParentConsole;
        }
        public override void Update()
        {
            currentTicks++;
            if ((currentTicks >= maxTicks) && (isMoving))
            {
                currentTicks = 0;
                MoveTowardsTarget();
            }
        }
        public override void Render()
        {
            if (ParentConsole.isWithinScreen(position))
            {
                base.Render();
            }
        }
        public virtual void Move(Point amount)
        {
            Position += amount;
        }
        public virtual void MoveTo(Point newPosition)
        {
            Position = newPosition;
        }
        public virtual void MoveTowardsTarget()
        {
            if ((Path != null) && isMoving)
            {
                if (Path.CurrentStep != Path.End)
                {
                    RogueSharp.Cell targetCell = Path.CurrentStep;
                    MoveTo(new Point(targetCell.X, targetCell.Y));
                    if (isFocus)
                    {
                        ParentConsole.CenterViewOn(Position);
                    }
                    Path.StepForward();
                }
                else
                {
                    RogueSharp.Cell targetCell = Path.CurrentStep;
                    MoveTo(new Point(targetCell.X, targetCell.Y));
                    if (isFocus)
                    {
                        ParentConsole.CenterViewOn(Position);
                    }
                    isMoving = false;
                }
            }
        }
    }
}
