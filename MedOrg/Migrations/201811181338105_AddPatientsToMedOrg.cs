namespace MedOrg.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPatientsToMedOrg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "MedOrganization_Id", c => c.Int());
            CreateIndex("dbo.Patients", "MedOrganization_Id");
            AddForeignKey("dbo.Patients", "MedOrganization_Id", "dbo.MedOrganizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "MedOrganization_Id", "dbo.MedOrganizations");
            DropIndex("dbo.Patients", new[] { "MedOrganization_Id" });
            DropColumn("dbo.Patients", "MedOrganization_Id");
        }
    }
}
