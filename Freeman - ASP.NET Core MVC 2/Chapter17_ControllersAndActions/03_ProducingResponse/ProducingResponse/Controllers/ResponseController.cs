using System.Text;
using Microsoft.AspNetCore.Mvc;
using ProducingResponse.Infrastructure;

namespace ProducingResponse.Controllers
{
    public class ResponseController : Controller
    {
        public void SendNotRecommendedResponse(string name, string city)
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/html";

            var content = Encoding.ASCII.GetBytes(
                $"<html><body>{name} lives in {city}</body></html>");

            Response.Body.WriteAsync(content, 0, content.Length);
        }

        public IActionResult SendCustomHtmlResult(string name, string city)
        {
            return new CustomHtmlResult
            {
                Content = $"{name} lives in {city}"
            };
        }

        public ViewResult SendViewResult(string name, string city) =>
            View("Result", $"{name} lives in {city}");
    }
}
