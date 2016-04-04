namespace AgendaProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class voteitemupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteItem", "mMotion", c => c.String());
            AddColumn("dbo.VoteItem", "mMover", c => c.String());
            AddColumn("dbo.VoteItem", "mSecond", c => c.String());
            AddColumn("dbo.VoteItem", "aMotion", c => c.String());
            AddColumn("dbo.VoteItem", "mOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "pAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "pMover", c => c.String());
            AddColumn("dbo.VoteItem", "pSecond", c => c.String());
            AddColumn("dbo.VoteItem", "apAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "pOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "sAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "sMover", c => c.String());
            AddColumn("dbo.VoteItem", "sSecond", c => c.String());
            AddColumn("dbo.VoteItem", "sOutcome", c => c.String());
            DropColumn("dbo.VoteItem", "OriginalMotion");
            DropColumn("dbo.VoteItem", "MotionMover");
            DropColumn("dbo.VoteItem", "MotionSecond");
            DropColumn("dbo.VoteItem", "AmendedMotion");
            DropColumn("dbo.VoteItem", "MotionOutcome");
            DropColumn("dbo.VoteItem", "PrimaryAmendment");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentMover");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentSecond");
            DropColumn("dbo.VoteItem", "AmendedPrimaryAmendment");
            DropColumn("dbo.VoteItem", "PrimaryAmendmentOutcome");
            DropColumn("dbo.VoteItem", "SecondaryAmendment");
            DropColumn("dbo.VoteItem", "SecondaryAmendmentMover");
            DropColumn("dbo.VoteItem", "SecondaryAmendmentSecond");
            DropColumn("dbo.VoteItem", "SecondaryAmendmentOutcome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VoteItem", "SecondaryAmendmentOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendmentSecond", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendmentMover", c => c.String());
            AddColumn("dbo.VoteItem", "SecondaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "AmendedPrimaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentSecond", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendmentMover", c => c.String());
            AddColumn("dbo.VoteItem", "PrimaryAmendment", c => c.String());
            AddColumn("dbo.VoteItem", "MotionOutcome", c => c.String());
            AddColumn("dbo.VoteItem", "AmendedMotion", c => c.String());
            AddColumn("dbo.VoteItem", "MotionSecond", c => c.String());
            AddColumn("dbo.VoteItem", "MotionMover", c => c.String());
            AddColumn("dbo.VoteItem", "OriginalMotion", c => c.String());
            DropColumn("dbo.VoteItem", "sOutcome");
            DropColumn("dbo.VoteItem", "sSecond");
            DropColumn("dbo.VoteItem", "sMover");
            DropColumn("dbo.VoteItem", "sAmendment");
            DropColumn("dbo.VoteItem", "pOutcome");
            DropColumn("dbo.VoteItem", "apAmendment");
            DropColumn("dbo.VoteItem", "pSecond");
            DropColumn("dbo.VoteItem", "pMover");
            DropColumn("dbo.VoteItem", "pAmendment");
            DropColumn("dbo.VoteItem", "mOutcome");
            DropColumn("dbo.VoteItem", "aMotion");
            DropColumn("dbo.VoteItem", "mSecond");
            DropColumn("dbo.VoteItem", "mMover");
            DropColumn("dbo.VoteItem", "mMotion");
        }
    }
}
