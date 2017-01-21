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
            characterReference.Shift(position);
        }

        public Point GetPosition()
        {
            lock (characterReference.positionLock)
            {
                return characterReference.Position;
            }
        }

        public void SetTarget(Point destination)
        {
            characterReference.MoveTo(destination);
        }

        public Point GetTarget()
        {
            lock (characterReference.destinationLock)
            {
                return characterReference.Destination;
            }
        }

        public void setCharacterReference(Object character)
        {
            characterReference = (Character)character;
        }
    }
}
