using GuessTheNumberGame.Bootstrap;
using GuessTheNumberGame.Common;

namespace GuessTheNumberGame.Impl
{
    /// <summary>
    /// Фабрика для инициализации экземпляра <see cref="IGame"/>.
    /// </summary>
    public class GameFactory : IBuilder<IGame>
    {
        /// <inheritdoc />
        public IGame Build()
        {
            var consoleInput = new ConsoleInput();
            var consolePrinter = new ConsolePrinter();

            var secret = new Secret();
            var guess = new Guess(consolePrinter, consoleInput);
            var difference = new DefaultDifference(secret, guess);
            var verboseDifference = new VerboseDifference(difference, consolePrinter);
            var attempts = new Attempts(verboseDifference, numberOfAttempts: 5, consolePrinter);
            var game = new Game(attempts, secret, consolePrinter);

            return game;
        }
    }
}
