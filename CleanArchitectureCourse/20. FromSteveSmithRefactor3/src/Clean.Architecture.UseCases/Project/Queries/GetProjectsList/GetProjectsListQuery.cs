using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;

namespace Clean.Architecture.UseCases.Project.Queries.GetProjectsList;

public class GetProjectsListQuery : IRequest<List<ProjectDTO>>
{
}
