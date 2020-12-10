namespace GuessTheNumberGame.Common
{
    /// <summary>
    /// Несколько попыток для сравнения чисел.
    /// </summary>
    public interface IAttempts
    {
        /// <summary>
        /// Сравнивает числа.
        /// </summary>
        /// <returns>true - числа равны, false - в противном случае.</returns>
        bool Matches();
    }
}
