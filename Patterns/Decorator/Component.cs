namespace Decorator
{
    // Базовый абстрактный класс Компонента определяет поведение, которое изменяется
    // декораторами.
    // Компонент также может использовать интерфейс.
    public abstract class Component
    {
        public abstract string Operation();
    }
}
