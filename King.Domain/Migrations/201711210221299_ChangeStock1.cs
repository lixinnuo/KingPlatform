namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStock1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.King_HWStock", "goodQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.King_HWStock", "inspectQty", c => c.Double());
            AddColumn("dbo.King_HWStock", "faultQty", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.King_HWStock", "faultQty");
            DropColumn("dbo.King_HWStock", "inspectQty");
            DropColumn("dbo.King_HWStock", "goodQuantity");
        }
    }
}
