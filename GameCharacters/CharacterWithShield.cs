using System;

namespace GameCharacters
{
    public abstract class CharacterWithShield : Character, IShieldable
    {
        public bool HasShield { get; protected set; }
        public int ShieldReduction => 10;

        protected CharacterWithShield(string name, int level, int startingHealth)
            : base(name, level, startingHealth)
        {
            HasShield = false;
        }

        public void ActivateShield() => HasShield = true;
        public void DeactivateShield() => HasShield = false;

        public override void Reset()
        {
            base.Reset();
            HasShield = false;
        }

        public override void DisplayFullInfo()
        {
            base.DisplayStatus();
            if (HasShield)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" 🛡️ (Shield: {ShieldReduction}% reduction)");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }
}