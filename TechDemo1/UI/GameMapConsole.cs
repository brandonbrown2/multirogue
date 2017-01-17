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
using TechDemo1.UI;

namespace TechDemo1
{
    class GameMapConsole : SadConsole.Consoles.Console
    {
        public GameMap rogueMap;
        private PlayerCharacterLocal playerEntity;
        private PlayerCharacterRemote remotePlayerEntity;
        protected bool viewMoved = true;

        public PlayerCharacterLocal Player { get { return playerEntity; } }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight)
            : this(viewWidth, viewHeight, mapWidth, mapHeight, new RogueSharp.Random.DotNetRandom())
        {

        }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight, IRandom r)
            : base(mapWidth, mapHeight)
        {
            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);

            playerEntity = new PlayerCharacterLocal(this);
            playerEntity.Font = Engine.DefaultFont;
            playerEntity.Animation = UIConstants.playerAnimation;
            playerEntity.Position = new Point(1, 1);

            playerEntity.target = new GameObject(Engine.DefaultFont);
            playerEntity.target.Animation = UIConstants.targetAnimation;
            playerEntity.target.Position = new Point(1, 1);

            remotePlayerEntity = new PlayerCharacterRemote(this);
            remotePlayerEntity.Font = Engine.DefaultFont;
            remotePlayerEntity.Animation = UIConstants.SecondPlayerAnimation;
            remotePlayerEntity.Position = new Point(1, 1);

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
        public void pathCharacterTo(Character entity, Point target)
        {
            entity.Path = rogueMap.calcPath(entity.Position, target);
            entity.isMoving = true;
        }
        public override void Render()
        {
            base.Render();

            // If he view area moved, we'll keep our entity in sync with it.
            if (viewMoved)
            {
                playerEntity.RenderOffset = Position - TextSurface.RenderArea.Location;
                playerEntity.target.RenderOffset = Position - TextSurface.RenderArea.Location;
                remotePlayerEntity.RenderOffset = Position - TextSurface.RenderArea.Location;
            }
            playerEntity.target.Render();
            playerEntity.Render();
            remotePlayerEntity.Render();
        }

        public override void Update()
        {
            base.Update();
            playerEntity.Update();
            playerEntity.target.Update();
            remotePlayerEntity.Update();
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
                    pathCharacterTo(playerEntity, playerEntity.target.Position);
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
