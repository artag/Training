using Clean.Architecture.UseCases.Project.Commands.CompleteProject;
using Clean.Architecture.UseCases.Project.Commands.CreateProject;
using Clean.Architecture.UseCases.Project.Dtos;
using Clean.Architecture.UseCases.Project.Queries.GetProjectById;
using Clean.Architecture.UseCases.Project.Queries.GetProjectsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Controllers.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class ProjectsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Projects
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var projectDTOs = await _mediator.Send(new GetProjectsListQuery());
        return Ok(projectDTOs);
    }

    // GET: api/Projects/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery { Id = id });
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // POST: api/Projects
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProjectDTO request)
    {
        var result = await _mediator.Send(new CreateProjectCommand { Dto = request });
        return Ok(result);
    }

    // PATCH: api/Projects/{projectId}/complete/{itemId}
    [HttpPatch("{projectId:int}/complete/{itemId}")]
    public async Task<IActionResult> Complete(int projectId, int itemId)
    {
        var result = await _mediator.Send(new CompleteProjectCommand { ProjectId = projectId, ItemId = itemId });
        if (result == CompleteProjectCommandResult.NoProject)
            return NotFound("No such project");

        if (result == CompleteProjectCommandResult.NoItem)
            return NotFound("No such item.");

        return Ok();
    }
}
