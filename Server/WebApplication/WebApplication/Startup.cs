using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MessageBus.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RiskFirst.Hateoas;
using WebApplication.Data;
using WebApplication.Data.Repository;
using WebApplication.Domain.Services;
using WebApplication.Presentation.Models;

namespace WebApplication
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
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; })
                .AddXmlSerializerFormatters();
            services.AddDbContext<DatabaseApplicationContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("Default")), ServiceLifetime.Singleton);
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IActorService, ActorService>();
            services.AddSingleton<IRepository<Data.Entities.Actor>, ActorRepository>();
            services.AddSingleton<IRepository<Data.Entities.Movie>, MovieRepository>();
            services.AddSingleton(new MessageBus.MessageBus(Configuration.GetConnectionString("MessageBroker")));
            services.AddLinks(config =>
            {
                config.AddPolicy<Actor>(policy => policy
                    .RequireRoutedLink("get", "GetActorById", actor => new { id = actor.Id })
                    .RequireRoutedLink("update", "PutActor", actor => new { id = actor.Id })
                    .RequireRoutedLink("delete", "DeleteActor", actor => new { id = actor.Id }));
                config.AddPolicy<Movie>(policy => policy
                    .RequireRoutedLink("get", "GetMovieById", movie => new {id = movie.Id})
                    .RequireRoutedLink("update", "PutMovie", movie => new {id = movie.Id})
                    .RequireRoutedLink("delete", "DeleteMovie", movie => new {id = movie.Id})
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            NotifyUp(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void NotifyUp(IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetService<ILogger>();
            var messageBus = app.ApplicationServices.GetService<MessageBus.MessageBus>();
            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            var addr = serverAddressesFeature.Addresses.First();
            try
            {
                // Get the local computer host name.
                string hostName = Dns.GetHostName();
                hostName = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                var port = new string(addr.Split(":").Last().Where(Char.IsDigit).ToArray());
                string fullPath = $"http://{hostName}:{port}";
                messageBus.Publish("server", new ServerUpEvent
                {
                    Url = fullPath
                });
            }
            catch (SocketException e)
            {
                logger.LogError("Unable to send notification to LoadBalancer", e);
            }
        }
    }
}