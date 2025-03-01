using Newtonsoft.Json;

namespace DataLayer.Daos;

public class ToDoDao : ICosmosDao
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Status { get; init; }

}