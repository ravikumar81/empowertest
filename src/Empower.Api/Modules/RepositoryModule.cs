using Autofac;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Empower.Api.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Empower.Repository"))
                   .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Factory"))
                   .AsImplementedInterfaces()
                   .WithParameter("sqlConnection", ConfigurationManager.AppSettings["EmpowerSQLConnection"])
                  .InstancePerRequest();
        }
    }
}