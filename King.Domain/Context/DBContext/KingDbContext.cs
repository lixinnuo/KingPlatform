using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace King.Domain.Context
{
    public class KingDbContext : DbContext
    {
        public KingDbContext()
            : base("KingDbContext")
        {
            //Database.SetInitializer<KingDbContext>(null);
            /*this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;*/
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("Basic.Data.DLL", "King.Mapping.DLL").Replace("file:///", "");
            //Assembly asm = Assembly.LoadFile(assembleFileName);
            //var typesToRegister = asm.GetTypes()
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
