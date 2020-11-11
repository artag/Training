using GuessTheNumberGame.Impl;

namespace GuessTheNumberGame
{
    class Program
    {
        static void Main(string[] args)
        {
            new GameFactory().Build().Run();
        }
    }
}
