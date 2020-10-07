namespace Decorator
{
    // Конкретные Компоненты предоставляют реализации поведения по умолчанию.
    // Может быть несколько вариаций этих классов.
    internal class ConcreteComponent : Component
    {
        public override string Operation()
        {
            return "ConcreteComponent";
        }
    }
}
