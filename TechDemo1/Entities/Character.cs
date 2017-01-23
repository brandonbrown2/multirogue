using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RogueSharp;
using SadConsole.Game;

namespace TechDemo1.Entities
{
    public class Character : GameObject
    {
        public object positionLock;
        public object destinationLock;
        public object pathingLock;
        public Path Path;
        public Point Destination;
        public GameMapConsole ParentConsole;
        public bool isMoving, isFocus;
        protected int currentTicks, maxTicks;

        public Character(GameMapConsole ParentConsole)
        {
            positionLock = new object();
            destinationLock = new object();
            pathingLock = new object();
            isFocus = false;
            currentTicks = 0;
            maxTicks = 10;
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
            lock (positionLock) lock (destinationLock)
                {
                    if (ParentConsole.isWithinScreen(position))
                    {
                        base.Render();
                    }
                }
        }
        public virtual void SetRenderOffset(Point offset)
        {
            RenderOffset = offset;
        }
        public virtual void Shift(Point amount)
        {
            lock (positionLock)
            {
                Position += amount;
            }
        }
        public virtual void ShiftBy(Point newPosition)
        {
            lock (positionLock)
            {
                Position = newPosition;
            }
        }
        public virtual void Move()
        {
            lock (positionLock)
            {
                if (Destination != null)
                {
                    if (ParentConsole.rogueMap.GetCell(Destination.X, Destination.Y).IsWalkable)
                    {
                        Path = ParentConsole.rogueMap.calcPath(position, Destination);
                        isMoving = true;
                    }
                }
            }
        }
        public virtual void MoveTo(Point newDestination)
        {
            lock (destinationLock)
            {
                Destination = newDestination;
            }
            Move();
        }
        public virtual void SetDestination(Point newDestination)
        {
            lock (positionLock)
            {
                Destination = newDestination;
            }
        }
        public virtual void MoveTowardsTarget()
        {
            lock (pathingLock)
            {
                if ((Path != null) && isMoving)
                {
                    if (Path.CurrentStep != Path.End)
                    {
                        RogueSharp.Cell targetCell = Path.CurrentStep;
                        ShiftBy(new Point(targetCell.X, targetCell.Y));
                        if (isFocus)
                        {
                            ParentConsole.CenterViewOn(Position);
                        }
                        Path.StepForward();
                    }
                    else
                    {
                        RogueSharp.Cell targetCell = Path.CurrentStep;
                        ShiftBy(new Point(targetCell.X, targetCell.Y));
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
}
