using System;

namespace GameCharacters
{
    public class Mage : CharacterWithShield
    {
        public int Mana { get; private set; }
        private const int MANA_PER_ATTACK = 30;
        private const int MANA_REGEN = 20;
        public override ConsoleColor Color => ConsoleColor.Cyan;

        public Mage(string name, int level)
            : base(name, level, 90 + level * 5)
        {
            Mana = 100 + level * 10;
        }

        protected override int GetStartingHealth() => 90 + Level * 5;

        public override int GetAttack()
        {
            if (Mana >= MANA_PER_ATTACK)
            {
                Mana -= MANA_PER_ATTACK;
                return Level * 5 + Mana / 2;
            }

            Mana = Math.Min(100 + Level * 10, Mana + MANA_REGEN);
            return Level * 2;
        }

        public override int GetDefense() => Mana / 15;

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.Write($"  Mana: {Mana}\n");
        }

        public override void Reset()
        {
            base.Reset();
            Mana = 100 + Level * 10;
        }
    }
}