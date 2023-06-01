using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UseCases;

namespace Demo;

internal class Program
{
    static async Task Main(string[] args)
    {
        await using var container = Container.Create();
        var logger = container.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
        logger.LogInformation("Program started...");

        logger.LogInformation("Create scope 1");
        using (var scope = container.CreateScope())
        {
            var listProducts = scope.ServiceProvider.GetRequiredService<ListAllProducts>();
            var listResult = await listProducts.Do();
            if (listResult.IsFailure)
                Console.WriteLine($"Error: {listResult.Error.Message}");
        }

        logger.LogInformation("Create scope 2");
        using (var scope = container.CreateScope())
        {
            var findProduct = scope.ServiceProvider.GetRequiredService<GetProductByName>();

            var findResult = await findProduct.Do("Unknown");
            Console.WriteLine(findResult.IsSuccess
                ? $"Find product: {findResult.Value}"
                : $"Error: {findResult.Error.Message}");

            var findResult2 = await findProduct.Do("Jeans");
            Console.WriteLine(findResult2.IsSuccess
                ? $"Find product: {findResult2.Value}"
                : $"Error: {findResult2.Error.Message}");
        }

        logger.LogInformation("Create scope 3");
        using (var scope = container.CreateScope())
        {
            var deleteProduct = scope.ServiceProvider.GetRequiredService<DeleteProductByName>();

            var deleteResult = await deleteProduct.Do("Unknown");
            Console.WriteLine(deleteResult.IsSuccess
                ? $"Product 'Unknown' succesfully deleted"
                : $"Error: {deleteResult.Error.Message}");

            var deleteResult2 = await deleteProduct.Do("Jeans");
            Console.WriteLine(deleteResult2.IsSuccess
                ? $"Product 'Jeans' succesfully deleted"
                : $"Error: {deleteResult2.Error.Message}");

            var findProduct = scope.ServiceProvider.GetRequiredService<GetProductByName>();
            var findResult = await findProduct.Do("Jeans");
            Console.WriteLine(findResult.IsSuccess
                ? $"Find product: {findResult.Value}"
                : $"Error: {findResult.Error.Message}");
        }
    }


}
