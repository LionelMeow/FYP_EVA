namespace FYP_EVA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventOrganiserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "OrganiserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "OrganiserName");
        }
    }
}
