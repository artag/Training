using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    partial class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            DisposeTestsRun(services);
            BindingInCollectionTestsRun(services);
            BindingGenericTestsRun(services);
            ValidateScopesTestsRun(services);
        }
    }
}
