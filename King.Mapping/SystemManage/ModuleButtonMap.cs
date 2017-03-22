using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemManage
{
    public class ModuleButtonMap : EntityTypeConfiguration<ModuleButtonEntity>
    {
        public ModuleButtonMap()
        {
            this.ToTable("King_ModuleButton");
            this.HasKey(t => t.F_Id);
        }
    }
}
