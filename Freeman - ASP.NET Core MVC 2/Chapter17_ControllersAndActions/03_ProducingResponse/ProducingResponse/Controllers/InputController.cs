using Microsoft.AspNetCore.Mvc;

namespace ProducingResponse.Controllers
{
    public class InputController : Controller
    {
        public IActionResult ToNotRecommendedResponse()
        {
            ViewBag.DistantAction = nameof(ResponseController.SendNotRecommendedResponse);
            return View("SendData");
        }

        public IActionResult ToCustomHtmlResult()
        {
            ViewBag.DistantAction = nameof(ResponseController.SendCustomHtmlResult);
            return View("SendData");
        }

        public IActionResult ToSendViewResult()
        {
            ViewBag.DistantAction = nameof(ResponseController.SendViewResult);
            return View("SendData");
        }
    }
}
