namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.King_HWVendorItem",
                c => new
                    {
                        F_Id = c.String(nullable: false, maxLength: 128),
                        vendorItemCode = c.String(),
                        vendorProductModel = c.String(),
                        vendorItemDesc = c.String(),
                        itemCategory = c.String(),
                        customerVendorCode = c.String(),
                        customerItemCode = c.String(),
                        customerProductModel = c.String(),
                        unitOfMeasure = c.String(),
                        inventoryType = c.String(),
                        goodPercent = c.Double(),
                        leadTime = c.Double(),
                        lifeCycleStatus = c.String(),
                        F_CreateUserId = c.String(),
                        F_CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.F_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.King_HWVendorItem");
        }
    }
}
