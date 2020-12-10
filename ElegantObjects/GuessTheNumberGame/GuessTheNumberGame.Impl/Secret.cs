using System;
using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Секретное, загаданное целочисленное число.
    /// </summary>
    internal class Secret : INumber<int>
    {
        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Secret"/>.
        /// Создает произвольное число от 0 до 99.
        /// </summary>
        public Secret()
            : this(new Random().Next(minValue: 0, maxValue: 100))
        {
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Secret"/>.
        /// </summary>
        /// <param name="num">Загаданное число.</param>
        public Secret(int num)
        {
            Number = num;
        }

        /// <inheritdoc />
        public int Number { get; }
    }
}
