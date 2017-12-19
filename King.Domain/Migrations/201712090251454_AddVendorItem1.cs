namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorItem1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.King_HWVendorItem", "success", c => c.Boolean());
            AddColumn("dbo.King_HWVendorItem", "errorMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.King_HWVendorItem", "errorMessage");
            DropColumn("dbo.King_HWVendorItem", "success");
        }
    }
}
