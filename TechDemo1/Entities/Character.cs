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
        public GameMapConsole ParentConsole;
        public bool isMoving;
        
        public Character(GameMapConsole ParentConsole)
        {
            isMoving = false;
            this.ParentConsole = ParentConsole;
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
                    ParentConsole.CenterViewOn(Position);
                    Path.StepForward();
                }
                else
                {
                    RogueSharp.Cell targetCell = Path.CurrentStep;
                    MoveTo(new Point(targetCell.X, targetCell.Y));
                    ParentConsole.CenterViewOn(Position);
                    isMoving = false;
                }
            }
        }
    }
}
