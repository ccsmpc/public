namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meetingtimeextension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetingTime", "AgendaUrl", c => c.String());
            AddColumn("dbo.MeetingTime", "MinutesUrl", c => c.String());
            AddColumn("dbo.MeetingTime", "RecordingUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetingTime", "RecordingUrl");
            DropColumn("dbo.MeetingTime", "MinutesUrl");
            DropColumn("dbo.MeetingTime", "AgendaUrl");
        }
    }
}
