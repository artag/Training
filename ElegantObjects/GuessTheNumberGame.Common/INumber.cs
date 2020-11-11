namespace GuessTheNumberGame.Common
{
    /// <summary>
    /// Число.
    /// </summary>
    public interface INumber<out T>
    {
        /// <summary>
        /// Число.
        /// </summary>
        T Number { get; }
    }
}
