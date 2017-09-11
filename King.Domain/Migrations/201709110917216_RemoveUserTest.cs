namespace King.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserTest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.King_User", "F_UserTest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.King_User", "F_UserTest", c => c.String());
        }
    }
}
