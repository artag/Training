namespace Builder
{
    // Вариант Директора с установкой Строителя через метод.
    public class DirectorB
    {
        public void BuildMinimalProduct(IBuilder builder)
        {
            builder.BuildPartA();
        }

        public void BuildFullProduct(IBuilder builder)
        {
            builder.BuildPartA();
            builder.BuildPartB();
            builder.BuildPartC();
        }
    }
}
