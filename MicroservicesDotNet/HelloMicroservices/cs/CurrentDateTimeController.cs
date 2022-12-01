using Microsoft.AspNetCore.Mvc;

namespace HelloMicroservices;

public class CurrentDateTimeController : ControllerBase
{
    [HttpGet("/")]
    public object Get() => DateTime.Now;
}
