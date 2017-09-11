namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.King_User", "F_UserTest", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.King_User", "F_UserTest");
        }
    }
}
