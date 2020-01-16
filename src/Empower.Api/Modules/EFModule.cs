using Autofac;
using Empower.Entities;
using Empower.Repository;
using System.Data.Entity;

namespace Empower.Api.Modules
{

    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(EmpowerContext)).As(typeof(DbContext)).InstancePerRequest();


            builder.RegisterType<UnitOfWorkManager>()
                .As<IManageUnitsOfWork>()
                .InstancePerRequest();
        }

    }
}