using BusinessLogic.Blas;
using BusinessLogic.DataManager;
using System.Collections.Immutable;
using BusinessLogic.DaoToBlaMappers;
using BusinessLogic.Enums;

namespace BusinessLogic.Services
{
    public sealed class ToDoService(IToDoDataManager dataManager) : IToDoService
    {
        public async Task<string> CreateNewTodo(ToDoPostRequestBodyBla request)
        {
            return await dataManager.AddAsync(request);
        }

        public async Task<ToDoEntityBla> GetAsync(string id)
        {
            var res =  await dataManager.GetAsync(id);

            return res ?? throw new Exception("Resource not found"); //I would return HTTP 404 with a businessException in this case if this was a real app,
                                                                     //I just feel like that's out of scope here to implement
        }

        public async Task<ImmutableArray<ToDoEntityBla>> GetAllAsync(string? description)
        {
            var res =  await dataManager.GetAllAsync(description);

            return res.Length == 0 ? throw new Exception("No resources found") : res;
        }
        public async Task<ImmutableArray<ToDoEntityBla>> GetAllByStatus(StatusBlaEnum status)
        {
            var res = await dataManager.GetByStatus(status.ToString());

            return res.Length == 0 ? throw new Exception("No resources found") : res;
        }

        public async Task DeleteAsync(string id)
        {
            var item = await dataManager.GetAsync(id);

            if (item != null)
            {
                var res = await dataManager.DeleteAsync(id, item.Title);
                if (res == false)
                {
                    throw new Exception("Could not delete id from table");
                }
            }
            else
            {
                throw new Exception("Resource not found");
            }
        }

        public async Task UpdateAsync(ToDoChangeRequestBla request)
        {
            var item = await dataManager.GetAsync(request.Id);

            if (item != null)
            {
                item.Status = request.Status ?? item.Status;
                item.Description = request.Description ?? item.Description;
                var res = await dataManager.UpdateItemAsync(item.MapToDao());

                if (res == false)
                {
                    throw new Exception("Could not update record in table");
                }
            }
            else
            {
                throw new Exception("Resource not found");
            }
        }

        public async Task<QueryByPageTodoResultBla> ListByPageAsync(string? continuationToken, int pageSize)
        {
            return await dataManager.GetByPageAsync(continuationToken, pageSize);
        }
    }
}
