using GameCharacters;
using System;
using System.Collections.Generic;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace GameSystem
{
    public class CharacterFactory
    {
        private Random random = new Random();

        public Character CreateCharacter(string type, string name, int level)
        {
            return type switch
            {
                "Warrior" => new Warrior(name, level),
                "Mage" => new Mage(name, level),
                "Archer" => new Archer(name, level),
                "Berserker" => new Berserker(name, level),
                _ => new Warrior(name, level)
            };
        }

        public List<Character> CreateRandomTeam(int teamSize, int level)
        {
            var team = new List<Character>();
            var availableTypes = new List<Func<string, int, Character>>
            {
                (name, lvl) => new Warrior(name, lvl),
                (name, lvl) => new Mage(name, lvl),
                (name, lvl) => new Archer(name, lvl),
                (name, lvl) => new Berserker(name, lvl)
            };

            for (int i = 0; i < teamSize; i++)
            {
                Character character;

                if (i < availableTypes.Count)
                {
                    character = availableTypes[i]($"Hero {i + 1}", level);
                }
                else
                {
                    var randomType = availableTypes[random.Next(availableTypes.Count)];
                    character = randomType($"Hero {i + 1}", level);
                }

                team.Add(character);
            }

            return team;
        }
    }
}