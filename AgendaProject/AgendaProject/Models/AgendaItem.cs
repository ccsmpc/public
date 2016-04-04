using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class AgendaItem
    {
        AgendaContext db = new AgendaContext();
        public int Id { get; set; }

        public string Description { get; set; }
        public string Minutes { get; set; }

        public int? Prev { get; set; }
        public int? Nxt { get; set; }
    }

    public partial class AgendaItem
    {
        // CreateAgendaItemFromWorkItem moved to its own file under AgendaItem Folder on work complete.   
        // MoveUp moved to its own file under AgendaItem Folder on work complete.

        public int MoveDown()
        {
            var agendaItem = db.AgendaItems.Find(Nxt);

            return agendaItem.MoveUp();
        }

        public void AddAttachment(Attachment attachment)
        {

            var agendaItem = db.AgendaItems.Find(attachment.AgendaItemId);
            agendaItem.Attachments.Add(attachment);
            db.SaveChanges();
        }

        public void RemoveAttachment(Attachment attachment)
        {
            var agendaItem = db.AgendaItems.Find(attachment.AgendaItemId);
            agendaItem.Attachments.Remove(attachment);
            db.Attachments.Remove(db.Attachments.Find(attachment.Id));
            db.SaveChanges();
        }


        public int EditAgendaItem()
        {
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
            var sectionHeading = db.SectionHeadings.Find(this.SectionHeadingId);
            return sectionHeading.AgendaId;
        }

        public int DeleteAgendaItem(int sid)
        {
            var agendaItem = db.AgendaItems.Find(Id);
            var sectionHeading = db.SectionHeadings.Find(sid);

            if (agendaItem.Prev == null && agendaItem.Nxt == null)
            {
                // there is one item, remove it.
                sectionHeading.AgendaItems.Remove(agendaItem);
                db.AgendaItems.Remove(agendaItem);
                db.SaveChanges();
            }
            else if (agendaItem.Prev == null && agendaItem.Nxt != null)
            {
                // top item
                var nextNode = db.AgendaItems.Find(agendaItem.Nxt);
                nextNode.Prev = agendaItem.Prev;

                sectionHeading.AgendaItems.Remove(agendaItem);
                db.AgendaItems.Remove(agendaItem);
                db.SaveChanges();
            }
            else if (agendaItem.Prev != null && agendaItem.Nxt == null)
            {
                //  bottom item
                var prevNode = db.AgendaItems.Find(agendaItem.Prev);
                prevNode.Nxt = agendaItem.Nxt;

                sectionHeading.AgendaItems.Remove(agendaItem);
                db.AgendaItems.Remove(agendaItem);
                db.SaveChanges();
            }
            else
            {
                // general case:  nodes before and after
                var prevNode = db.AgendaItems.Find(agendaItem.Prev);
                var nextNode = db.AgendaItems.Find(agendaItem.Nxt);

                prevNode.Nxt = nextNode.Id;
                nextNode.Prev = prevNode.Id;

                sectionHeading.AgendaItems.Remove(agendaItem);
                db.AgendaItems.Remove(agendaItem);
                db.SaveChanges();
            }

            return sectionHeading.AgendaId;
        }
        public void AddMinutes() { }
        public void EditMintues() { }

    }

    public partial class AgendaItem
    {
        // Data Access

        public AgendaItem GetAgendaItem(int id)
        {

            var agendaItem = db.AgendaItems.Find(id);
            return agendaItem;
        }
    }

    public partial class AgendaItem
    {
        // Navigation Properties

        public int? SectionHeadingId { get; set; }


        public int? WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public virtual WorkItem WorkItem { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<VoteItem> VoteItems { get; set; }
    }
}