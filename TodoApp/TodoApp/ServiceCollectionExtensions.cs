using BusinessLogic.DataManager;
using BusinessLogic.SequenceGenerator;
using BusinessLogic.Services;
using DataLayer.Daos;
using DataLayer.DataAccessManagers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddScoped<IToDoService, ToDoService>();
            return services;
        }
        public static IServiceCollection AddTodoDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IToDoDataManager, ToDoDataManager>();
            return services;
        }
        public static IServiceCollection AddSequenceGenerator(this IServiceCollection services)
        {
            services.AddSingleton<ISequenceGenerator, SequenceGenerator>();
            return services;
        }
        public static void RegisterCosmosDb(this IServiceCollection services)
        {
            services.AddScoped<ICosmosDataAccessManager<ToDoDao>, CosmosDataAccessManager<ToDoDao>>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                string endpointUri = configuration["CosmosDb:EndpointUri"]!; //No config validator due to the app's simplicity
                string primaryKey = configuration["CosmosDb:PrimaryKey"]!;
                var cosmosClient = new CosmosClient(endpointUri, primaryKey);
                return new CosmosDataAccessManager<ToDoDao>(cosmosClient, configuration["CosmosDb:DbId"]!,
                    configuration["CosmosDb:ContainerId"]!, new Logger<CosmosDataAccessManager<ToDoDao>>(new LoggerFactory()));
            });

        }

    }
}
