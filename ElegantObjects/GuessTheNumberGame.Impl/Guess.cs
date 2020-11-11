using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Предполагаемое число.
    /// </summary>
    internal class Guess : INumber<int>
    {
        private readonly IPrinter<string> _printer;
        private readonly IInput<string> _input;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Guess"/>.
        /// </summary>
        /// <param name="printer">Принтер.</param>
        /// <param name="input">Ввод информации.</param>
        public Guess(IPrinter<string> printer, IInput<string> input)
        {
            _printer = printer;
            _input = input;
        }

        /// <inheritdoc />
        public int Number
        {
            get
            {
                _printer.Print("Угадай число в диапазоне от 0 по 99: ");
                var num = _input.Input();

                return int.Parse(num);
            }
        }
    }
}
