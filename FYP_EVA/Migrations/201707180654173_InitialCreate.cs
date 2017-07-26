namespace FYP_EVA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        EventName = c.String(nullable: false),
                        EventDescription = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        VolunteerLimit = c.Int(nullable: false),
                        EventStatus = c.Int(nullable: false),
                        OrganiserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Organisers", t => t.OrganiserID, cascadeDelete: true)
                .Index(t => t.OrganiserID);
            
            CreateTable(
                "dbo.Organisers",
                c => new
                    {
                        OrganiserID = c.Int(nullable: false, identity: true),
                        OrganiserName = c.String(nullable: false),
                        OrganiserType = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrganiserID);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        VolunteerID = c.Int(nullable: false, identity: true),
                        TPNumber = c.String(nullable: false),
                        VolunteerName = c.String(),
                        Intake = c.String(),
                        PhoneNumber = c.String(),
                        Teamwork = c.Int(nullable: false),
                        Communication = c.Int(nullable: false),
                        Initiative = c.Int(nullable: false),
                        Supportiveness = c.Int(nullable: false),
                        Professionalism = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        ReligionPreferences = c.Int(nullable: false),
                        VolunteerGender = c.Int(nullable: false),
                        VolunteerCountryOfOrigin = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VolunteerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "OrganiserID", "dbo.Organisers");
            DropIndex("dbo.Events", new[] { "OrganiserID" });
            DropTable("dbo.Volunteers");
            DropTable("dbo.Organisers");
            DropTable("dbo.Events");
        }
    }
}
