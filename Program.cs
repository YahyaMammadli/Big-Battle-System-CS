using GameSystem;

namespace BattleRoyale
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Team Battle Arena";
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Game game = new Game();
                game.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}