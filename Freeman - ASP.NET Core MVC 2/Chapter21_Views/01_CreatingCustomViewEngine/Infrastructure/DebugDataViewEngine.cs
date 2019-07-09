using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CreatingCustomViewEngine.Infrastructure
{
    public class DebugDataViewEngine : IViewEngine
    {
        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            var searchedLocations = new string[] { "(Debug View Engine - GetView)" };
            return ViewEngineResult.NotFound(viewPath, searchedLocations);
        }

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            if (viewName == "DebugData")
            {
                return ViewEngineResult.Found(viewName, new DebugDataView());
            }

            var searchedLocations = new string[] { "(Debug View Engine - FindView)" };
            return ViewEngineResult.NotFound(viewName, searchedLocations);
        }
    }
}
