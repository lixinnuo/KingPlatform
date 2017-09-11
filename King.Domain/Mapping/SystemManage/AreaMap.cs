using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Domain.Mapping.SystemManage
{
    public class AreaMap : EntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            this.ToTable("King_Area");
            this.HasKey(t => t.F_Id);
        }
    }
}
