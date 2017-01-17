﻿using RogueSharp;
using SadConsole;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;

namespace TechDemo1.Map
{
    class GameMap : RogueSharp.Map
    {
        public SadConsole.CellAppearance[,] mapData;
        protected RogueSharp.PathFinder pathing;

        public GameMap() : base()
        {
        }
        public GameMap(int mapHeight, int mapWidth, Console outputRef) : base(mapHeight, mapWidth)
        {

        }

        public void InitPathing()
        {
            pathing = new RogueSharp.PathFinder(this);
        }
        public void InitMapData()
        {            
            // Create the local cache of map data
            mapData = new CellAppearance[Width, Height];

            // Loop through the map information generated by RogueSharp and create our cached visuals of that data
            foreach (var cell in GetAllCells())
            {
                if (cell.IsWalkable)
                {
                    mapData[cell.X, cell.Y] = new MapObjects.Floor();
                }
                else
                {
                    mapData[cell.X, cell.Y] = new MapObjects.Wall();
                }
            }
        }

        public void CopyApearanceTo(Console target)
        {
            if (mapData == null)
            {
                InitMapData();
            }
            foreach (var cell in GetAllCells())
            {
                mapData[cell.X, cell.Y].CopyAppearanceTo(target[cell.X, cell.Y]);
            }
        }

        public Path calcPath(Microsoft.Xna.Framework.Point start, Microsoft.Xna.Framework.Point end)
        {
            if (pathing == null)
            {
                InitPathing();
            }
            Path path;
            if ((start.X == end.X) && (start.Y == end.Y))
            {
                return null;
            }
            try
            {
                path = pathing.ShortestPath(this.GetCell(start.X, start.Y), this.GetCell(end.X, end.Y));
            }
            catch (System.Exception e)
            {
                path = null;
            }
            return path;
        }

    }
}
