namespace GuessTheNumberGame.Common
{
    /// <summary>
    /// Ввод информации в игру.
    /// </summary>
    public interface IInput<out T>
    {
        /// <summary>
        /// Вводит информацию в игру.
        /// </summary>
        /// <returns>Введенная информация.</returns>
        T Input();
    }
}
