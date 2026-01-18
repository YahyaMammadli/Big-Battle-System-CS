using System;

namespace GameCharacters
{
    public class Warrior : CharacterWithShield
    {
        public int Strength { get; private set; }
        public int Armor { get; private set; }
        public override ConsoleColor Color => ConsoleColor.Yellow;

        public Warrior(string name, int level)
            : base(name, level, 120 + level * 10)
        {
            Strength = 10 + level * 2;
            Armor = 5 + level;
        }

        protected override int GetStartingHealth() => 120 + Level * 10;

        public override int GetAttack() => Strength * 2 + Level * 3;

        public override int GetDefense() => Armor * 2;

        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(1, damage - Armor);
            base.TakeDamage(reducedDamage);
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.Write($"  Strength: {Strength} | Armor: {Armor}\n");
        }
    }
}