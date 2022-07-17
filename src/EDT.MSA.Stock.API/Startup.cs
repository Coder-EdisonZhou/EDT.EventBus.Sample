using EDT.MSA.Stocking.API.Events;
using EDT.MSA.Stocking.API.Models;
using EDT.MSA.Stocking.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace EDT.MSA.Stocking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EDT.MSA.Stocking.API", Version = "v1" });
            });
            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));
            // Settings
            services.Configure<StockDatabaseSettings>(
                Configuration.GetSection(nameof(StockDatabaseSettings)));
            services.AddSingleton<IStockDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<StockDatabaseSettings>>().Value);
            // Services
            services.AddSingleton<IStockService, StockService>();
            // Redis
            services.AddRedisClient(Configuration);
            // MsgTracker
            services.AddMsgTracker();
            // EventBus
            services.AddCap(option =>
            {
                option.UseMongoDB(option =>
                {
                    option.DatabaseConnection = Configuration["StockDatabaseSettings:ConnectionString"];
                    option.DatabaseName = Configuration["StockDatabaseSettings:DatabaseName"];
                });
                option.UseKafka(option =>
                {
                    option.Servers = Configuration["KafkaSettings:BootstrapServers"];
                });
            });
            // Consumers
            services.AddTransient<INewOrderSubmittedEventService, NewOrderSubmittedEventService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EDT.MSA.Stocking.API v1"));
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
