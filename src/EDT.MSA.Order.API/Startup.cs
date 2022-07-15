using EDT.MSA.Ordering.API.Models;
using EDT.MSA.Ordering.API.Repositories;
using EDT.MSA.Ordering.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace EDT.MSA.Ordering.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EDT.MSA.Order.API", Version = "v1" });
            });
            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));
            // Settings
            services.Configure<OrderDatabaseSettings>(
                Configuration.GetSection(nameof(OrderDatabaseSettings)));
            services.AddSingleton<IOrderDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<OrderDatabaseSettings>>().Value);
            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            // EventBus
            services.AddCap(option =>
            {
                option.UseMongoDB(option =>
                {
                    option.DatabaseConnection = Configuration["OrderDatabaseSettings:ConnectionString"];
                    option.DatabaseName = Configuration["OrderDatabaseSettings:DatabaseName"];
                });
                option.UseKafka(option =>
                {
                    option.Servers = Configuration["KafkaSettings:BootstrapServers"];
                });
            });
            // Consumer
            services.AddTransient<IProductStockDeductedEventService, ProductStockDeductedEventService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EDT.MSA.Order.API v1"));
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
