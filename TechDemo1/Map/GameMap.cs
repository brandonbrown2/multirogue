using RogueSharp;
using SadConsole;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;
using TechDemo1.MapObjects;
using SadConsole.Shapes;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Collections.Generic;

namespace TechDemo1.Map
{
    public class GameMap : Console
    {
        public RogueSharp.Map collisionMap;
        public Console ParentConsole;
        protected RogueSharp.PathFinder pathing;
        protected LayeredTextSurface mapData;
        protected SurfaceEditor editor;
        protected System.Random rand;

        public GameMap(int mapWidth, int mapHeight, Console outputRef) : this(mapWidth, mapHeight, outputRef, new System.Random())
        {
        }

        public GameMap(int mapWidth, int mapHeight, Console outputRef, System.Random r) : base(mapWidth, mapHeight)
        {
            rand = r;
            ParentConsole = outputRef;

            collisionMap = new RogueSharp.Map(mapWidth, mapHeight);
            collisionMap.Clear(true, true);

            mapData = new LayeredTextSurface(mapWidth, mapHeight, 2);
            mapData.RenderArea = ParentConsole.TextSurface.RenderArea;
            TextSurface = mapData;
            editor = new SurfaceEditor(mapData);

            Renderer = new LayeredTextRenderer();

            FillGround<Grass>();
            for (int i = 0; i < 1000; i++)
            {
                DrawRandom<Tree>(1, false);
            }
                drawRooms(50);

            pathing = new RogueSharp.PathFinder(collisionMap);
        }

        public override void Render()
        {
            mapData.RenderArea = ParentConsole.TextSurface.RenderArea;
            base.Render();
        }

        public bool IsWalkable(int x, int y)
        {
            return collisionMap.IsWalkable(x, y);
        }

        public Path calcPath(Microsoft.Xna.Framework.Point start, Microsoft.Xna.Framework.Point end)
        {
            Path path;
            if (((start.X == end.X) && (start.Y == end.Y)) || !collisionMap.IsWalkable(end.X, end.Y))
            {
                return null;
            }
            try
            {
                path = pathing.ShortestPath(collisionMap.GetCell(start.X, start.Y), collisionMap.GetCell(end.X, end.Y));
            }
            catch (System.Exception e)
            {
                path = null;
            }
            return path;
        }

        public void FillGround<T>() where T : IDeterministicCellAppearance, new()
        {
            mapData.SetActiveLayer(0);
            for (int y = 0; y < textSurface.Height; y++)
            {
                for (int x = 0; x < textSurface.Width; x++)
                {
                    T temp = new T();
                    temp.Randomize(rand);
                    editor.SetCellAppearance(x, y, temp);
                }
            }
        }
        public void DrawRandom<T>(int layer, bool walkable) where T : IDeterministicCellAppearance, new()
        {
            int x = rand.Next(Width), y = rand.Next(Height);
            collisionMap.SetCellProperties(x, y, false, walkable);
            mapData.SetActiveLayer(layer);

            T temp = new T();
            temp.Randomize(rand);
            editor.SetCellAppearance(x, y, temp);
        }
        private void drawRooms(int roomCount)
        {
            mapData.SetActiveLayer(1);
            var rooms = new List<Rectangle>();
            int _roomMinSize = 10, _roomMaxSize = 15;
            
            for (int r = 0; r < roomCount; r++)
            {
                int roomWidth = rand.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = rand.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = rand.Next(0, Width - roomWidth - 1);
                int roomYPosition = rand.Next(0, Height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = false;
                foreach (Rectangle room in rooms)
                {
                    if (newRoom.Intersects(room))
                    {
                        newRoomIntersects = true;
                        break;
                    }
                }
                if (!newRoomIntersects)
                {
                    rooms.Add(newRoom);
                }
            }

            foreach (Rectangle room in rooms)
            {
                MakeRoom(room);
            }

            for (int r = 0; r < rooms.Count; r++)
            {
                if (r == 0)
                {
                    continue;
                }

                int previousRoomCenterX = rooms[r - 1].Center.X;
                int previousRoomCenterY = rooms[r - 1].Center.Y;
                int currentRoomCenterX = rooms[r].Center.X;
                int currentRoomCenterY = rooms[r].Center.Y;
                /*
                if (_random.Next(0, 2) == 0)
                {
                    MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    MakeVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    MakeHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
                */
            }
        }
        private void MakeRoom(Rectangle room)
        {
            mapData.SetActiveLayer(0);
            setSquare(room, new Tile(), true);
            mapData.SetActiveLayer(1);
            setSquare(room, new Wall(), false);
            room.Inflate(-1, -1);
            setSquare(room, new EmptyCell(), true);
            int side = rand.Next(2);
            if (side == 0)
            {
                int x = room.Center.X;
                editor.SetCellAppearance(x, room.Top - 1, new Door());
                collisionMap.SetCellProperties(x, room.Top - 1, true, true);
                editor.SetCellAppearance(x, room.Bottom, new Door());
                collisionMap.SetCellProperties(x, room.Bottom, true, true);
            } else
            {
                int y = room.Center.Y;
                editor.SetCellAppearance(room.Left - 1, y, new Door());
                collisionMap.SetCellProperties(room.Left - 1, y, true, true);
                editor.SetCellAppearance(room.Right, y, new Door());
                collisionMap.SetCellProperties(room.Right, y, true, true);
            }
        }
        private void setSquare(Rectangle rect, CellAppearance cell, bool walkable)
        {
            Microsoft.Xna.Framework.Point p = rect.Location;
            while (rect.Contains(p) || (p.Y < rect.Bottom))
            {
                while (rect.Contains(p))
                {
                    editor.SetCellAppearance(p.X, p.Y, cell);
                    collisionMap.SetCellProperties(p.X, p.Y, walkable, walkable);
                    p.X++;
                }
                p.X = rect.Location.X;
                p.Y++;
            }
        }
    }
}
