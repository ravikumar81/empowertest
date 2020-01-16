using Autofac;
using StackExchange.Redis;
using System.Linq;
using System.Reflection;

namespace Empower.Api.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            IDatabase redisCache = GetConnection().GetDatabase();
            builder.RegisterAssemblyTypes(Assembly.Load("Empower.Service"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .WithParameter("azureConnection", System.Configuration.ConfigurationManager.AppSettings["AzureConnection"])
                      .WithParameter("playerStorageContainer", System.Configuration.ConfigurationManager.AppSettings["AzureContainerPlayerDocument"])
                      .WithParameter("playerStorageFolder", System.Configuration.ConfigurationManager.AppSettings["PlayerDocumentServerFolder"])
                      .WithParameter("htmlPrintReportsStorageContainer", System.Configuration.ConfigurationManager.AppSettings["AzureContainerHtmlPrintReportDocument"])
                      .WithParameter("redisCache", redisCache)
                      .InstancePerRequest();

        }

        private static ConnectionMultiplexer GetConnection()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["AzureRedisCacheConnection"];
            return ConnectionMultiplexer.Connect(connectionString);
        }

    }
}