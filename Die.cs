public class Die
{
    public int Value { get; private set; }
    private static readonly Random random = new();

    public void Roll() {
        Value = random.Next(1, 7);
    }
}