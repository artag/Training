using Clean.Architecture.Entities.ProjectAggregate.Specifications;
using Clean.Architecture.Infrastructure.Interfaces;
using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;
using ProjectEntity = Clean.Architecture.Entities.ProjectAggregate.Project;

namespace Clean.Architecture.UseCases.Project.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO?>
{
    private readonly IRepository<ProjectEntity> _repository;

    public GetProjectByIdQueryHandler(IRepository<ProjectEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ProjectDTO?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(request.Id);
        var project = await _repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
            return null;

        var items = project.Items
            .Select(i => ToDoItemDTO.FromToDoItem(i))
            .ToList();

        var result = new ProjectDTO(
            id: project.Id,
            name: project.Name,
            items: new List<ToDoItemDTO>(items));

        return result;
    }
}
