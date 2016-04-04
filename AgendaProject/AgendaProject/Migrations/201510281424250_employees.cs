namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Title = c.String(maxLength: 70),
                        Department = c.String(maxLength: 30),
                        WorkPhone = c.String(maxLength: 15),
                        WorkEmail = c.String(maxLength: 70),
                        SupervisorId = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        Duties = c.String(),
                    })
                .PrimaryKey(t => t.StaffId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employee");
        }
    }
}
