using DataLayer.Daos;

namespace BusinessLogic.DataManager
{
    public interface ICosmosDataAccessManager<TDao> where TDao : ICosmosDao
    {
        Task AddItemAsync(TDao item);
        Task CreateDatabaseAsync(string dbId, string containerName);
        Task<List<TDao>> QueryItemsAsync(string queryDefinition);
        Task<List<TDao>> QueryItemsAsync(string queryText, Dictionary<string, object> parameters);
        Task<bool> DeleteItemAsync(string id, string pk);
        Task<bool> UpdateSingleAttributeAsync(string id, string pk, string attributeName, string attributeValue);
        Task<bool> UpsertItemAsync(TDao item, string partitionKey);
        Task<QueryByPageGenericResult<TDao>> QueryItemsByPageAsync(string queryText, string? continuationToken, int pageSize);
    }
}
