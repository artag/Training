using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Несколько попыток для сравнения чисел.
    /// Сравнение происходит до тех пор,
    /// пока числа не будут равны или
    /// пока не будет достигнуто максимальное число попыток.
    /// </summary>
    internal class Attempts : IAttempts
    {
        private readonly IDifference<int> _diff;
        private readonly int _numberOfAttempts;
        private readonly IPrinter<string> _printer;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Attempts"/>.
        /// </summary>
        /// <param name="diff">Разница между целыми числами.</param>
        /// <param name="numberOfAttempts">Максимальное число попыток.</param>
        /// <param name="printer">Принтер.</param>
        public Attempts(IDifference<int> diff, int numberOfAttempts, IPrinter<string> printer)
        {
            _diff = diff;
            _numberOfAttempts = numberOfAttempts;
            _printer = printer;
        }

        /// <inheritdoc />
        public bool Matches()
        {
            var t = 1;
            while (true)
            {
                if (t > _numberOfAttempts)
                    break;

                _printer.Print($"Попытка #{t} из {_numberOfAttempts}");

                if (_diff.Difference == 0)
                    break;

                ++t;
            }

            return t <= _numberOfAttempts;
        }
    }
}
