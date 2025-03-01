using BusinessLogic.Blas;
using DataLayer.Daos;
using System.Collections.Immutable;

namespace BusinessLogic.DataManager
{
    public interface IToDoDataManager
    {
        Task<string> AddAsync(ToDoPostRequestBodyBla request);
        Task<ImmutableArray<ToDoEntityBla>> GetAllAsync(string? Description);
        Task<ToDoEntityBla?> GetAsync(string id);
        Task<ImmutableArray<ToDoEntityBla>> GetByStatus(string status);
        Task<QueryByPageTodoResultBla> GetByPageAsync(string? continuationToken, int pageSize);
        Task<bool> DeleteAsync(string id, string title);
        Task<bool> UpdateItemAsync(string id, string title, string attributeToUpdate, string attributeValue);
        Task<bool> UpdateItemAsync(ToDoDao item);
    }
}
