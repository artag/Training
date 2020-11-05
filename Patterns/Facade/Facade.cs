namespace Facade
{
    // Класс Фасада предоставляет простой интерфейс для сложной логики одной или
    // нескольких подсистем. Фасад делегирует запросы клиентов соответствующим
    // объектам внутри подсистемы. Фасад также отвечает за управление их
    // жизненным циклом. Все это защищает клиента от нежелательной сложности
    // подсистемы.
    public class Facade
    {
        private readonly Subsystem1 _subsystem1;

        private readonly Subsystem2 _subsystem2;

        public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
        {
            _subsystem1 = subsystem1;
            _subsystem2 = subsystem2;
        }

        // Методы Фасада удобны для быстрого доступа к сложной функциональности
        // подсистем. Однако клиенты получают только часть возможностей
        // подсистемы.
        public string Operation()
        {
            var result = "Facade initializes subsystems:\n";
            result += _subsystem1.Init();
            result += _subsystem2.Init();

            result += "Facade orders subsystems to perform the action:\n";

            result += _subsystem1.DoWork1();
            result += _subsystem2.DoWork2();
            return result;
        }
    }
}
