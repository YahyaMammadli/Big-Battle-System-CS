using GameCharacters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSystem
{
    public class Team
    {
        public string Name { get; }
        public ConsoleColor TeamColor { get; }
        public List<Character> Characters { get; }
        public bool IsDefeated => Characters.All(c => !c.IsAlive);

        public Team(string name, ConsoleColor color)
        {
            Name = name;
            TeamColor = color;
            Characters = new List<Character>();
        }

        public void AddCharacter(Character character) => Characters.Add(character);

        public void DisplayTeamStatus()
        {
            Console.ForegroundColor = TeamColor;
            Console.Write($"\n════════════════════════════════════\n");
            Console.Write($"           {Name} TEAM\n");
            Console.Write($"════════════════════════════════════\n\n");
            Console.ResetColor();

            for (int i = 0; i < Characters.Count; i++)
            {
                var character = Characters[i];
                Console.Write($"{i + 1}. ");
                character.DisplayStatus();
                Console.Write("\n");
            }
            Console.ResetColor();
        }

        public List<Character> GetAliveCharacters()
        {
            return Characters.Where(c => c.IsAlive).ToList();
        }

        public Character? GetAliveCharacter(int index)
        {
            var alive = GetAliveCharacters();
            return index >= 0 && index < alive.Count ? alive[index] : null;
        }

        public bool HasAliveCharacters()
        {
            return Characters.Any(c => c.IsAlive);
        }
    }
}