using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Empower.Api.Configuration;
using Empower.Entities;
using Empower.Api.Controllers;
using Empower.Api.Modules;
using Serilog;
using System.Configuration;

namespace Empower.Api {
    public class DependencyConfig {
        public static void Configure(HttpConfiguration config) {

            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterApiControllers(typeof(AuthenticateController).Assembly);

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            Log.Logger = new LoggerConfiguration()
     .WriteTo.File(ConfigurationManager.AppSettings["LogPath"] + "\\log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose, retainedFileCountLimit: 5)
     .CreateLogger();



            var container = builder.Build();

            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

        }
    }
}