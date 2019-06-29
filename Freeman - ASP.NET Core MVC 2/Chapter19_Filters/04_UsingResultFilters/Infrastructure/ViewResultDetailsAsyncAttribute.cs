using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UsingResultFilters.Infrastructure
{
    public class ViewResultDetailsAsyncAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var dict = new Dictionary<string, string>();
            dict["Result Type"] = context.Result.GetType().Name;

            ViewResult vr;
            if ((vr = context.Result as ViewResult) != null)
            {
                dict["View Name"] = vr.ViewName;
                dict["Model Type"] = vr.ViewData.Model.GetType().Name;
                dict["Model Data"] = vr.ViewData.Model.ToString();
            }

            context.Result = new ViewResult
            {
                ViewName = "Message",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = dict
                }
            };

            await next();
        }
    }
}
