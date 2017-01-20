using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1.Entities.Wrappers
{
    public class CharacterInstanceWrapper
    {
        private Character characterReference;
        public int EntityID;

        public void SetPosition(Point position)
        {
            characterReference.Position = position;
        }

        public Point GetPosition()
        {
            return characterReference.Position;
        }

        public void SetTarget(Point destination)
        {
            characterReference.Destination = destination;
        }

        public Point GetTarget()
        {
            return characterReference.Destination;
        }
    }
}
