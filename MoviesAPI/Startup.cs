using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoviesAPI.DataAccess;
using MoviesAPI.Extensions;
using MoviesAPI.Repositories;
using MoviesAPI.Repositories.Interfaces;
using MoviesAPI.Services;
using MoviesAPI.Services.Interfaces;
using Serilog;

namespace MoviesAPI
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
            services.AddControllers();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });


            services.AddDbContext<MoviesDbContext>(options =>
                options.UseSqlite(@"DataSource=MoviesDb.db;"));

            services.AddScoped<IMoviesRepository, MoviesRepository>();
            services.AddScoped<IRatingsRepository, RatingsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<IMoviesControllerService, MoviesControllerService>();
            services.AddScoped<IRatingsControllerService, RatingsControllerService>();

            services.ConfigureSwagger();

        }

        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env, 
            MoviesDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Exception logging done here
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ConfigureSwagger();


            //Create in-memory database and seed data
            Log.Information("Ensuring database exists, otherwise it will be created");
            dbContext.Database.EnsureCreated();


        }
    }
}
