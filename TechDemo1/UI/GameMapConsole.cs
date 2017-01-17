using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SadConsole.Game;
using SadConsole.Consoles;
using SadConsole.Input;
using System.Threading;
using TechDemo1.Map;
using RogueSharp;
using RogueSharp.Random;
using TechDemo1.Entities;

namespace TechDemo1
{
    class GameMapConsole : SadConsole.Consoles.Console
    {
        public GameMap rogueMap;
        private PlayerCharacterLocal playerEntity;
        protected int currentTicks, maxTicks;
        protected bool viewMoved = true;

        public PlayerCharacterLocal Player { get { return playerEntity; } }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight)
            : this(viewWidth, viewHeight, mapWidth, mapHeight, new RogueSharp.Random.DotNetRandom())
        {

        }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight, IRandom r)
            : base(mapWidth, mapHeight)
        {
            currentTicks = 0;
            maxTicks = 5;

            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);

            AnimatedTextSurface playerAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            playerAnimation.CreateFrame();
            playerAnimation.CurrentFrame[0].Foreground = Color.Orange;
            playerAnimation.CurrentFrame[0].GlyphIndex = '@';

            AnimatedTextSurface targetAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            targetAnimation.CreateFrame();
            targetAnimation.CurrentFrame[0].Foreground = Color.Red;
            targetAnimation.CurrentFrame[0].GlyphIndex = 'X';

            playerEntity = new PlayerCharacterLocal(this);
            playerEntity.Font = Engine.DefaultFont;
            playerEntity.Animation = playerAnimation;
            playerEntity.Position = new Point(1, 1);

            playerEntity.target = new GameObject(Engine.DefaultFont);
            playerEntity.target.Animation = targetAnimation;
            playerEntity.target.Position = new Point(1, 1);

            RogueSharp.MapCreation.IMapCreationStrategy<GameMap> mapCreationStrategy
                = new MapTypes.SimpleMapCreationStrategy<GameMap>(mapWidth, mapHeight, 100, 30, 10, r);

            rogueMap = mapCreationStrategy.CreateMap();
            rogueMap.CopyApearanceTo(this);
            PlacePlayer();
        }

        private void PlacePlayer()
        {
            playerEntity.Teleport();
            CenterViewOn(playerEntity.Position);
        }
        public void MovePlayer()
        {
            playerEntity.Path = rogueMap.calcPath(playerEntity.Position, playerEntity.target.Position);
            playerEntity.isMoving = true;
        }
        public override void Render()
        {
            base.Render();

            // If he view area moved, we'll keep our entity in sync with it.
            if (viewMoved)
            {
                playerEntity.RenderOffset = Position - TextSurface.RenderArea.Location;
                playerEntity.target.RenderOffset = Position - TextSurface.RenderArea.Location;
            }
            playerEntity.target.Render();
            playerEntity.Render();
        }

        public override void Update()
        {
            base.Update();
            playerEntity.Update();
            playerEntity.target.Update();
            currentTicks++;
            if ((currentTicks >= maxTicks) && (playerEntity.isMoving))
            {
                currentTicks = 0;
                playerEntity.MoveTowardsTarget();
            }
        }


        public override bool ProcessMouse(MouseInfo info)
        {
            if (info.LeftClicked)
            {
                base.ProcessMouse(info);
                if (info.Console == this)
                {
                    playerEntity.MoveTargetTo(info.ConsoleLocation);
                    playerEntity.isMoving = true;
                    MovePlayer();
                }
            }
            return false;
        }

        public void CenterViewOn (Point newPosition)
        {
            // Scroll the view area to center the player on the screen
            TextSurface.RenderArea = new Rectangle(newPosition.X - (TextSurface.RenderArea.Width / 2),
                                                   newPosition.Y - (TextSurface.RenderArea.Height / 2),
                                                   TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);
        }
        public bool isWithinMap(Point newPosition)
        {
            return (new Rectangle(0, 0, Width, Height)).Contains(newPosition);
        }
        public bool isWithinScreen(Point newPosition)
        {
            return TextSurface.RenderArea.Contains(newPosition);
        }
        public bool isWithinMapAndScreen(Point newPosition)
        {
            return (isWithinMap(newPosition) && isWithinScreen(newPosition));
        }
    }
}
