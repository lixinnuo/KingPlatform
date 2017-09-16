namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHWStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.King_HWStock",
                c => new
                    {
                        F_Id = c.String(nullable: false, maxLength: 128),
                        vendorFactoryCode = c.String(),
                        vendorItemCode = c.String(),
                        customerCode = c.String(),
                        vendorStock = c.String(),
                        vendorLocation = c.String(),
                        stockTime = c.String(),
                        vendorItemRevision = c.String(),
                        goodQuantity = c.Double(nullable: false),
                        inspectQty = c.Double(),
                        faultQty = c.Double(),
                        F_CreateUserId = c.String(),
                        F_CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.F_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.King_HWStock");
        }
    }
}
