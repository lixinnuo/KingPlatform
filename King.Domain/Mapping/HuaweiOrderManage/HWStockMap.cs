using King.Domain.Entity.HuaweiOrderManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Domain.Mapping.HuaweiOrderManage
{
    public class HWStockMap : EntityTypeConfiguration<HWStockEntity>
    {
        public HWStockMap()
        {
            this.ToTable("King_HWStock");
            this.HasKey(t => t.F_Id);
        }
    }
}
