using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UsingExceptionFilters.Infrasructure
{
    public class RangeExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is ArgumentOutOfRangeException))
            {
                return;
            }

            context.Result = new ViewResult
            {
                ViewName = "Message",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = @"The data received by the application cannot be processed"
                }
            };
        }
    }
}
