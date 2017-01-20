using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp.Random;
using TechDemo1.Map;
using TechDemo1.MapTypes;

namespace GameServer
{
    class Simulation
    {
        private int mapSeedValue;
        private GameMap map;

        public Simulation()
        {
            var randomgen = new RogueSharp.Random.DotNetRandom();
            mapSeedValue = randomgen.Next(Int32.MaxValue - 1);

            RogueSharp.MapCreation.IMapCreationStrategy<GameMap> mapCreationStrategy
                = new TechDemo1.MapTypes.SimpleMapCreationStrategy<GameMap>(150, 150, 100, 30, 10, new RogueSharp.Random.DotNetRandom(mapSeedValue));
            map = mapCreationStrategy.CreateMap();
        }

        public int getMapSeed()
        {
            return mapSeedValue;
        }
    }
}
