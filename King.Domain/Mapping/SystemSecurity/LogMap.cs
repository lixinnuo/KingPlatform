using King.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemSecurity
{
    public class LogMap : EntityTypeConfiguration<LogEntity>
    {
        public LogMap()
        {
            this.ToTable("King_Log");
            this.HasKey(t => t.F_Id);
        }
    }
}
