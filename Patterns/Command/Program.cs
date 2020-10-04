namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            var simpleCommand = new SimpleCommand("Say Hi!");

            var receiver = new Receiver();
            var complexCommand = new ComplexCommand(receiver, "Send email", "Save report");

            // Клиентский код может параметризовать объект
            // вызывающего класса любыми командами.
            var invoker = new Invoker(simpleCommand, complexCommand);
            invoker.Invoke();
        }
    }
}
