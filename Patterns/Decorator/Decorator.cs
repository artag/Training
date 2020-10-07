namespace Decorator
{
    // Базовый класс Декоратора следует тому же интерфейсу, что и другие
    // компоненты. Основная цель этого класса - определить интерфейс обёртки для
    // всех конкретных декораторов.
    public abstract class Decorator : Component
    {
        private readonly Component _component;

        protected Decorator(Component component)
        {
            _component = component;
        }

        // Декоратор делегирует всю работу обёрнутому компоненту.
        public override string Operation()
        {
            return _component != null
                ? _component.Operation()
                : string.Empty;
        }
    }
}
