using Newtonsoft.Json;

internal class Statistics
{
    readonly string fileName;

    public Statistics(string fileName)
    {
        if (!File.Exists(fileName))
        {
            File.Create(fileName).Close();
        }
        this.fileName = fileName;
    }

    public void SaveGameResults(string gameName, int result)
    {
        string json = JsonConvert.SerializeObject(new { gameName, result });
        File.AppendAllText(fileName, json + Environment.NewLine);
    }

    public void PrintGameResults()
    {
        // convert the result to statistics overall

        string[] lines = File.ReadAllLines(fileName);
        int totalGames = lines.Length;
        int player1SevensOutWins = 0;
        int player2SevensOutWins = 0;
        int player1ThreeOrMoreWins = 0;
        int player2ThreeOrMoreWins = 0;

        foreach (string line in lines)
        {
            dynamic? game = JsonConvert.DeserializeObject(line);
            if (game == null) continue;
            if (game.result == 0)
            {
                if (game.gameName == "SevensOut")
                    {
                        player1SevensOutWins++;
                    }
                    else
                    {
                        player1ThreeOrMoreWins++;
                    }
                }
            else
            {
                if (game.gameName == "SevensOut")
                    {
                        player2SevensOutWins++;
                    }
                    else
                    {
                        player2ThreeOrMoreWins++;
                    }
                }
        }

        Console.WriteLine("========================== Statistics ==========================");
        Console.WriteLine($"Sevens Out Games: {player1SevensOutWins + player2SevensOutWins}");
        Console.WriteLine($"Three or More Games: {player1ThreeOrMoreWins + player2ThreeOrMoreWins}");

        Console.WriteLine($"Player 1 Sevens Out Wins: {player1SevensOutWins}");
        Console.WriteLine($"Player 2 Sevens Out Wins: {player2SevensOutWins}");

        Console.WriteLine($"Player 1 Three or More Wins: {player1ThreeOrMoreWins}");
        Console.WriteLine($"Player 2 Three or More Wins: {player2ThreeOrMoreWins}");

        Console.WriteLine($"Total games: {totalGames}");
        Console.WriteLine($"{player1SevensOutWins + player1ThreeOrMoreWins} total wins for Player 1");
        Console.WriteLine($"{player2SevensOutWins + player2ThreeOrMoreWins} total wins for Player 2");
        Console.WriteLine(
            $"Player 1 win rate overall: {Math.Round((double)(player1SevensOutWins + player1ThreeOrMoreWins) / totalGames * 100, 2)}%"
        );
        Console.WriteLine(
            $"Player 2 win rate overall: {Math.Round((double)(player2SevensOutWins + player2ThreeOrMoreWins) / totalGames * 100, 2)}%"
        );

        Console.WriteLine("================================================================");
    }
}
