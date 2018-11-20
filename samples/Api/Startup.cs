using System.Data.SqlClient;
using Dapper.Logging;
using Dapper.Logging.Configuration;
using Data;
using Data.Products.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            var conStr = Configuration.GetConnectionString("DefaultConnection");

            //EF Core
            services.AddDbContext<EfDataContext>(
                options => options
                    .UseSqlServer(conStr)
                    //.EnableSensitiveDataLogging() //uncomment to show values of the query parameters
                ,ServiceLifetime.Scoped);

            //Dapper + Logging
            services.AddDbConnectionFactory(
                prv => new SqlConnection(conStr), 
                options => options
                    .WithLogLevel(LogLevel.Debug)
                    //.WithSensitiveDataLogging() //uncomment to show values of the query parameters
                ,ServiceLifetime.Scoped);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMediatR(typeof(GetProductsHandler));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
