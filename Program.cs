//#define TESTING

public class Program
{

    public static void Main(string[] args)
    {
#if TESTING
        Testing _ = new();
#else
        Statistics stats = new("statistics.json");

        bool exit = false;
        while (!exit)
        {
            string gameChoice = GetUserInput("Which game would you like to play? (default SevensOut)\n1. SevensOut\n2. ThreeOrMore", "1", "2", "1");
            string playerChoice = GetUserInput("Would you like to play against another person or the computer? (default computer)\n1. Another person\n2. Computer", "1", "2", "2");
            string player1 = GetUserInput("Player 1, enter your name (default \"Player 1\"):", null, null, "Player 1");
            string player2 = playerChoice == "1" ? GetUserInput("Player 2, enter your name (default \"Player 2\"):", null, null, "Player 2") : "computer";
            int result;
            if (gameChoice == "1")
            {
                SevensOut game = new SevensOut(new string[] { player1, player2 });
                result = game.PlayGame();
                Console.WriteLine(GetResultMessage(result, player1, player2));
            }
            else
            {
                ThreeOrMore game = new ThreeOrMore(new string[] { player1, player2 });
                result = game.PlayGame();
                Console.WriteLine(GetResultMessage(result, player1, player2));
            }

            // display statistics
            stats.SaveGameResults(gameChoice == "1" ? "SevensOut" : "ThreeOrMore", result);
            stats.PrintGameResults();

            exit = GetUserInput("Would you like to play again? (default no)\n1. Yes\n2. No", "1", "2", "2") == "2";
        }
#endif
    }

    private static string GetUserInput(string prompt, string? option1, string? option2, string defaultValue)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return defaultValue;
        }
        else if (input == option1 || input == option2)
        {
            return input;
        }
        else
        {
            return defaultValue;
        }
    }

    private static string GetResultMessage(int result, string player1, string player2)
    {
        return result switch
        {
            0 => $"{player1} won!",
            1 => $"{player2} won!",
            _ => throw new NotImplementedException()
        };
    }
}
