using System.Text;
using DILifetime.Services;

namespace DILifetime;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<ScopedService>();
        builder.Services.AddTransient<TransientService>();
        builder.Services.AddSingleton<SingletonService>();

        var app = builder.Build();
        app.MapGet("/", async context =>
        {
            context.Response.ContentType = "text/html;charset=utf-8";

            var html = new StringBuilder();
            html.AppendLine("Первое обращение к сервисам. <br/>");
            var transientService1 = context.RequestServices.GetRequiredService<TransientService>();
            var scopedService1 = context.RequestServices.GetRequiredService<ScopedService>();
            var singletonService1 = context.RequestServices.GetRequiredService<SingletonService>();
            Display(html, transientService1, scopedService1, singletonService1);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            html.AppendLine("Второе обращение к сервисам. <br/>");
            var transientService2 = context.RequestServices.GetRequiredService<TransientService>();
            var scopedService2 = context.RequestServices.GetRequiredService<ScopedService>();
            var singletonService2 = context.RequestServices.GetRequiredService<SingletonService>();
            Display(html, transientService2, scopedService2, singletonService2);

            await context.Response.WriteAsync(html.ToString());
        });

        app.Run();
    }

    private static void Display(StringBuilder sb, params IService[] services)
    {
        sb.AppendLine("Созданные сервисы:<br/>");
        foreach (var service in services)
        {
            sb.AppendLine($"{service.ToString()}<br/>");
        }
        sb.AppendLine("<hr/>");
        sb.AppendLine();
    }
}
