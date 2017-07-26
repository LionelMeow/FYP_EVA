namespace FYP_EVA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventMessage", c => c.String());
            AddColumn("dbo.Organisers", "Email", c => c.String());
            AddColumn("dbo.Volunteers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "Email");
            DropColumn("dbo.Organisers", "Email");
            DropColumn("dbo.Events", "EventMessage");
        }
    }
}
