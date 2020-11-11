using System;
using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Вывод (печать) на консоль.
    /// </summary>
    internal class ConsolePrinter : IPrinter<string>
    {
        /// <inheritdoc />
        public void Print(string media)
        {
            Console.WriteLine(media);
        }
    }
}
