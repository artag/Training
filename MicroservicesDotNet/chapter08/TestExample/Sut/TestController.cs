using Microsoft.AspNetCore.Mvc;

namespace Sut;

public class TestController : ControllerBase
{
    // Endpoint used in the test
    [HttpGet("/")]
    public Task<OkResult> Get() => Task.FromResult(Ok());
}
