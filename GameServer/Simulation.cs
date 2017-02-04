using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1.Map;
using TechDemo1.MapTypes;
using TechDemo1.Entities;
using TechDemo1.Entities.Wrappers;
using Lidgren.Network;

namespace GameServer
{
    class Simulation
    {
        private class MockCharacter
        {
            public Point currentLoc;
            public Point targetLoc;
        }

        private int mapSizeX = 150;
        private int mapSizeY = 150;
        private int mapSeedValue;
        private GameMap map;
        private IDictionary<int, MockCharacter> PlayerDictionary;

        public Simulation()
        {
            var randomgen = new Random();
            mapSeedValue = randomgen.Next(Int32.MaxValue - 1);
            map = new GameMap(mapSizeX, mapSizeY, null, new Random(mapSeedValue));
            PlayerDictionary = new Dictionary<int, MockCharacter>();
        }

        public int getMapSeed()
        {
            return mapSeedValue;
        }

        public Point spawnNewPlayer(int newPlayer)
        {
            PlayerDictionary.Add(newPlayer, new MockCharacter());
            PlayerDictionary[newPlayer].currentLoc = FindSpawnLocation();
            return PlayerDictionary[newPlayer].currentLoc;
        }

        public void GenerateEntityList(NetOutgoingMessage response)
        {
            foreach(KeyValuePair<int, MockCharacter> CharacterLookup in PlayerDictionary)
            {
                response.Write("Player");
                response.Write(CharacterLookup.Key);
                response.Write(CharacterLookup.Value.currentLoc.X);
                response.Write(CharacterLookup.Value.currentLoc.Y);
                response.Write(CharacterLookup.Value.targetLoc.X);
                response.Write(CharacterLookup.Value.targetLoc.Y);
            }
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
