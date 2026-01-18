using System;

namespace GameCharacters
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public bool IsAlive => Health > 0;
        public abstract ConsoleColor Color { get; }

        protected Random random = new Random();

        public Character(string name, int level, int startingHealth)
        {
            Name = name;
            Level = level;
            Health = startingHealth;
        }

        public abstract int GetAttack();
        public abstract int GetDefense();

        public virtual void TakeDamage(int damage)
        {
            if (this is IShieldable shieldable && shieldable.HasShield)
            {
                int reducedDamage = damage * (100 - shieldable.ShieldReduction) / 100;
                Health = Math.Max(0, Health - reducedDamage);
                shieldable.DeactivateShield();
            }
            else
            {
                Health = Math.Max(0, Health - damage);
            }
        }

        public virtual void Heal(int amount) => Health += amount;

        public virtual void DisplayStatus()
        {
            if (IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[ALIVE] ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[DEAD]  ");
            }

            Console.ForegroundColor = Color;
            Console.Write($"{Name} (Level {Level})");
            Console.ResetColor();
            Console.Write($" - Health: {Health}");
        }

        public virtual void DisplayInfo()
        {
            DisplayStatus();
            Console.Write("\n");
        }

        public virtual void DisplayFullInfo()
        {
            DisplayStatus();
            if (this is IShieldable shieldable && shieldable.HasShield)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" 🛡️ (Shield: {shieldable.ShieldReduction}%)");
                Console.ResetColor();
            }
            Console.Write("\n");
        }

        public virtual void Reset() => Health = GetStartingHealth();

        protected abstract int GetStartingHealth();
    }
}