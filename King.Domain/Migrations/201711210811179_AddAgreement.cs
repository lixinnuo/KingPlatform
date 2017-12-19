namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgreement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.King_HWStock", "agreementStocking", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.King_HWStock", "agreementStocking");
        }
    }
}
