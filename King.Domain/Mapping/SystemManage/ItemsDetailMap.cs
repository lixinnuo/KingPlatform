using King.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Domain.Mapping.SystemManage
{
    public class ItemsDetailMap : EntityTypeConfiguration<ItemsDetailEntity>
    {
        public ItemsDetailMap()
        {
            this.ToTable("King_ItemsDetail");
            this.HasKey(t => t.F_Id);
        }
    }
}
