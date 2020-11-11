using System;
using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Ввод информации через консоль.
    /// </summary>
    internal class ConsoleInput : IInput<string>
    {
        /// <inheritdoc />
        public string Input()
        {
            return Console.ReadLine();
        }
    }
}
