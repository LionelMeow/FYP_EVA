namespace FYP_EVA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackID = c.Int(nullable: false, identity: true),
                        EventID = c.Int(nullable: false),
                        OrganiserID = c.Int(nullable: false),
                        VolunteerID = c.Int(nullable: false),
                        Teamwork = c.Int(nullable: false),
                        Communication = c.Int(nullable: false),
                        Initiative = c.Int(nullable: false),
                        Supportiveness = c.Int(nullable: false),
                        Professionalism = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeedbackID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: false)
                .ForeignKey("dbo.Organisers", t => t.OrganiserID, cascadeDelete: false)
                .ForeignKey("dbo.Volunteers", t => t.VolunteerID, cascadeDelete: false)
                .Index(t => t.EventID)
                .Index(t => t.OrganiserID)
                .Index(t => t.VolunteerID);
            
            CreateTable(
                "dbo.Participations",
                c => new
                    {
                        ParticipationID = c.Int(nullable: false, identity: true),
                        EventID = c.Int(nullable: false),
                        VolunteerID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipationID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: false)
                .ForeignKey("dbo.Volunteers", t => t.VolunteerID, cascadeDelete: false)
                .Index(t => t.EventID)
                .Index(t => t.VolunteerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participations", "VolunteerID", "dbo.Volunteers");
            DropForeignKey("dbo.Participations", "EventID", "dbo.Events");
            DropForeignKey("dbo.Feedbacks", "VolunteerID", "dbo.Volunteers");
            DropForeignKey("dbo.Feedbacks", "OrganiserID", "dbo.Organisers");
            DropForeignKey("dbo.Feedbacks", "EventID", "dbo.Events");
            DropIndex("dbo.Participations", new[] { "VolunteerID" });
            DropIndex("dbo.Participations", new[] { "EventID" });
            DropIndex("dbo.Feedbacks", new[] { "VolunteerID" });
            DropIndex("dbo.Feedbacks", new[] { "OrganiserID" });
            DropIndex("dbo.Feedbacks", new[] { "EventID" });
            DropTable("dbo.Participations");
            DropTable("dbo.Feedbacks");
        }
    }
}
