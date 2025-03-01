using System.Collections.Immutable;
using BusinessLogic.Blas;
using BusinessLogic.DaoToBlaMappers;
using BusinessLogic.SequenceGenerator;
using DataLayer.Daos;

namespace BusinessLogic.DataManager
{
    public sealed class ToDoDataManager(ICosmosDataAccessManager<ToDoDao> dataAccess, ISequenceGenerator sequenceGenerator) : IToDoDataManager
    {
        public async Task<string> AddAsync(ToDoPostRequestBodyBla request)
        {
            string nextId = sequenceGenerator.GetNext().ToString();
            await dataAccess.AddItemAsync(new ToDoDao
            {
                Id = nextId,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status.ToString()
            });
            return nextId;
        }
        public async Task<ImmutableArray<ToDoEntityBla>> GetAllAsync(string? description)
        {
            const string sqlQueryText = "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo";
            
            var dbResult = await dataAccess.QueryItemsAsync(sqlQueryText);

            if (description != null)
            {
                return dbResult.Count == 0 ? [] : [.. dbResult.Where(x => x.Description.Contains(description, StringComparison.OrdinalIgnoreCase)).Select(y => y.MapToBla())];
            }

            return dbResult.Count == 0 ? [] : [..dbResult.Select(x => x.MapToBla())];
        }
        public async Task<QueryByPageTodoResultBla> GetByPageAsync(string? continuationToken, int pageSize)
        {
            const string sqlQueryText = "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo";

            var dbResult = await dataAccess.QueryItemsByPageAsync(sqlQueryText, continuationToken, pageSize);

            return dbResult.Results.Count == 0 ? new QueryByPageTodoResultBla(continuationToken, ImmutableArray<ToDoEntityBla>.Empty)
                : new QueryByPageTodoResultBla(dbResult.ContinuationToken, [..dbResult.Results.Select(x => x.MapToBla())]);
        }
        public async Task<ToDoEntityBla?> GetAsync(string id)
        {
            const string sqlQueryText = "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo Where Todo.id = @id";
            var parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };

            var dbResult = await dataAccess.QueryItemsAsync(sqlQueryText, parameters);

            return dbResult.Count == 0 ? null : dbResult.Select(x => x.MapToBla()).First();
        }
        public async Task<ImmutableArray<ToDoEntityBla>> GetByStatus(string status)
        {
            const string sqlQueryText = "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo Where Todo.Status = @status";
            var parameters = new Dictionary<string, object>
            {
                { "@status", status }
            };

            var dbResult = await dataAccess.QueryItemsAsync(sqlQueryText, parameters);

            return dbResult.Count == 0 ? [] : [..dbResult.Select(x => x.MapToBla())];
        }
        public async Task<bool> DeleteAsync(string id, string title)
        {
            return await dataAccess.DeleteItemAsync(id, title);
        }
        public async Task<bool> UpdateItemAsync(ToDoDao item)
        {
            return await dataAccess.UpsertItemAsync(item, item.Title);
        }

        //I just decided not to use it
        public async Task<bool> UpdateItemAsync(string id, string title, string attributeToUpdate, string attributeValue)
        {
            return await dataAccess.UpdateSingleAttributeAsync(id, title, attributeToUpdate, attributeValue);
        }
    }
}
