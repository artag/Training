using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;

namespace Clean.Architecture.UseCases.Project.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDTO?>
{
    public int Id { get; init; }
}
