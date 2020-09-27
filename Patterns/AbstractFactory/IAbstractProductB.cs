namespace AbstractFactory
{
    public interface IAbstractProductB
    {
        // Продукт B способен работать самостоятельно...
        string MethodB();

        // ...а также взаимодействовать с Продуктами А той же вариации.
        //
        // Абстрактная Фабрика гарантирует, что все продукты, которые она
        // создает, имеют одинаковую вариацию и, следовательно, совместимы.
        string AnotherMethodB(IAbstractProductA collaborator);
    }
}
