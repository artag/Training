using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CreatingGlobalFilters.Infrastructure
{
    public class ViewResultDiagnostics : IActionFilter
    {
        private readonly IFilterDiagnostics _diagnostics;

        public ViewResultDiagnostics(IFilterDiagnostics diagnostics)
        {
            _diagnostics = diagnostics;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ViewResult vr;
            if ((vr = context.Result as ViewResult) == null)
            {
                return;
            }

            _diagnostics.AddMessage($"View name: {vr.ViewName}");
            _diagnostics.AddMessage($"Model type: {vr.ViewData.Model.GetType().Name}");
        }
    }
}
