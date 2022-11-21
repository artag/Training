using Clean.Architecture.Entities.ProjectAggregate;
using Clean.Architecture.Infrastructure.Interfaces;
using Clean.Architecture.UseCases.Project.Dtos;
using MediatR;
using ProjectEntity = Clean.Architecture.Entities.ProjectAggregate.Project;

namespace Clean.Architecture.UseCases.Project.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDTO>
{
    private readonly IRepository<ProjectEntity> _repository;

    public CreateProjectCommandHandler(IRepository<ProjectEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ProjectDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var newProject = new ProjectEntity(request.Dto.Name, PriorityStatus.Backlog);
        var createdProject = await _repository.AddAsync(newProject);
        var result = new ProjectDTO
        (
            id: createdProject.Id,
            name: createdProject.Name
        );

        return result;
    }
}
