using System;
using System.Threading.Tasks;
using App.Implementations;
using App.Interfaces;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Implementations;
using Repository.Interfaces;
using UseCases;

namespace Demo;

internal static class Container
{
    public static ServiceProvider Create()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole());

        services.AddSingleton<DummyData>();
        services.AddSingleton<IService<GetConfigurationRequest, GetConfigurationResponse>, GetConfigurationService>();

        // Services
        services.AddScoped(c => CreateGetProductsService(c));
        services.AddScoped(c => CreateFindProductService(c));
        services.AddScoped(c => CreateDeleteProductService(c));

        // Use cases
        services.AddScoped<ListAllProducts>();
        services.AddScoped<GetProductByName>();
        services.AddScoped<DeleteProductByName>();

        var container = services.BuildServiceProvider();
        return container;
    }

    private static IService<GetProductsRequest, GetProductsResponse> CreateGetProductsService(IServiceProvider c)
    {
        GetProductsService CreateService(IServiceProvider c) =>
            new GetProductsService(
                c.GetService<GetConfigurationRequest, GetConfigurationResponse>(),
                c.GetLogger<GetProductsService>(),
                c.Get<DummyData>());

        var service = new LogHandler<GetProductsRequest, GetProductsResponse>(
            CreateService(c),
            c.GetLoggerFactory());

        return service;
    }

    private static IService<FindProductRequest, FindProductResponse> CreateFindProductService(
        IServiceProvider c)
    {
        FindProductService CreateService(IServiceProvider c) =>
            new FindProductService(
                c.GetService<GetConfigurationRequest, GetConfigurationResponse>(),
                c.GetLogger<FindProductService>(),
                c.Get<DummyData>());

        var service = new LogHandler<FindProductRequest, FindProductResponse>(
            new ExceptionHandler<FindProductRequest,FindProductResponse, InvalidOperationException>(
                CreateService(c),
                c.GetLoggerFactory(),
                Handler1),
            c.GetLoggerFactory());

        return service;
    }

    private static IService<DeleteProductRequest, DeleteProductResponse> CreateDeleteProductService(
        IServiceProvider c)
    {
        DeleteProductService CreateService(IServiceProvider c) =>
            new DeleteProductService(
                c.GetService<GetConfigurationRequest, GetConfigurationResponse>(),
                c.GetLogger<DeleteProductService>(),
                c.Get<DummyData>());

        var service = new ExceptionHandler<DeleteProductRequest, DeleteProductResponse, InvalidOperationException>(
                CreateService(c),
                c.GetLoggerFactory(),
                Handler2);

        return service;
    }

    private static Task<Result<FindProductResponse>> Handler1(InvalidOperationException ex)
    {
        var result = Result.Failure<FindProductResponse>(new FindProductError("Can't found product."));
        return Task.FromResult(result);
    }

    private static Task<Result<DeleteProductResponse>> Handler2(InvalidOperationException ex)
    {
        var result = Result.Failure<DeleteProductResponse>(new DeleteProductError("Can't delete product"));
        return Task.FromResult(result);
    }
}

internal static class IServiceProviderExtensions
{
    public static ILoggerFactory GetLoggerFactory(this IServiceProvider c) =>
        c.GetRequiredService<ILoggerFactory>();

    public static ILogger<T> GetLogger<T>(this IServiceProvider c) =>
        c.GetRequiredService<ILogger<T>>();

    public static IService<TRequest, TResponse> GetService<TRequest, TResponse>(this IServiceProvider c) =>
        c.GetRequiredService<IService<TRequest, TResponse>>();

    public static T Get<T>(this IServiceProvider c) =>
        c.GetRequiredService<T>();
}