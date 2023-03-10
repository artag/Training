using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace LoyaltyProgramUnitTests;

// Custom implementation of the controller provider.
public class FixedControllerProvider : ControllerFeatureProvider
{
    private readonly Type[] _controllerTypes;

    public FixedControllerProvider(params Type[] controllerTypes) =>
        _controllerTypes = controllerTypes;

    // Override the method used to identify controllers.
    protected override bool IsController(TypeInfo typeInfo) =>
        _controllerTypes.Contains(typeInfo);
}
