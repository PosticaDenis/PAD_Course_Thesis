using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Data;
using WebApplication.Data.Repository;
using WebApplication.Domain.Services;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation
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
            services.AddMvc();
            services.AddDbContext<DatabaseApplicationContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("Default")), ServiceLifetime.Singleton);
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            
            Mapper.Initialize(RegisterMapping);
        }

        public void RegisterMapping(IMapperConfigurationExpression conf)
        {
            conf.CreateMap<Movie, Data.Entities.Movie>();
        }
    }
}