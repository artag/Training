namespace Adapter
{
    // Адаптер делает интерфейс Адаптируемого класса совместимым с целевым
    // интерфейсом.
    internal class Adapter : ITarget
    {
        private readonly ISource _source;

        public Adapter(ISource source)
        {
            _source = source;
        }

        public string GetRequest()
        {
            var resultSource = _source.GetRequestSource();
            return $"This is '{resultSource}'";
        }
    }
}
