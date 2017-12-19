namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorItem2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.King_HWVendorItem", "leadTime", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.King_HWVendorItem", "leadTime", c => c.Double());
        }
    }
}
