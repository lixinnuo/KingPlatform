using King.Domain.Entity.HuaweiOrderManage;
using System.Data.Entity.ModelConfiguration;

namespace King.Domain.Mapping.HuaweiOrderManage
{
    public class HWVendorItemMap : EntityTypeConfiguration<HWVendorItemEntity>
    {
        public HWVendorItemMap()
        {
            this.ToTable("King_HWVendorItem");
            this.HasKey(t => t.F_Id);
        }
    }
}
