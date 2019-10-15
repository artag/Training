using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCamps()
        {
            return Ok(new {Moniker = "ATL2018", Name = "Atlanta Code Camp" });
        }
    }
}
