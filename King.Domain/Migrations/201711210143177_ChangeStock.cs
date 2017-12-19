namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStock : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.King_HWStock", "goodQuantity");
            DropColumn("dbo.King_HWStock", "inspectQty");
            DropColumn("dbo.King_HWStock", "faultQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.King_HWStock", "faultQty", c => c.Double());
            AddColumn("dbo.King_HWStock", "inspectQty", c => c.Double());
            AddColumn("dbo.King_HWStock", "goodQuantity", c => c.Double(nullable: false));
        }
    }
}
