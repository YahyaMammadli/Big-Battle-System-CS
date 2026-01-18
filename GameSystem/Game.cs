using GameCharacters;
using System;
using System.Media;
using System.Threading;

namespace GameSystem
{
    public class Game
    {
        private CharacterFactory factory = new CharacterFactory();
        private const int DEFAULT_LEVEL = 5;
        private const int TEAM_SIZE = 3;

        private SoundPlayer menuMusicPlayer;
        private SoundPlayer battleMusicPlayer;
        private SoundPlayer currentMusicPlayer;
        private Thread musicThread;
        private bool isMusicEnabled = true;

        private readonly string menuMusicPath = @"C:\Users\Root\Desktop\it\C#\Game System Final Version\Game\Music\MenuMusic.wav";
        private readonly string battleMusicPath = @"C:\Users\Root\Desktop\it\C#\Game System Final Version\Game\Music\BattleMusic.wav";

        public Game()
        {
            InitializeMusic();
        }

        private void InitializeMusic()
        {
            try
            {
                Console.WriteLine("Loading music...");

                if (!System.IO.File.Exists(menuMusicPath))
                {
                    Console.WriteLine($"Warning: Menu music file not found at {menuMusicPath}");
                    isMusicEnabled = false;
                    return;
                }

                if (!System.IO.File.Exists(battleMusicPath))
                {
                    Console.WriteLine($"Warning: Battle music file not found at {battleMusicPath}");
                    isMusicEnabled = false;
                    return;
                }

                menuMusicPlayer = new SoundPlayer(menuMusicPath);
                battleMusicPlayer = new SoundPlayer(battleMusicPath);

                
                menuMusicPlayer.LoadAsync();
                battleMusicPlayer.LoadAsync();

                Console.WriteLine("Music loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading music: {ex.Message}");
                isMusicEnabled = false;
            }
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();

                
                PlayMenuMusic();

                ShowMainMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartQuickBattle();
                        break;
                    case "2":
                        ShowRules();
                        break;
                    case "3":
                        StartCustomBattle();
                        break;
                    case "4":
                        Console.Write("\nThanks for playing! Goodbye!\n");
                        StopMusic();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nInvalid choice! Press any key to continue...\n");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void PlayMenuMusic()
        {
            if (!isMusicEnabled || menuMusicPlayer == null) return;

            try
            {
                StopMusic();
                currentMusicPlayer = menuMusicPlayer;

                if (menuMusicPlayer.IsLoadCompleted)
                {
                    musicThread = new Thread(() =>
                    {
                        try
                        {
                            currentMusicPlayer.PlayLooping();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error playing menu music: {ex.Message}");
                        }
                    });

                    musicThread.IsBackground = true;
                    musicThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting menu music: {ex.Message}");
            }
        }

        private void PlayBattleMusic()
        {
            if (!isMusicEnabled || battleMusicPlayer == null) return;

            try
            {
                StopMusic();
                currentMusicPlayer = battleMusicPlayer;

                if (battleMusicPlayer.IsLoadCompleted)
                {
                    musicThread = new Thread(() =>
                    {
                        try
                        {
                            currentMusicPlayer.PlayLooping();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error playing battle music: {ex.Message}");
                        }
                    });

                    musicThread.IsBackground = true;
                    musicThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting battle music: {ex.Message}");
            }
        }

        private void StopMusic()
        {
            try
            {
                if (currentMusicPlayer != null)
                {
                    currentMusicPlayer.Stop();
                    currentMusicPlayer = null;
                }

                if (musicThread != null && musicThread.IsAlive)
                {
                    musicThread.Join(100); 
                    musicThread = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping music: {ex.Message}");
            }
        }

        private void PauseMusic()
        {
            try
            {
                if (currentMusicPlayer != null)
                {
                    currentMusicPlayer.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pausing music: {ex.Message}");
            }
        }

        private void ResumeMusic()
        {
            try
            {
                if (currentMusicPlayer != null && currentMusicPlayer.IsLoadCompleted)
                {
                    currentMusicPlayer.PlayLooping();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resuming music: {ex.Message}");
            }
        }

        private void ShowMainMenu()
        {
            Console.Write("════════════════════════════════════\n");
            Console.Write("                  TEAM BATTLE\n");
            Console.Write("════════════════════════════════════\n\n");
            Console.Write("1. Quick Battle\n");
            Console.Write("2. How to Play\n");
            Console.Write("3. Custom Battle\n");
            Console.Write("4. Exit\n");
            Console.Write("\nSelect option => ");
        }

        private void ShowRules()
        {
            PauseMusic();

            Console.Clear();
            Console.Write("════════════════════════════════════\n");
            Console.Write("          HOW TO PLAY\n");
            Console.Write("════════════════════════════════════\n\n");
            Console.Write("Team Battle Rules:\n");
            Console.Write("─────────────────────────────\n");
            Console.Write("• Two teams of 3 characters each\n");
            Console.Write("• Players take turns controlling their team\n");
            Console.Write("• On your turn:\n");
            Console.Write("  1. Choose one of your alive characters\n");
            Console.Write("  2. Choose action: Attack, Shield, or Skip\n");
            Console.Write("  3. If Attack: choose target enemy\n");
            Console.Write("  4. If Shield: character gets 10% damage\n");
            Console.Write("     reduction for next attack against them\n");
            Console.Write("• Shield lasts until character is attacked\n");
            Console.Write("• Team wins when all enemies are defeated\n\n");
            Console.Write("Important Notes:\n");
            Console.Write("────────────────\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("• You cannot select dead characters for actions\n");
            Console.Write("• You cannot attack dead enemies\n");
            Console.ResetColor();
            Console.Write("• Dead characters are marked with [DEAD]\n");
            Console.Write("• Living characters are marked with [ALIVE]\n");
            Console.Write("• Characters with active shields show 🛡️ icon\n\n");
            Console.Write("Character Types:\n");
            Console.Write("────────────────\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Warrior: ");
            Console.ResetColor();
            Console.Write("High armor, reduces incoming damage\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Mage: ");
            Console.ResetColor();
            Console.Write("Uses mana, strong attacks\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Archer: ");
            Console.ResetColor();
            Console.Write("Can dodge, limited arrows\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Berserker: ");
            Console.ResetColor();
            Console.Write("Stronger when low health\n");
            Console.Write("\nPress any key to return to menu...\n");
            Console.ReadKey();

            ResumeMusic();
        }

        private void StartQuickBattle()
        {
            try
            {
                Console.Clear();

                PlayBattleMusic();

                Console.Write("════════════════════════════════════\n");
                Console.Write("          QUICK BATTLE\n");
                Console.Write("════════════════════════════════════\n\n");

                Team redTeam = new Team("Red", ConsoleColor.Red);
                Team blueTeam = new Team("Blue", ConsoleColor.Blue);

                var redChars = factory.CreateRandomTeam(TEAM_SIZE, DEFAULT_LEVEL);
                var blueChars = factory.CreateRandomTeam(TEAM_SIZE, DEFAULT_LEVEL);

                foreach (var character in redChars)
                    redTeam.AddCharacter(character);
                foreach (var character in blueChars)
                    blueTeam.AddCharacter(character);

                Console.Write("Random teams generated!\n");
                Console.Write("\nPress any key to start...\n");
                Console.ReadKey();

                TurnBasedBattleManager battle = new TurnBasedBattleManager(redTeam, blueTeam);
                battle.StartBattle();
            }
            finally
            {
                
                PlayMenuMusic();
            }
        }

        private void StartCustomBattle()
        {
            try
            {
                Console.Clear();

                
                PlayBattleMusic();

                Console.Write("════════════════════════════════════\n");
                Console.Write("          CUSTOM BATTLE\n");
                Console.Write("════════════════════════════════════\n\n");

                Team redTeam = CreateTeam("Red", ConsoleColor.Red);
                Team blueTeam = CreateTeam("Blue", ConsoleColor.Blue);

                TurnBasedBattleManager battle = new TurnBasedBattleManager(redTeam, blueTeam);
                battle.StartBattle();
            }
            finally
            {
               
                PlayMenuMusic();
            }
        }

        private Team CreateTeam(string teamName, ConsoleColor color)
        {
            Team team = new Team(teamName, color);

            for (int i = 1; i <= TEAM_SIZE; i++)
            {
                Console.Clear();
                Console.Write($"════════════════════════════════════\n");
                Console.Write($"  Creating Character {i} for {teamName} Team\n");
                Console.Write($"════════════════════════════════════\n\n");

                Console.Write("Choose character type:\n\n");
                Console.Write("1. Warrior\n");
                Console.Write("2. Mage\n");
                Console.Write("3. Archer\n");
                Console.Write("4. Berserker\n");
                Console.Write("\nSelect type (1-4) => ");

                string typeChoice = Console.ReadLine();

                Console.Write("\nEnter character name => ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    name = $"{teamName} Hero {i}";

                Character character = typeChoice switch
                {
                    "1" => factory.CreateCharacter("Warrior", name, DEFAULT_LEVEL),
                    "2" => factory.CreateCharacter("Mage", name, DEFAULT_LEVEL),
                    "3" => factory.CreateCharacter("Archer", name, DEFAULT_LEVEL),
                    "4" => factory.CreateCharacter("Berserker", name, DEFAULT_LEVEL),
                    _ => factory.CreateCharacter("Warrior", name, DEFAULT_LEVEL)
                };

                team.AddCharacter(character);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\n{name} has been added to {teamName} Team!\n");
                Console.ResetColor();

                if (i < TEAM_SIZE)
                {
                    Console.Write("\nPress any key to create next character...\n");
                    Console.ReadKey();
                }
            }

            return team;
        }
    }
}