namespace Decorator
{
    // Декораторы могут выполнять своё поведение до или после вызова обёрнутого
    // объекта.
    internal class ConcreteDecoratorB : Decorator
    {
        public ConcreteDecoratorB(Component comp) : base(comp)
        {
        }

        public override string Operation()
        {
            return $"ConcreteDecoratorB({base.Operation()})";
        }
    }
}
