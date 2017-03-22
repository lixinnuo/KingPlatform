using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemManage
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<RoleAuthorizeEntity>
    {
        public RoleAuthorizeMap()
        {
            this.ToTable("King_RoleAuthorize");
            this.HasKey(t => t.F_Id);
        }
    }
}
