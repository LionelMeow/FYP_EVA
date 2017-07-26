namespace FYP_EVA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUpdateList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "Event_EventID", c => c.Int());
            CreateIndex("dbo.Volunteers", "Event_EventID");
            AddForeignKey("dbo.Volunteers", "Event_EventID", "dbo.Events", "EventID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "Event_EventID", "dbo.Events");
            DropIndex("dbo.Volunteers", new[] { "Event_EventID" });
            DropColumn("dbo.Volunteers", "Event_EventID");
        }
    }
}
