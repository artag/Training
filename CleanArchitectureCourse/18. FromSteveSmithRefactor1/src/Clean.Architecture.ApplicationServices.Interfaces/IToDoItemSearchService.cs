using Ardalis.Result;
using Clean.Architecture.Entities.ProjectAggregate;

namespace Clean.Architecture.ApplicationServices.Interfaces;

public interface IToDoItemSearchService
{
  Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId);
  Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString);
}
