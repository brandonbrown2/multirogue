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
    public class GameMapConsole : SadConsole.Consoles.Console
    {
        public GameMap rogueMap;
        private List<Character> entities;
        protected bool viewMoved = true;

        public PlayerCharacterLocal Player {
            get
            {
                if (entities.Count > 0) return (PlayerCharacterLocal)entities[0];
                return null;
            }
        }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight)
            : this(viewWidth, viewHeight, mapWidth, mapHeight, new System.Random())
        {

        }

        public GameMapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight, System.Random r)
            : base(mapWidth, mapHeight)
        {
            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);


            entities = new List<Character>();

            EntityGenerator.setGameConsole(this);

            rogueMap = new GameMap(mapHeight, mapWidth, this, r);
        }

        public void AddEntity (Character character)
        {
            entities.Add(character);
        }

        public override void Render()
        {
            base.Render();
            rogueMap.Render();
            foreach (Character c in entities)
            {
                if (viewMoved)
                {
                    c.SetRenderOffset(Position - textSurface.RenderArea.Location);
                }
                c.Render();
            }
        }

        public override void Update()
        {
            base.Update();
            foreach (Character c in entities)
            {
                c.Update();
            }
        }


        public override bool ProcessMouse(MouseInfo info)
        {
            if (info.LeftClicked && entities.Count > 0)
            {
                base.ProcessMouse(info);
                if (info.Console == this)
                {
                    entities[0].SetDestination(info.ConsoleLocation);
                    entities[0].isMoving = true;
                    entities[0].Move();
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
