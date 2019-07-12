using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ConfiguringRazor.Infrastructure
{
    public class SimpleExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            foreach (var location in viewLocations)
            {
                yield return location.Replace("Shared", "Common");
            }

            yield return "/Views/Legacy/{1}/{0}/View.cshtml";
        }
    }
}
