namespace MedOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableHandleDateMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AttachRequests", "HandleDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AttachRequests", "HandleDate", c => c.DateTime(nullable: false));
        }
    }
}
