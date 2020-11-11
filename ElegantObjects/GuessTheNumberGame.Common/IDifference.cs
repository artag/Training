namespace GuessTheNumberGame.Common
{
    /// <summary>
    /// Разница.
    /// </summary>
    public interface IDifference<out T>
    {
        T Difference { get; }
    }
}
