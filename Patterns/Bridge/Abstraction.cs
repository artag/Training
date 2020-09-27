namespace Bridge
{
    // Абстракция устанавливает интерфейс для «управляющей» части двух иерархий
    // классов. Она содержит ссылку на объект из иерархии Реализации и
    // делегирует ему всю настоящую работу.
    internal class Abstraction : IAbstraction
    {
        public Abstraction(IImplementation implementation)
        {
            Implementation = implementation;
        }

        protected IImplementation Implementation { get; }

        public virtual string Operation()
        {
            return "Abstraction: Base operation with:\n" +
                   Implementation.OperationImplementation();
        }
    }
}
