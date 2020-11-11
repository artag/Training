namespace GuessTheNumberGame.Bootstrap
{
    /// <summary>
    /// Строитель.
    /// </summary>
    public interface IBuilder<out T>
    {
        /// <summary>
        /// Создает объект.
        /// </summary>
        /// <returns>Объект.</returns>
        T Build();
    }
}
