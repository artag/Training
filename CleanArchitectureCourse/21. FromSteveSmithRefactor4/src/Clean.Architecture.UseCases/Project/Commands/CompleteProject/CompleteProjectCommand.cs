using MediatR;
using ProjectEntity = Clean.Architecture.Entities.ProjectAggregate.Project;

namespace Clean.Architecture.UseCases.Project.Commands.CompleteProject;

public class CompleteProjectCommand : IRequest<CompleteProjectCommandResult>
{
    public int ProjectId { get; init; }
    public int ItemId { get; init; }
}
