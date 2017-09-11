using King.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace King.Mapping.SystemSecurity
{
    public class DbBackupMap : EntityTypeConfiguration<DbBackupEntity>
    {
        public DbBackupMap()
        {
            this.ToTable("King_DbBackup");
            this.HasKey(t => t.F_Id);
        }
    }
}
