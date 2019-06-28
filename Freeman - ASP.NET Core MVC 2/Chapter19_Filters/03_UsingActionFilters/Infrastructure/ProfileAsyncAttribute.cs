using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using UsingActionFilters.Controllers;

namespace UsingActionFilters.Infrastructure
{
    public class ProfileAsyncAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var timer = Stopwatch.StartNew();

            await next();

            timer.Stop();

            var result = $"{timer.Elapsed.TotalMilliseconds} ms";
            var controller = context.Controller as HomeController;
            controller.ViewData["ElapsedTime"] = result;
        }
    }
}
