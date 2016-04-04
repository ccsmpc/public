namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class votechanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vote", "Phase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vote", "Phase");
        }
    }
}
