// (1) Adds MVC controller and helper services to the service collection
// (2) Use Scrutor to scan the ShoppingCart assembly

// (3) Register ProductCatalogClient as typed http client
// (4) Wraps http calls made in ProductCatalogClient in a Polly policy
// (5) Uses Polly’s fluent API to set up a retry policy with an exponential back-off

// (6) Redirects all HTTP requests to HTTPS
// (7) Adds all endpoints in all controllers to MVCs route table

using Polly;
using ShoppingCart;
using ShoppingCart.EventFeed;
using ShoppingCart.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();                // (1)

// Толком не заработало
//builder.Services.Scan(selector => selector      // (2)
//    .FromAssemblyOf<Program>()
//    .AddClasses(classes => classes.AssignableTo<IShoppingCartStore>())
//    .AsImplementedInterfaces()
//    .WithTransientLifetime());

builder.Services.AddTransient<IShoppingCartStore, ShoppingCartStore>();
builder.Services.AddTransient<IEventStore, SqlEventStore>();
builder.Services.AddSingleton<ICache, Cache>();

builder.Services
    .AddHttpClient<IProductCatalogClient, ProductCatalogClient>()   // (3)
    .AddTransientHttpErrorPolicy(p =>                               // (4)
        p.WaitAndRetryAsync(                                        // (5)
            retryCount: 3,
            attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))));

var app = builder.Build();

app.UseHttpsRedirection();                                      // (6)
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());      // (7)

app.Run();
