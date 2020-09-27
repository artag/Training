namespace Adapter
{
    // Целевой класс объявляет интерфейс, с которым может работать клиентский код.
    public interface ITarget
    {
        string GetRequest();
    }
}
