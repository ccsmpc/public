namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class handleWorkItemAgendaItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkItem", "AgendaItem_Id", "dbo.AgendaItem");
            DropIndex("dbo.WorkItem", new[] { "AgendaItem_Id" });
            CreateIndex("dbo.AgendaItem", "WorkItemId");
            AddForeignKey("dbo.AgendaItem", "WorkItemId", "dbo.WorkItem", "Id");
            DropColumn("dbo.WorkItem", "AgendaItem_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkItem", "AgendaItem_Id", c => c.Int());
            DropForeignKey("dbo.AgendaItem", "WorkItemId", "dbo.WorkItem");
            DropIndex("dbo.AgendaItem", new[] { "WorkItemId" });
            CreateIndex("dbo.WorkItem", "AgendaItem_Id");
            AddForeignKey("dbo.WorkItem", "AgendaItem_Id", "dbo.AgendaItem", "Id");
        }
    }
}
