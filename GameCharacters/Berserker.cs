using System;

namespace GameCharacters
{
    public class Berserker : CharacterWithShield
    {
        public int Strength { get; private set; }
        public int Rage { get; private set; }
        public override ConsoleColor Color => ConsoleColor.Red;

        public Berserker(string name, int level)
            : base(name, level, 110 + level * 8)
        {
            Strength = 12 + level * 2;
            Rage = 0;
        }

        protected override int GetStartingHealth() => 110 + Level * 8;

        public override int GetAttack()
        {
            int baseDamage = Strength * 2 + Level * 2;
            Rage = Math.Min(100, Rage + 10);

            if (Health < GetStartingHealth() * 0.5)
            {
                return baseDamage * 2 + Rage / 2;
            }
            return baseDamage + Rage / 4;
        }

        public override int GetDefense() => 5 + Level;

        public override void TakeDamage(int damage)
        {
            int finalDamage = damage;
            if (Health < GetStartingHealth() * 0.3)
            {
                finalDamage = (int)(damage * 0.7);
            }
            base.TakeDamage(finalDamage);
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.Write($"  Rage: {Rage}");
            if (Health < GetStartingHealth() * 0.5)
                Console.Write(" | ENRAGED!");
            Console.Write("\n");
        }

        public override void Reset()
        {
            base.Reset();
            Rage = 0;
        }
    }
}