using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ReceivingContextData.Controllers
{
    public class PocoController
    {
        [ControllerContext]
        public ControllerContext ControllerContext { get; set; }

        public ViewResult Index()
        {
            var viewResult = new ViewResult
            {
                ViewName = "Result",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = "This is a POCO Cotroller"
                }
            };

            return viewResult;
        }

        public ViewResult Headers()
        {
            var viewResult = new ViewResult
            {
                ViewName = "DictionaryResult",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ControllerContext.HttpContext.Request.Headers
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First())
                }
            };

            return viewResult;
        }
    }
}
