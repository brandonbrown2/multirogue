using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp.Random;
using TechDemo1.Map;
using TechDemo1.MapTypes;
using TechDemo1.Entities;
using TechDemo1.Entities.Wrappers;
using Lidgren.Network;

namespace GameServer
{
    class Simulation
    {
        private int mapSizeX = 150;
        private int mapSizeY = 150;
        private int mapSeedValue;
        private GameMap map;
        private IDictionary<int, CharacterInstanceWrapper> PlayerDictionary;

        public Simulation()
        {
            var randomgen = new RogueSharp.Random.DotNetRandom();
            mapSeedValue = randomgen.Next(Int32.MaxValue - 1);

            RogueSharp.MapCreation.IMapCreationStrategy<GameMap> mapCreationStrategy
                = new TechDemo1.MapTypes.SimpleMapCreationStrategy<GameMap>(mapSizeX, mapSizeY, 100, 30, 10, new RogueSharp.Random.DotNetRandom(mapSeedValue));
            map = mapCreationStrategy.CreateMap();

            PlayerDictionary = new Dictionary<int, CharacterInstanceWrapper>();
        }

        public int getMapSeed()
        {
            return mapSeedValue;
        }

        public Point spawnNewPlayer(int newPlayer)
        {
            PlayerDictionary.Add(newPlayer, EntityGenerator.GenerateRemoteCharacter());
            PlayerDictionary[newPlayer].SetPosition(FindSpawnLocation());
            return PlayerDictionary[newPlayer].GetPosition();
        }

        public void GenerateEntityList(NetOutgoingMessage response)
        {
        }

        public Point FindSpawnLocation()
        {
            RogueSharp.Random.IRandom random = new RogueSharp.Random.DotNetRandom();
            var Position = new Point();
            while (true)
            {
                int x = random.Next(mapSizeX - 1);
                int y = random.Next(mapSizeY - 1);
                if (map.IsWalkable(x, y))
                {
                    Position = new Point(x, y);
                    break;
                }
            }
            return Position;
        }
    }
}
