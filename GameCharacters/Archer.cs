using System;

namespace GameCharacters
{
    public class Archer : CharacterWithShield
    {
        public int Agility { get; private set; }
        public int Arrows { get; private set; }
        private const int MAX_ARROWS = 5;
        public override ConsoleColor Color => ConsoleColor.DarkGray;

        public Archer(string name, int level)
            : base(name, level, 100 + level * 8)
        {
            Agility = 15 + level * 3;
            Arrows = MAX_ARROWS;
        }

        protected override int GetStartingHealth() => 100 + Level * 8;

        public override int GetAttack()
        {
            if (Arrows > 0)
            {
                Arrows--;
                return Level * 3 + Agility;
            }
            return Level;
        }

        public override int GetDefense() => Agility * 2;

        public override void TakeDamage(int damage)
        {
            if (random.Next(100) < Agility)
            {
                return;
            }
            base.TakeDamage(damage);
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.Write($"  Agility: {Agility} | Arrows: {Arrows}\n");
        }

        public override void Reset()
        {
            base.Reset();
            Arrows = MAX_ARROWS;
        }
    }
}