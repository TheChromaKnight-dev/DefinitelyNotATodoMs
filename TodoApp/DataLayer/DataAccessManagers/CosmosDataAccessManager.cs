using BusinessLogic.DataManager;
using DataLayer.Daos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Container = Microsoft.Azure.Cosmos.Container;

namespace DataLayer.DataAccessManagers
{
    public sealed class CosmosDataAccessManager<TDao> : ICosmosDataAccessManager<TDao> where TDao : ICosmosDao
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<CosmosDataAccessManager<TDao>> _logger;
        public CosmosDataAccessManager(CosmosClient cosmosClient, string dbId, string containerName, ILogger<CosmosDataAccessManager<TDao>> logger)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(dbId, containerName);
        }

        public async Task AddItemAsync(TDao item)
        {
            try
            {
                await _container.CreateItemAsync(item);
            }
            catch (CosmosException ce) when (ce.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                _logger.LogError(ce, "Resource already exists");
                throw new Exception("This Resource already exists");
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Cosmos Exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected Exception");
            }

        }
        public async Task<bool> DeleteItemAsync(string id, string pk)
        {
            try
            {
                await _container.DeleteItemAsync<TDao>(id, new PartitionKey(pk));
                return true;
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Cosmos Exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected Exception");
            }

            return false;

        }
        public async Task<bool> UpdateSingleAttributeAsync(string id, string pk, string attributeName, string attributeValue)
        {
            try
            {
                await _container.PatchItemAsync<TDao>(id, new PartitionKey(pk),new PatchOperation[]
                {
                    PatchOperation.Replace(attributeName, attributeValue)
                });
                return true;
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Cosmos Exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected Exception");
            }

            return false;

        }
        public async Task<bool> UpsertItemAsync(TDao item, string partitionKey)
        {
            try
            {
                await _container.UpsertItemAsync(item, new PartitionKey(partitionKey));
                return true;
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Cosmos Exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected Exception");
            }

            return false;
        }
        public async Task<List<TDao>> QueryItemsAsync(string queryText)
        {
            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(queryText);
                using FeedIterator<TDao> queryResultSetIterator = _container.GetItemQueryIterator<TDao>(queryDefinition);
                List<TDao> items = new List<TDao>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<TDao> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (TDao item in currentResultSet)
                    {
                        items.Add(item);
                    }
                }
                return items;
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Could not Query data, see inner Exception");
                throw new Exception("Unable to query from Cosmos");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                throw new Exception("Unable to query from Cosmos");
            }
           
        }
        public async Task<List<TDao>> QueryItemsAsync(string queryText, Dictionary<string, object> parameters)
        {
            try
            {
                var queryDefinition = new QueryDefinition(queryText);

                foreach (var parameter in parameters)
                {
                    queryDefinition.WithParameter(parameter.Key, parameter.Value);
                }

                using var iterator = _container.GetItemQueryIterator<TDao>(queryDefinition);
                var items = new List<TDao>();

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    items.AddRange(response.ToList());
                }

                return items;
            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Could not Query data, see inner Exception");
                throw new Exception("Unable to query from Cosmos");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                throw new Exception("Unable to query from Cosmos");
            }

        }
        public async Task<QueryByPageGenericResult<TDao>> QueryItemsByPageAsync(string queryText, string? continuationToken, int pageSize)
        {
            try
            {
                var queryRequestOptions = new QueryRequestOptions
                {
                    MaxItemCount = pageSize
                };
                var queryDefinition = new QueryDefinition(queryText);

                using var iterator =
                    _container.GetItemQueryIterator<TDao>(queryDefinition, continuationToken, queryRequestOptions);
                var results = new List<TDao>();
                string? newContinuationToken = null;

                if (!iterator.HasMoreResults) return new QueryByPageGenericResult<TDao>()
                {
                    ContinuationToken = newContinuationToken,
                    Results = results
                };
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
                newContinuationToken = response.ContinuationToken;

                return new QueryByPageGenericResult<TDao>()
                {
                    ContinuationToken = newContinuationToken,
                    Results = results
                };

            }
            catch (CosmosException ce)
            {
                _logger.LogError(ce, "Could not Query data, see inner Exception");
                throw new Exception("Unable to query from Cosmos");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                throw new Exception("Unable to query from Cosmos");
            }
        }

        public async Task CreateDatabaseAsync(string dbId, string containerName)
        {
            // Create a new database
            var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbId, 1000);
            await CreateContainerAsync(database.Database, containerName);
        }
        private async Task CreateContainerAsync(Database database, string containerName)
        {
            // Create a new container
            await database.CreateContainerIfNotExistsAsync(containerName, "/Title", 1000);
        }
    }
}
