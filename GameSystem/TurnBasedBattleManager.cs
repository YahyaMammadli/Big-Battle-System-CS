using GameCharacters;
using System;
using System.Linq;

namespace GameSystem
{
    public class TurnBasedBattleManager
    {
        private Team teamRed;
        private Team teamBlue;
        private Random random = new Random();
        private bool isPlayer1Turn = true;

        public TurnBasedBattleManager(Team redTeam, Team blueTeam)
        {
            teamRed = redTeam;
            teamBlue = blueTeam;
        }

        public void StartBattle()
        {
            Console.Clear();
            Console.Write("════════════════════════════════════\n");
            Console.Write("                  TEAM BATTLE\n");
            Console.Write("════════════════════════════════════\n\n");

            DisplayTeamCompositions();

            while (!teamRed.IsDefeated && !teamBlue.IsDefeated)
            {
                Team currentTeam = isPlayer1Turn ? teamRed : teamBlue;
                Team opponentTeam = isPlayer1Turn ? teamBlue : teamRed;
                string playerName = isPlayer1Turn ? "Player 1 (Red)" : "Player 2 (Blue)";

                Character selectedCharacter = SelectCharacterFromTeam(currentTeam, playerName);
                if (selectedCharacter == null)
                {
                    isPlayer1Turn = !isPlayer1Turn;
                    continue;
                }

                Console.Clear();
                Console.ForegroundColor = currentTeam.TeamColor;
                Console.Write($"════════════════════════════════════\n");
                Console.Write($"          {playerName}'s TURN\n");
                Console.Write($"════════════════════════════════════\n\n");
                Console.ResetColor();

                Console.Write($"Selected character: ");
                selectedCharacter.DisplayInfo();
                Console.Write("\n\n");

                if (!selectedCharacter.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"\nERROR: {selectedCharacter.Name} is already dead and cannot perform actions!\n");
                    Console.ResetColor();
                    Console.Write("\nPress any key to continue...\n");
                    Console.ReadKey();
                    isPlayer1Turn = !isPlayer1Turn;
                    continue;
                }

                Console.Write($"\nWhat will {selectedCharacter.Name} do?\n");
                Console.Write("1. Attack\n");
                Console.Write("2. Use Shield (10% damage reduction for next attack)\n");
                Console.Write("3. Skip Turn\n");
                Console.Write("\nChoose action (1-3) => ");

                string actionChoice = Console.ReadLine();

                Console.Clear();

                switch (actionChoice)
                {
                    case "1":
                        Console.ForegroundColor = currentTeam.TeamColor;
                        Console.Write($"════════════════════════════════════\n");
                        Console.Write($"          ATTACK PHASE\n");
                        Console.Write($"════════════════════════════════════\n\n");
                        Console.ResetColor();
                        Console.Write($"Attacker: {selectedCharacter.Name}\n\n");
                        PerformAttack(selectedCharacter, currentTeam, opponentTeam);
                        break;

                    case "2":
                        if (selectedCharacter is IShieldable shieldable)
                        {
                            shieldable.ActivateShield();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($"════════════════════════════════════\n");
                            Console.Write($"          SHIELD ACTIVATED\n");
                            Console.Write($"════════════════════════════════════\n\n");
                            Console.Write($"{selectedCharacter.Name} raises a shield! Next attack will be reduced by 10%\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nThis character cannot use a shield!\n");
                            Console.ResetColor();
                        }
                        break;

                    case "3":
                        Console.Write($"════════════════════════════════════\n");
                        Console.Write($"          TURN SKIPPED\n");
                        Console.Write($"════════════════════════════════════\n\n");
                        Console.Write($"{selectedCharacter.Name} skips this turn.\n");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid choice! Turn skipped.\n");
                        Console.ResetColor();
                        break;
                }

                if (teamRed.IsDefeated || teamBlue.IsDefeated)
                {
                    break;
                }

                Console.Write("\nPress any key to continue to next turn...\n");
                Console.ReadKey();

                isPlayer1Turn = !isPlayer1Turn;
            }

            DisplayBattleResults();
        }

        private Character SelectCharacterFromTeam(Team team, string playerName)
        {
            Console.Clear();
            Console.ForegroundColor = team.TeamColor;
            Console.Write($"════════════════════════════════════\n");
            Console.Write($"      SELECT YOUR CHARACTER\n");
            Console.Write($"════════════════════════════════════\n\n");
            Console.ResetColor();

            if (!team.HasAliveCharacters())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\nAll characters in {playerName}'s team are dead!\n");
                Console.ResetColor();
                return null;
            }

            Console.Write($"{playerName}, select a character from your team:\n\n");

            for (int i = 0; i < team.Characters.Count; i++)
            {
                var character = team.Characters[i];
                Console.Write($"{i + 1}. ");
                character.DisplayInfo();
                Console.Write("\n");
            }

            Console.Write($"\nChoose character (1-{team.Characters.Count}) => ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= team.Characters.Count)
            {
                Character selectedCharacter = team.Characters[choice - 1];

                if (!selectedCharacter.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"\nERROR: {selectedCharacter.Name} is already dead! Please select a living character.\n");
                    Console.ResetColor();
                    Console.Write("\nPress any key to select again...\n");
                    Console.ReadKey();
                    return SelectCharacterFromTeam(team, playerName);
                }

                return selectedCharacter;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nInvalid selection! Random living character selected.\n");
            Console.ResetColor();

            var aliveCharacters = team.GetAliveCharacters();
            if (aliveCharacters.Count > 0)
            {
                return aliveCharacters[random.Next(aliveCharacters.Count)];
            }

            return null;
        }

        private void PerformAttack(Character attacker, Team attackerTeam, Team defenderTeam)
        {
            if (!defenderTeam.HasAliveCharacters())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nNo enemies left to attack!\n");
                Console.ResetColor();
                return;
            }

            Character target = SelectAttackTarget(defenderTeam, attacker.Name);
            if (target == null) return;

            if (!target.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\nERROR: {target.Name} is already dead! Please select a living target.\n");
                Console.ResetColor();
                Console.Write("\nPress any key to select another target...\n");
                Console.ReadKey();
                PerformAttack(attacker, attackerTeam, defenderTeam);
                return;
            }

            ExecuteAttack(attacker, target);
        }

        private Character SelectAttackTarget(Team defenderTeam, string attackerName)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"════════════════════════════════════\n");
            Console.Write($"          SELECT TARGET\n");
            Console.Write($"════════════════════════════════════\n\n");
            Console.ResetColor();

            Console.Write($"{attackerName}, select target to attack:\n\n");

            for (int i = 0; i < defenderTeam.Characters.Count; i++)
            {
                var character = defenderTeam.Characters[i];
                Console.Write($"{i + 1}. ");
                character.DisplayFullInfo();
                Console.Write("\n");
            }

            Console.Write($"\nChoose target (1-{defenderTeam.Characters.Count}) => ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= defenderTeam.Characters.Count)
            {
                return defenderTeam.Characters[choice - 1];
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nInvalid selection! Random living target selected.\n");
            Console.ResetColor();

            var aliveDefenders = defenderTeam.GetAliveCharacters();
            if (aliveDefenders.Count > 0)
            {
                return aliveDefenders[random.Next(aliveDefenders.Count)];
            }

            return null;
        }

        private void ExecuteAttack(Character attacker, Character target)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"════════════════════════════════════\n");
            Console.Write($"          ATTACK RESULT\n");
            Console.Write($"════════════════════════════════════\n\n");
            Console.ResetColor();

            int attackDamage = attacker.GetAttack();
            int defense = target.GetDefense();
            int finalDamage = Math.Max(1, attackDamage - defense);

            Console.ForegroundColor = attacker.Color;
            Console.Write($"{attacker.Name} attacks {target.Name}!\n\n");
            Console.ResetColor();

            Console.Write($"Attack damage: {attackDamage}\n");
            Console.Write($"Target defense: {defense}\n");
            Console.Write($"Base damage: {finalDamage}\n");

            if (target is IShieldable shieldable && shieldable.HasShield)
            {
                int reduction = shieldable.ShieldReduction;
                int reducedDamage = finalDamage * (100 - reduction) / 100;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n🛡️ {target.Name}'s shield reduces damage by {reduction}%! ");
                Console.Write($"(Damage reduced from {finalDamage} to {reducedDamage})\n");
                Console.ResetColor();
                finalDamage = reducedDamage;
            }

            target.TakeDamage(finalDamage);
            Console.Write($"\nFinal damage dealt: {finalDamage}\n");
            Console.Write($"{target.Name} ");

            if (!target.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"has been defeated! 💀\n");
                Console.ResetColor();
            }
            else
            {
                Console.Write($"remaining HP: {target.Health}\n");
            }
        }

        private void DisplayBothTeams()
        {
            teamRed.DisplayTeamStatus();
            teamBlue.DisplayTeamStatus();
        }

        private void DisplayTeamCompositions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Red Team:\n");
            Console.ResetColor();
            foreach (var character in teamRed.Characters)
            {
                Console.Write("\n");
                character.DisplayInfo();
            }

            Console.Write("\n\n");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Blue Team:\n");
            Console.ResetColor();
            foreach (var character in teamBlue.Characters)
            {
                Console.Write("\n");
                character.DisplayInfo();
            }

            Console.Write("\nPress any key to start the battle...\n");
            Console.ReadKey();
        }

        private void DisplayBattleResults()
        {
            Console.Clear();
            Console.Write("════════════════════════════════════\n");
            Console.Write("         BATTLE RESULTS\n");
            Console.Write("════════════════════════════════════\n\n");

            Team winner = teamRed.IsDefeated ? teamBlue : teamRed;
            Team loser = teamRed.IsDefeated ? teamRed : teamBlue;

            Console.ForegroundColor = winner.TeamColor;
            Console.Write($" {winner.Name} TEAM WINS! \n\n");
            Console.ResetColor();

            Console.Write("Survivors:\n");
            foreach (var character in winner.Characters.Where(c => c.IsAlive))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"✓ {character.Name} (HP: {character.Health})\n");
                Console.ResetColor();
            }

            Console.Write("\nDefeated:\n");
            foreach (var character in loser.Characters)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"✗ {character.Name}\n");
                Console.ResetColor();
            }

            var loserSurvivors = loser.Characters.Where(c => c.IsAlive).ToList();
            if (loserSurvivors.Count > 0)
            {
                Console.Write("\nSurvivors in losing team:\n");
                foreach (var character in loserSurvivors)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"✓ {character.Name} (HP: {character.Health})\n");
                    Console.ResetColor();
                }
            }

            Console.Write("\n════════════════════════════════════\n");
            Console.Write("\nPress any key to return to menu...\n");
            Console.ReadKey();
        }
    }
}