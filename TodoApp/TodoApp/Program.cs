namespace TodoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            AddConfig(builder);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            AddServices(builder.Services);
            AddDataLayer(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void AddConfig(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTodoServices();
            services.AddSequenceGenerator();
        }
        private static void AddDataLayer(IServiceCollection services)
        {
            services.RegisterCosmosDb();
            services.AddTodoDataAccessLayer();
        }
    }
}
