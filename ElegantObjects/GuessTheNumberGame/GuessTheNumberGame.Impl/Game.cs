using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Игра "Угадай число".
    /// </summary>
    internal class Game : IGame
    {
        private readonly IAttempts _attempts;
        private readonly INumber<int> _secret;
        private readonly IPrinter<string> _printer;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Game"/>.
        /// </summary>
        /// <param name="attempts">Несколько попыток для сравнения чисел.</param>
        /// <param name="secret">Загаданное число.</param>
        /// <param name="printer">Принтер.</param>
        public Game(IAttempts attempts, INumber<int> secret, IPrinter<string> printer)
        {
            _attempts = attempts;
            _secret = secret;
            _printer = printer;
        }

        /// <inheritdoc />
        public void Run()
        {
            if (!_attempts.Matches())
            {
                _printer.Print($"Вы проиграли. Было загадано число {_secret.Number}");
            }
            else
            {
                _printer.Print($"Bingo! Вы угадали загаданное число!");
            }

            _printer.Print("Спасибо за игру!");
        }
    }
}
