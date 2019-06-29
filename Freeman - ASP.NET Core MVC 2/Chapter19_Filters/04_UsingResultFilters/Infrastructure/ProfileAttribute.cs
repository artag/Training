using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using UsingResultFilters.Controllers;

namespace UsingResultFilters.Infrastructure
{
    public class ProfileAttribute : ActionFilterAttribute
    {
        private Stopwatch _timer;
        private double _actionTime;

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _timer = Stopwatch.StartNew();

            await next();

            _actionTime = _timer.Elapsed.TotalMilliseconds;
        }

        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _timer.Stop();

            var actionTime = $"{_actionTime} ms";
            var elapsedTime = $"{_timer.Elapsed.TotalMilliseconds} ms";

            var controller = context.Controller as HomeController;
            controller.ViewData["Action Time"] = actionTime;
            controller.ViewData["Elapsed Time"] = elapsedTime;

            await next();
        }
    }
}
