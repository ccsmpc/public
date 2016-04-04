namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class voteitemchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteItem", "OriginalMotion", c => c.String());
            AddColumn("dbo.VoteItem", "MotionMover", c => c.String());
            AddColumn("dbo.VoteItem", "MotionSecond", c => c.String());
            AddColumn("dbo.VoteItem", "AmendedMotion", c => c.String());
            AddColumn("dbo.VoteItem", "MotionOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentMover", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentSecond", c => c.String());
            AddColumn("dbo.VoteItem", "AmendedPrimaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendmentMover", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendmentSecond", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendmentOutcome", c => c.String());
            DropColumn("dbo.VoteItem", "Motion");
            DropColumn("dbo.VoteItem", "Mover");
            DropColumn("dbo.VoteItem", "Seconded");
            DropColumn("dbo.VoteItem", "Outcome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VoteItem", "Outcome", c => c.String());
            AddColumn("dbo.VoteItem", "Seconded", c => c.String());
            AddColumn("dbo.VoteItem", "Mover", c => c.String());
            AddColumn("dbo.VoteItem", "Motion", c => c.String());
            DropColumn("dbo.VoteItem", "SecondaryAmendmentOutcome");
            DropColumn("dbo.VoteItem", "SecondaryAmendmentSecond");
            DropColumn("dbo.VoteItem", "SecondaryAmendmentMover");
            DropColumn("dbo.VoteItem", "SecondaryAmendment");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentOutcome");
            DropColumn("dbo.VoteItem", "AmendedPrimaryAmendment");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentSecond");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentMover");
            DropColumn("dbo.VoteItem", "PrimaryAmendment");
            DropColumn("dbo.VoteItem", "MotionOutcome");
            DropColumn("dbo.VoteItem", "AmendedMotion");
            DropColumn("dbo.VoteItem", "MotionSecond");
            DropColumn("dbo.VoteItem", "MotionMover");
            DropColumn("dbo.VoteItem", "OriginalMotion");
        }
    }
}
