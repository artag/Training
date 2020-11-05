namespace Facade
{
    // Некоторые фасады могут работать с разными подсистемами одновременно.
    public class Subsystem2
    {
        public string Init()
        {
            return "Subsystem2: Get ready!\n";
        }

        public string DoWork2()
        {
            return "Subsystem2: Fire!\n";
        }
    }
}
