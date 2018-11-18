namespace MedOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MedOrgOnRequestMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachRequests", "MedOrganization_Id", c => c.Int());
            CreateIndex("dbo.AttachRequests", "MedOrganization_Id");
            AddForeignKey("dbo.AttachRequests", "MedOrganization_Id", "dbo.MedOrganizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AttachRequests", "MedOrganization_Id", "dbo.MedOrganizations");
            DropIndex("dbo.AttachRequests", new[] { "MedOrganization_Id" });
            DropColumn("dbo.AttachRequests", "MedOrganization_Id");
        }
    }
}
