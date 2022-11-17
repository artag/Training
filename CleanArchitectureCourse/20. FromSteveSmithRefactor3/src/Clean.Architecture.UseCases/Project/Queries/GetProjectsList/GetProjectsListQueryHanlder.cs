using Clean.Architecture.Infrastructure.Interfaces;
using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;
using ProjectEntity = Clean.Architecture.Entities.ProjectAggregate.Project;

namespace Clean.Architecture.UseCases.Project.Queries.GetProjectsList;

public class GetProjectsListQueryHanlder : IRequestHandler<GetProjectsListQuery, List<ProjectDTO>>
{
    private readonly IRepository<ProjectEntity> _repository;

    public GetProjectsListQueryHanlder(IRepository<ProjectEntity> repository)
    {
        _repository = repository;
    }

    public async Task<List<ProjectDTO>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.ListAsync();
        var projectDTOs = projects
            .Select(project => new ProjectDTO(id: project.Id, name: project.Name))
            .ToList();

        return projectDTOs;
    }
}
