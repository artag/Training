using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Разница между загаданным и предполагаемым числом.
    /// Печатает информацию о разнице через <see cref="IPrinter{T}"/>.
    /// </summary>
    internal class VerboseDifference : IDifference<int>
    {
        private readonly IDifference<int> _diff;
        private readonly IPrinter<string> _printer;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="VerboseDifference"/>.
        /// </summary>
        /// <param name="diff">Разница между загаданным и предполагаемым числом.</param>
        /// <param name="printer">Принтер информации о разнице.</param>
        public VerboseDifference(IDifference<int> diff, IPrinter<string> printer)
        {
            _diff = diff;
            _printer = printer;
        }

        public int Difference
        {
            get
            {
                var number = _diff.Difference;
                if (number < 0)
                {
                    _printer.Print("Загаданное число больше");
                }
                else if (number > 0)
                {
                    _printer.Print("Загаданное число меньше");
                }
                else
                {
                    _printer.Print("Угадано!");
                }

                return number;
            }
        }
    }
}
