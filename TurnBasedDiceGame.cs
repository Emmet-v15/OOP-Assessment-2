//#define TESTING


/// <summary>
/// Represents an abstract base class for a turn-based dice game.
/// </summary>
public abstract class TurnBasedDiceGame
{
    /// <summary>
    /// Represents a player in the dice game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Gets or sets the score of the player.
        /// </summary>
        public int score;

        /// <summary>
        /// Initializes a new instance of the Player class with a name.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        public Player(string name)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// Represents a die that can be rolled to get a random value.
    /// </summary>
    public class Die
    {
        /// <summary>
        /// Gets the value of the die after rolling.
        /// </summary>
        public int Value { get; private set; }

        private static readonly Random random = new Random();

        /// <summary>
        /// Rolls the die to get a new random value.
        /// </summary>
        public void Roll()
        {
            Value = random.Next(1, 7);
        }
    }

    protected List<Die> dice;
    protected Player[] players = new Player[2];

    /// <summary>
    /// Initializes a new instance of the TurnBasedDiceGame class with player names.
    /// </summary>
    /// <param name="playerNames">An array of player names.</param>
    public TurnBasedDiceGame(string[] playerNames)
    {
        dice = new List<Die>();
        players[0] = new Player(playerNames[0]);
        players[1] = new Player(playerNames[1]);
    }

    /// <summary>
    /// Plays the dice game and returns the index of the winning player.
    /// </summary>
    /// <returns>The index of the winning player.</returns>
    public abstract int PlayGame();
}

/// <summary>
/// Represents the Sevens Out dice game.
/// </summary>
public class SevensOut : TurnBasedDiceGame
{
    /// <summary>
    /// Initializes a new instance of the SevensOut class with player names.
    /// </summary>
    /// <param name="playerNames">An array of player names.</param>
    public SevensOut(string[] playerNames) : base(playerNames)
    {
        dice = Enumerable.Range(0, 4).Select(_ => new Die()).ToList();
    }

    /// <summary>
    /// Plays the Sevens Out game and returns the index of the winning player.
    /// </summary>
    /// <returns>The index of the winning player.</returns>
    public override int PlayGame()
    {

        bool[] playerOut = new bool[2]; // Array to keep track of whether a player is out.

        while (players[0].score < 100 && players[1].score < 100 && (!playerOut[0] || !playerOut[1]))
        {
            for (int i = 0; i < 2; i++)
            {
                if (playerOut[i]) continue; // Skip the turn for the player who is out.

                dice[0 + i * 2].Roll();
                dice[1 + i * 2].Roll();

                int sum = dice[0 + i * 2].Value + dice[1 + i * 2].Value;

                if (sum == 7)
                {
#if !TESTING
                    Console.WriteLine($"{players[i].name} rolled a sum of 7 and is out of the game.");
#endif
                    playerOut[i] = true; // Mark the player as out.
                    continue; // Move to the next player's turn.
                }

                bool isDouble = dice[0 + i * 2].Value == dice[1 + i * 2].Value;
                int points = isDouble ? sum * 2 : sum;
#if !TESTING
                Console.WriteLine($"{players[i].name} rolled {dice[0 + i * 2].Value} and {dice[1 + i * 2].Value} for a total of {points}{(isDouble ? " *DOUBLE!*" : "")}.");
#endif
                players[i].score += points;
            }
        }

        // which player wins
        return players[0].score > players[1].score ? 0 : 1;
    }

}

/// <summary>
/// Represents the Three Or More dice game.
/// </summary>
public class ThreeOrMore : TurnBasedDiceGame
{
    /// <summary>
    /// Initializes a new instance of the ThreeOrMore class with player names.
    /// </summary>
    /// <param name="playerNames">An array of player names.</param>
    public ThreeOrMore(string[] playerNames) : base(playerNames)
    {
        dice = Enumerable.Range(0, 5).Select(_ => new Die()).ToList();
    }

    /// <summary>
    /// Plays the Three Or More game and returns the index of the winning player.
    /// </summary>
    /// <returns>The index of the winning player.</returns>
    public override int PlayGame()
    {
        while (players[0].score < 20 && players[1].score < 20)
        {
            for (int i = 0; i < 2; i++)
            {
                dice.ForEach(die => die.Roll());
                List<IGrouping<int, Die>> groups = dice.GroupBy(die => die.Value).ToList();
                int points = groups.Max(group => group.Count()) switch
                {
                    3 => 3,
                    4 => 6,
                    5 => 12,
                    _ => 0
                };
#if !TESTING
                Console.WriteLine($"{players[i].name} rolled {string.Join(", ", dice.Select(die => die.Value))} for a  of {points}. They are now on {players[i].score} points.");
#endif
                players[i].score += points;
            }
        }
#if !TESTING
        Console.WriteLine($"{players[0].name} scored {players[0].score} and {players[1].name} scored {players[1].score}.");
#endif
        return players[0].score > players[1].score ? 0 : 1;
    }
}