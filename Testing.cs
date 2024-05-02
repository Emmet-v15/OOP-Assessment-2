using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A class that performs unit testing on the Die and Game classes.
/// </summary>
internal class Testing
{
    /// <summary>
    /// Initializes a new instance of the Testing class and runs the tests.
    /// </summary>
    public Testing()
    {
        Console.WriteLine("Testing Die class...");
        TestDieClass();
        Console.WriteLine("Testing Game Class...");
        TestGameClass();
        Console.WriteLine("Testing succeeded.");
    }

    /// <summary>
    /// Tests the Die class by rolling a million dice and checking their values and distribution.
    /// </summary>
    private void TestDieClass()
    {
        int rollCount = 1_000_000;
        int[] rolls = new int[rollCount];

        // Create and roll a million dices.
        rolls = Enumerable.Range(0, rollCount).Select(_ => { Die die = new Die(); die.Roll(); return die.Value; }).ToArray();

        Debug.Assert(rolls.Where((_, i) => i % 2 == 0).Sum() + rolls.Where((_, i) => i % 2 == 1).Sum() != 7 * rollCount, "Die cannot sum to 7.");

        // Check if any of them are out of bounds.
        Debug.Assert(rolls.Max() <= 6, "Die rolled a value above 6.");
        Debug.Assert(rolls.Min() >= 0, "Die rolled a negative value.");

        // Check if the numbers are evenly distributed.
        Debug.Assert(IsEvenlyDistributed(rolls), "Die is not fair.");
    }

    /// <summary>
    /// Tests the Game class by creating if it correctly exits.
    /// </summary>
    private void TestGameClass()
    {
        // Create and test many sevens out games.
        for (int i = 0; i < 100; i++)
        {
            SevensOut game = new SevensOut(new string[] { "Player 1", "Player 2" });
            int result = game.PlayGame();
            Debug.Assert(result == 0 || result == 1, $"Game did not return a valid result, returned: {result}");
        }

        // Create and test many three or more games.
        for (int i = 0; i < 100; i++)
        {
            ThreeOrMore game = new ThreeOrMore(new string[] { "Player 1", "Player 2" });
            int result = game.PlayGame();
            Debug.Assert(result == 0 || result == 1, $"Game did not return a valid result, returned: {result}");
        }
    }

    /// <summary>
    /// Checks if an array of integers is evenly distributed using the mean and standard deviation.
    /// </summary>
    /// <param name="array">The array of integers to check.</param>
    /// <returns>A boolean value indicating whether the array is evenly distributed or not.</returns>
    private bool IsEvenlyDistributed(int[] array)
    {
        double epsilon = 0.1; // margin of error.
        double mean = array.Average();
        double std = Math.Sqrt(array.Select(num => Math.Pow(num - mean, 2)).Average()); // How spread out the values are.
        bool is_even = true; // Assume even.
        if (Math.Abs(mean - 3.5) > epsilon || Math.Abs(std - 1.7078) > epsilon) is_even = false; // compare to our margin of error.
        return is_even;
    }
}
