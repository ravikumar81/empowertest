using Empower.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;

namespace Empower.Entities
{
    public class EmpowerContext : DbContext
    {
        public EmpowerContext()
            : base("Name=EmpowerContext")
        {

        }       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EmpowerContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();           

        }

    }
}