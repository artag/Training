using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResolvingFilterDependencies.Infrastructure
{
    public class TimeFilter : IAsyncActionFilter, IAsyncResultFilter
    {
        private Stopwatch _timer;
        private IFilterDiagnostics _diagnostics;

        public TimeFilter(IFilterDiagnostics diagnostics)
        {
            _diagnostics = diagnostics;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _timer = Stopwatch.StartNew();
            await next();
            _diagnostics.AddMessage($"Action time: {_timer.Elapsed.TotalMilliseconds} ms");
        }

        public async Task OnResultExecutionAsync(
            ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next();
            _timer.Stop();
            _diagnostics.AddMessage($"Result time: {_timer.Elapsed.TotalMilliseconds} ms");
        }
    }
}
