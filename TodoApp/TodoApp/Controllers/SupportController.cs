using BusinessLogic.DataManager;
using DataLayer.Daos;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupportController(ICosmosDataAccessManager<ToDoDao> dataAccess, IConfiguration config) : ControllerBase
    {
        [HttpGet("CreateDb")]
        public async Task<IActionResult> CreateDb()
        {
            await dataAccess.CreateDatabaseAsync(config["CosmosDb:DbId"]!, config["CosmosDb:ContainerId"]!);
            return new NoContentResult();
        }
    }
}
