using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;

namespace Clean.Architecture.UseCases.Project.Commands.CreateProject;

public class CreateProjectCommand : IRequest<ProjectDTO>
{
    public CreateProjectDTO Dto { get; init; } = null!;
}
