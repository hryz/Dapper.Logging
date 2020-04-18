using Dapper.Logging;
using Dapper.Logging.Configuration;
using Data;
using Data.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace ApiV3
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

            services.AddMediatR(typeof(GetProductList).Assembly);
            
            services.AddSwaggerGen(x => 
                x.SwaggerDoc("v1", new OpenApiInfo{Title = "products", Version = "v1"}));
            
            var conStr = Configuration.GetConnectionString("DefaultConnection");

            //EF Core
            services.AddDbContext<EfDataContext>(
                options => options
                    .UseNpgsql(conStr)
                .EnableSensitiveDataLogging() //uncomment to show values of the query parameters
                ,ServiceLifetime.Scoped);

            //Dapper + Logging
            services.AddDbConnectionFactory(
                prv => new NpgsqlConnection(conStr), 
                options => options
                    .WithLogLevel(LogLevel.Information)
                .WithSensitiveDataLogging() //uncomment to show values of the query parameters
                ,ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
