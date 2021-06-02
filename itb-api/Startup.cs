using System;
using itb_api.Models.Configurtations;
using itb_api.Services.Chat;
using itb_api.Services.Database;
using itb_api.Services.Instagram;
using itb_api.Services.Statistic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace itb_api
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
            services.Configure<DatabaseConfiguration>(Configuration.GetSection("Database"));
            services.Configure<InstagramConfiguration>(Configuration.GetSection("Instagram"));

            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IInstagramService, InstagramService>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IStatisticService, StatisticService>();

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}