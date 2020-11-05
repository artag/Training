namespace Facade
{
    class Program
    {
        // В клиентском коде могут быть уже созданы некоторые объекты
        // подсистемы. В этом случае может оказаться целесообразным
        // инициализировать Фасад с этими объектами вместо того, чтобы
        // позволить Фасаду создавать новые экземпляры.
        static void Main(string[] args)
        {
            var subsystem1 = new Subsystem1();
            var subsystem2 = new Subsystem2();
            var facade = new Facade(subsystem1, subsystem2);
            Client.Run(facade);
        }
    }
}
