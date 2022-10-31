using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.UseCases.Dto;

namespace Web.Controllers;

[ApiController]
[Route("dummy")]
public class DummyController : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<SomeStuffDto> Get(int id)
    {
        throw new NotImplementedException();
    }
}
