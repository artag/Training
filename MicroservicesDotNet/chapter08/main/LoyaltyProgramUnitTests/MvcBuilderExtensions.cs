using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyProgramUnitTests;

public static class MvcBuilderExtensions
{
    // (1) - Make the service collection aware (знающие о) of controllers.
    // (2) - Get access to feature providers.
    // (3) - Remove the default controller provider feature.
    // (4) - Add a custom controller provider feature.
    public static IMvcBuilder AddControllersByType(
        this IServiceCollection services,
        params Type[] controllerTypes) =>
        services
            .AddControllers()                           // (1)
            .ConfigureApplicationPartManager(mgr =>     // (2)
            {
                mgr.FeatureProviders.Remove(            // (3)
                    mgr.FeatureProviders.First(f => f is ControllerFeatureProvider));
                mgr.FeatureProviders.Add(               // (4)
                    new FixedControllerProvider(controllerTypes));
            });
}
