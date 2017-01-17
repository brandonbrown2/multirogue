using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using Microsoft.Xna.Framework;
using SadConsole.Game;
using SadConsole.Consoles;
using SadConsole.Input;
using System.Threading;
using TechDemo1.Map;
//using RogueSharp;

namespace TechDemo1
{
    class GameMapConsole : SadConsole.Consoles.Console
    {
        GameMap rogueMap;
        GameObject playerEntity;
        private GameObject target;
        RogueSharp.Path path;
        protected int currentTicks, maxTicks;
        private bool _isMoving;
        public bool isMoving
        {
            get { return _isMoving; }
            set { _isMoving = value;
                if (rogueMap == null)
                {
                    return;
                }
                if (value == true)
                {
                    calcPath();
                }
            }
        }


        public GameObject Player { get { return playerEntity; } }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight) : base(mapWidth, mapHeight)
        {
            currentTicks = 0;
            maxTicks = 5;
            isMoving = false;

            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);

            AnimatedTextSurface playerAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            playerAnimation.CreateFrame();
            playerAnimation.CurrentFrame[0].Foreground = Color.Orange;
            playerAnimation.CurrentFrame[0].GlyphIndex = '@';

            AnimatedTextSurface targetAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            targetAnimation.CreateFrame();
            targetAnimation.CurrentFrame[0].Foreground = Color.Red;
            targetAnimation.CurrentFrame[0].GlyphIndex = 'X';

            playerEntity = new GameObject(Engine.DefaultFont);
            playerEntity.Animation = playerAnimation;
            playerEntity.Position = new Point(1, 1);

            target = new GameObject(Engine.DefaultFont);
            target.Animation = targetAnimation;
            target.Position = new Point(1, 1);

            rogueMap = new GameMap(mapHeight, mapWidth, this);
            PlacePlayer();
        }

        private void PlacePlayer()
        {
            RogueSharp.Random.IRandom random = new RogueSharp.Random.DotNetRandom();
            while (true)
            {
                int x = random.Next(Width - 1);
                int y = random.Next(Height - 1);
                if (rogueMap.IsWalkable(x, y))
                {
                    playerEntity.Position = new Point(x, y);

                    // Center the veiw area
                    TextSurface.RenderArea = new Rectangle(playerEntity.Position.X - (TextSurface.RenderArea.Width / 2),
                                                           playerEntity.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                           TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

                    playerEntity.RenderOffset = this.Position - TextSurface.RenderArea.Location;

                    break;
                }
            }
            target.Position = playerEntity.Position;
            MovePlayerBy(Point.Zero);
        }

        public override void Render()
        {
            base.Render();
            target.Render();
            playerEntity.Render();
        }

        public override void Update()
        {
            base.Update();
            playerEntity.Update();
            target.Update();
            currentTicks++;
            if ((currentTicks >= maxTicks) && (isMoving))
            {
                currentTicks = 0;
                MoveTowardsTarget();
            }
        }

        private void calcPath()
        {
            if ((playerEntity.Position.X == this.target.Position.X) && (playerEntity.Position.Y == this.target.Position.Y))
            {
                isMoving = false;
                return;
            }
            try
            {
                //path = pathing.ShortestPath(rogueMap.GetCell(playerEntity.Position.X, playerEntity.Position.Y), rogueMap.GetCell(target.Position.X, target.Position.Y));
            } catch (Exception e)
            {
                isMoving = false;
            }
        }

        public override bool ProcessMouse(MouseInfo info)
        {
            if (info.LeftClicked)
            {
                base.ProcessMouse(info);
                if (info.Console == this)
                {
                    MoveTargetTo(info.ConsoleLocation);
                    isMoving = true;
                }
            }
            return false;
        }

        public void MoveTowardsTarget()
        {
            //if (path.CurrentStep != path.End)
            {
                RogueSharp.Cell targetCell = path.CurrentStep;
                MovePlayerBy(new Point(targetCell.X, targetCell.Y) - playerEntity.Position);
                path.StepForward();
            } else
            {
                RogueSharp.Cell targetCell = path.CurrentStep;
                MovePlayerBy(new Point(targetCell.X, targetCell.Y) - playerEntity.Position);
                isMoving = false;
            }
        }

        public void MovePlayerBy(Point amount)
        {
            // Get the position the player will be at
            Point newPosition = playerEntity.Position + amount;
            // Check to see if the position is within the map
            if (new Rectangle(0, 0, Width, Height).Contains(newPosition)
                && rogueMap.IsWalkable(newPosition.X, newPosition.Y))
            {
                // Move the player
                playerEntity.Position += amount;

                // Scroll the view area to center the player on the screen
                TextSurface.RenderArea = new Rectangle(playerEntity.Position.X - (TextSurface.RenderArea.Width / 2),
                                                       playerEntity.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                       TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

                // If he view area moved, we'll keep our entity in sync with it.
                playerEntity.RenderOffset = this.Position - TextSurface.RenderArea.Location;
                target.RenderOffset = this.Position - TextSurface.RenderArea.Location;
            }
        }
        public void MoveTargetBy(Point amount)
        {
            Point newPosition = target.Position + amount;
            // Check to see if the position is within the map
            if (TextSurface.RenderArea.Contains(newPosition))
                if ((new Rectangle(0, 0, Width, Height).Contains(newPosition))
                        && (TextSurface.RenderArea.Contains(newPosition)))
                {
                    // Move the target
                    target.Position += amount;
                }
        }
        public void MoveTargetTo(Point newPosition)
        {
            // Check to see if the position is within the map
            if (TextSurface.RenderArea.Contains(newPosition))
                if ((new Rectangle(0, 0, Width, Height).Contains(newPosition))
                        && (TextSurface.RenderArea.Contains(newPosition)))
                {
                    // Move the target
                    target.Position = newPosition;
                }
        }
    }
}
