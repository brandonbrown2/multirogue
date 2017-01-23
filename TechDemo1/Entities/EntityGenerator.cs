using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDemo1.Entities.Wrappers;
using Microsoft.Xna.Framework;

namespace TechDemo1.Entities
{
    public class EntityGenerator
    {
        private static GameMapConsole gameReference;
        public static CharacterInstanceWrapper GenerateCharacter()
        {
            CharacterInstanceWrapper characterRef = new CharacterInstanceWrapper();
            Character newCharacter = new Character(gameReference);
            gameReference.AddEntity(newCharacter);
            characterRef.setCharacterReference(newCharacter);
            return characterRef;
        }
        public static CharacterInstanceWrapper GenerateRemoteCharacter()
        {
            CharacterInstanceWrapper characterRef = new CharacterInstanceWrapper();
            Character newCharacter = new PlayerCharacterRemote(gameReference);
            gameReference.AddEntity(newCharacter);
            characterRef.setCharacterReference(newCharacter);
            return characterRef;
        }
        public static CharacterInstanceWrapper GenerateLocalCharacter(int entityID, Point location)
        {
            CharacterInstanceWrapper characterRef = new CharacterInstanceWrapper();
            Character newCharacter = new PlayerCharacterLocal(gameReference);
            gameReference.AddEntity(newCharacter);
            characterRef.setCharacterReference(newCharacter);
            characterRef.SetPosition(location);
            characterRef.EntityID = entityID;
            return characterRef;
        }

        public static void setGameConsole(GameMapConsole console)
        {
            gameReference = console;
        }
    }
}
