using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Разница между загаданным и предполагаемым числом.
    /// </summary>
    internal class DefaultDifference : IDifference<int>
    {
        private readonly INumber<int> _secret;
        private readonly INumber<int> _guess;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="DefaultDifference"/>.
        /// </summary>
        /// <param name="secret">Загаданное число.</param>
        /// <param name="guess">Предполагаемое число.</param>
        public DefaultDifference(INumber<int> secret, INumber<int> guess)
        {
            _secret = secret;
            _guess = guess;
        }

        /// <inheritdoc />
        public int Difference =>
            _guess.Number - _secret.Number;
    }
}
