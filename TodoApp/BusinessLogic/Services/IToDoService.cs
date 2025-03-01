using BusinessLogic.Blas;
using BusinessLogic.Enums;
using System.Collections.Immutable;

namespace BusinessLogic.Services
{
    public interface IToDoService
    {
        Task<string> CreateNewTodo(ToDoPostRequestBodyBla requestBla);
        Task<ToDoEntityBla> GetAsync(string id);
        Task<ImmutableArray<ToDoEntityBla>> GetAllAsync(string? description);
        Task<ImmutableArray<ToDoEntityBla>> GetAllByStatus(StatusBlaEnum status);
        Task DeleteAsync(string id);
        Task UpdateAsync(ToDoChangeRequestBla request);
        Task<QueryByPageTodoResultBla> ListByPageAsync(string? continuationToken, int pageSize);
    }
}
