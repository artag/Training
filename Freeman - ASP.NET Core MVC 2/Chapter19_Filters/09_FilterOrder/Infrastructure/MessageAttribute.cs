using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilterOrder.Infrastructure
{
    public class MessageAttribute : ResultFilterAttribute
    {
        private readonly string _message;

        public MessageAttribute(string message)
        {
            _message = message;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as Controller;
            var key = $"Before Result: {_message}";
            controller.ViewData[key] = string.Empty;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var message = $"After Result: {_message}";
            var bytes = Encoding.ASCII.GetBytes($"<div>{message}</div>");
            context.HttpContext.Response.Body.Write(bytes, 0, bytes.Length);
        }
    }
}
