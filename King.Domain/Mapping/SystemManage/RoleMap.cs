using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Domain.Mapping.SystemManage
{
    public class RoleMap : EntityTypeConfiguration<RoleEntity>
    {
        public RoleMap()
        {
            this.ToTable("King_Role");
            this.HasKey(t => t.F_Id);
        }
    }
}
