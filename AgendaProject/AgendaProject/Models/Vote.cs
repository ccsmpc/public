using System;
using System.Linq;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class Vote
    {
        public int Id { get; set; }
        public string Phase { get; set; }
        public int BoardMemberId { get; set; }
        public string VoteCast { get; set; }

        AgendaContext db = new AgendaContext();
    }

    public partial class Vote
    {
        public void RecordVote(int voteItemId, int boardMemberId, string voteCast, string phase)
        {
            var vote = new Vote();
            vote.VoteItemId = voteItemId;
            vote.BoardMemberId = boardMemberId;
            vote.VoteCast = voteCast;
            vote.Phase = phase;
            db.Votes.Add(vote);
            db.SaveChanges();
        }

        public string TallyVote(int voteItemId, int membersPresent)
        {
            int yesCount = 0;
            int noCount = 0;
            string votingYes = "";
            string votingNo = "";
            string voteResult = "";

            var boardMember = new BoardMember();
            

            var votes = db.Votes.Where(s => s.VoteItemId == voteItemId);
            foreach (var vote in votes)
            {
                switch (vote.VoteCast)
                {
                    case "Yes":
                        yesCount++;
                        votingYes = votingYes + boardMember.GetBoardMemberNameFromId(vote.BoardMemberId) + ", ";
                        break;
                    case "No":
                        noCount++;
                        votingNo = votingNo + boardMember.GetBoardMemberNameFromId(vote.BoardMemberId) + ", ";
                        break;
                    default:
                        break;
                }
            }


            if (String.IsNullOrEmpty(votingYes))
            {
                votingYes = "No one";
            } else
            {
                votingYes = votingYes.Substring(0, votingYes.Length - 2);
            }

            if (String.IsNullOrEmpty(votingNo))
            {
                votingNo = "No one";
            } else
            {
                votingNo = votingNo.Substring(0, votingNo.Length - 2);
            }

            if (yesCount > noCount)
            {
                voteResult = "This motion has passed.";
            }
            else
            {
                voteResult = "This motion has failed.";
            }

            voteResult = voteResult + " <div class='row'><div class='left-column'>Voting yes: " + votingYes + ".</div><div class='right-column'>Voting no: " + votingNo + ". </div></div>";

            return voteResult;

        }
    }

    public partial class Vote
    {
        // Navigation Properties

        public int VoteItemId { get; set; }
    }
}