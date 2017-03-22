using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemManage
{
    public class ModuleMap : EntityTypeConfiguration<ModuleEntity>
    {
        public ModuleMap()
        {
            this.ToTable("King_Module");
            this.HasKey(t => t.F_Id);
        }
    }
}
