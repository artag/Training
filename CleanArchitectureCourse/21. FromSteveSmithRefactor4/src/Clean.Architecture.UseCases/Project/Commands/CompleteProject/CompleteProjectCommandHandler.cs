using Clean.Architecture.Entities.ProjectAggregate.Specifications;
using Clean.Architecture.Infrastructure.Interfaces;
using MediatR;
using ProjectEntity = Clean.Architecture.Entities.ProjectAggregate.Project;

namespace Clean.Architecture.UseCases.Project.Commands.CompleteProject;

public class CompleteProjectCommandHandler : IRequestHandler<CompleteProjectCommand, CompleteProjectCommandResult>
{
    private readonly IRepository<ProjectEntity> _repository;

    public CompleteProjectCommandHandler(IRepository<ProjectEntity> repository)
    {
        _repository = repository;
    }

    public async Task<CompleteProjectCommandResult> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectSpec = new ProjectByIdWithItemsSpec(request.ProjectId);
        var project = await _repository.FirstOrDefaultAsync(projectSpec);
        if (project == null)
            return CompleteProjectCommandResult.NoProject;

        var toDoItem = project.Items.FirstOrDefault(item => item.Id == request.ItemId);
        if (toDoItem == null)
            return CompleteProjectCommandResult.NoItem;

        toDoItem.MarkComplete();
        await _repository.UpdateAsync(project);
        return CompleteProjectCommandResult.OK;
    }
}
