using King.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemSecurity
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIPEntity>
    {
        public FilterIPMap()
        {
            this.ToTable("King_FilterIp");
            this.HasKey(t => t.F_Id);
        }
    }
}
