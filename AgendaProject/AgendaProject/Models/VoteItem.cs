using System.Collections.Generic;
using System.Linq;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class VoteItem
    {
        public int Id { get; set; }
        public string mMotion { get; set; }
        public string mMover { get; set; }
        public string mSecond { get; set; }
        public string aMotion { get; set; }
        public string mOutcome { get; set; }                   // Possible choices:  Pass, Fail

        public string pAmendment { get; set; }
        public string pMover { get; set; }
        public string pSecond { get; set; }
        public string apAmendment { get; set; }
        public string pOutcome { get; set; }
        // Possible choices:  Pass, Fail

        public string sAmendment { get; set; }
        public string sMover { get; set; }
        public string sSecond { get; set; }
        public string sOutcome { get; set; }       // Possible choices:  Pass, Fail



        AgendaContext db = new AgendaContext();
    }



    public partial class VoteItem
    {
        public VoteItem SetDefault(int id)
        {
            var agendaItem = db.AgendaItems.Find(id);
            //  split the info out of the agendaItem for field replacement.
            string[] petition = agendaItem.Description.Split('|');
            string petitioner = petition[0].Replace("Petition of ", "");
            string fileNumber = petition[1];
            string address = petition[2];
            string scopeOfWork = petition[3];

            var voteItem = new VoteItem();
            voteItem.mMotion =
                db.DropDowns.Where(s => s.DropDownName == "MotionSelector")
                    .Where(s => s.Text == "Approve - As Submitted")
                    .Select(s => s.Value)
                    .First().Replace("NAME", petitioner);
                   

            agendaItem.VoteItems.Add(voteItem);
            db.SaveChanges();

            return voteItem;
        }

        public int Save(VoteItem item)
        {
            var voteItem = db.VoteItems.Find(item.Id);
            voteItem = item;
            var agendaItem = db.AgendaItems.Find(voteItem.AgendaItemId);
            var sectionHeading = db.SectionHeadings.Find(agendaItem.SectionHeadingId);


            db.SaveChanges();

            return sectionHeading.AgendaId;

        }

        public int RecordResult(int voteItemId, int votePhase, string result)
        {
            var voteItem = db.VoteItems.Find(voteItemId);

            var agendaItem = db.AgendaItems.Find(voteItem.AgendaItemId);
            var sectionHeading = db.SectionHeadings.Find(agendaItem.SectionHeadingId);

            switch (votePhase)
            {
                case 3:
                    voteItem.sOutcome = result;
                    break;
                case 2:
                    voteItem.pOutcome = result;
                    break;
                case 1:
                    voteItem.mOutcome = result;
                    break;
                default:
                    break;
            }


            db.SaveChanges();

            return sectionHeading.AgendaId;

        }

        public VoteItem GetVoteItemById(int voteItemId)
        {
            var voteItem = db.VoteItems.Find(voteItemId);
            return voteItem;
        }
    }

    public partial class VoteItem
    {
        // Navigation Properties 
        public int AgendaItemId { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }

}