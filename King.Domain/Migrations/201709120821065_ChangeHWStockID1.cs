namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeHWStockID1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.King_HWStock");
            AlterColumn("dbo.King_HWStock", "F_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.King_HWStock", "F_Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.King_HWStock");
            AlterColumn("dbo.King_HWStock", "F_Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.King_HWStock", "F_Id");
        }
    }
}
