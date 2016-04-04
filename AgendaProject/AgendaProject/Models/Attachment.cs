using System.Data.Entity;
using System.Web;
using AgendaProject.Models.Lib;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class Attachment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        HttpPostedFileBase MyFile { get; set; }
        AgendaContext db = new AgendaContext();
    }

    public partial class Attachment
    {
        // Navigation Properties
        public int? AgendaItemId { get; set; }
        public int? WorkItemId { get; set; }
    }

    public partial class Attachment
    {
        public void AddAttachment()
        {
            db.Attachments.Add(this);
            db.SaveChanges();
        }

        public void AddWorkItemAttachment(string year, string fileNumber, HttpPostedFileBase file)
        {
            this.FilePath = FileFolders.SaveWorkItemAttachment(year, fileNumber, file);
            
            db.Attachments.Add(this);
            db.SaveChanges();
        }

        public void AddAgendaItemAttachment(string year, string monthDay, HttpPostedFileBase file)
        {
            this.FilePath = FileFolders.SaveAgendaItemAttachment(year, monthDay, file);
        }

        public Attachment GetAttachment(int id)
        {
            return db.Attachments.Find(id);
        }

        public void EditAttachmentDescription(Attachment attachment)
        {
            var editAttachment = db.Attachments.Find(attachment.Id);
            editAttachment.Description = attachment.Description;
            db.SaveChanges();
        }

        public void DeleteAttachment(int id, int workItemId)
        {
            var workItem = new WorkItem();
            workItem = workItem.FindById(workItemId);
            var attachment = new Attachment();
            attachment = attachment.GetAttachment(id);
            workItem.Attachments.Remove(attachment);
            db.Entry(attachment).State=EntityState.Deleted;
            

            FileFolders.DeleteAttachment(attachment.FilePath);
            db.SaveChanges();

            
        }
    }
}