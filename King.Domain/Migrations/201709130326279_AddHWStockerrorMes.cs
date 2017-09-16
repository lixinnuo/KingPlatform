namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHWStockerrorMes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.King_HWStock", "success", c => c.Boolean());
            AddColumn("dbo.King_HWStock", "errorMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.King_HWStock", "errorMessage");
            DropColumn("dbo.King_HWStock", "success");
        }
    }
}
