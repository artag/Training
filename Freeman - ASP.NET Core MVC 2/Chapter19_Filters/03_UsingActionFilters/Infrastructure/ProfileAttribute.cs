using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using UsingActionFilters.Controllers;

namespace UsingActionFilters.Infrastructure
{
    public class ProfileAttribute : ActionFilterAttribute
    {
        private Stopwatch _timer;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();

            var result = $"{_timer.Elapsed.TotalMilliseconds} ms";
            var controller = context.Controller as HomeController;
            controller.ViewData["ElapsedTime"] = result;
        }
    }
}
