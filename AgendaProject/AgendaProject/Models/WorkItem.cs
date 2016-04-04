using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using AgendaProject.DAL;
using AgendaProject.Models.Lib;

namespace AgendaProject.Models
{
    public partial class WorkItem
    {
        public int Id { get; set; }

        [Display(Name = "Date Recieved")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Recieved { get; set; }

        [Display(Name="File Number")]
        public string Filenumber { get; set; }
        
        public string Petitioner { get; set; }

        [Display(Name = "Petition Type")]
        public string PetitionType { get; set; }

        [Display(Name = "Fee Paid")]
        public bool FeePaid { get; set; }

        [Display(Name = "Construction Cost")]
        public double ConstructionCost { get; set; }

        [Display(Name = "Scope of Work")]
        public string ScopeOfWork { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Display(Name = "Is complete?")]
        public bool IsComplete { get; set; }

        [Display(Name = "Is ready for review?")]
        public bool ReadyForReview { get; set; }

        [Display(Name = "Was withdrawn")]
        public bool WasWithdrawn { get; set; }

        AgendaContext db = new AgendaContext();

    }

    public partial class WorkItem
    {
        public WorkItem()
        {
            AgendaItems = new List<AgendaItem>();
        }

        // Navigation Properties
        public virtual ICollection<AgendaItem> AgendaItems { get; set; } 
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<StaffDecision> StaffDecisions { get; set; }
    }

    public partial class WorkItem
    {
        public void AddWorkItem()
        {
            
            db.WorkItems.Add(this);
            db.SaveChanges();
        }
        public void AssignTo(string reason) { }

        public bool ReviewComplete()
        {
            var agendaItem = new AgendaItem();
            var address = db.Addresses.FirstOrDefault(s => s.WorkItemId == this.Id);
            var staffDecision = db.StaffDecisions.FirstOrDefault(s => s.WorkItemId == this.Id);

            string streetAddress = "";
            string decision = "";

            if (address != null)
            {
                streetAddress = address.StreetAddress + " | ";
                decision = this.ScopeOfWork;
            }

            if (staffDecision != null)
            {
                decision = "Staff " + staffDecision.Decision + " - " + this.ScopeOfWork;
            }

            agendaItem.Description = "Petition of " + this.Petitioner + " | " + this.Filenumber + " | " +
                                     streetAddress + decision;


            return agendaItem.CreateAgendaItemFromWorkItem(this.Id);

        }

        public void SaveWorkItem(string oldOwner)
        {
            
            if (oldOwner != this.AssignedTo)
            
            {
                    var sender = MailHelper.GetEmailAddress(oldOwner);
                    var file = this.Filenumber;
                    var assignedTo = MailHelper.GetEmailAddress(this.AssignedTo);
                    var subject = "File number " + file + " has been assigned to you.";

                    var body = "";

                    if (this.ReadyForReview)
                    {
                        body = "File number " + file + " is ready for review.";
                    }
                    else
                    {
                        body = "File number " + file +
                               " is assigned to you.  Please review the submittal and perform all work needed, and when done, mark as ready for review and assign to the department head.";}

                    MailHelper.SendMailMessage(assignedTo, sender, subject, body);
                }

            
            db.Entry(this).State = EntityState.Modified;
            db.SaveChanges();
        }

    }

    public partial class WorkItem
    {
        
        public List<WorkItem> GetActiveWorkItems()
        {
            var workItems = db.WorkItems.Where(s => s.IsComplete == false).Where(s => s.WasWithdrawn == false);
            return workItems.ToList();
        }

        public WorkItem FindById(int id)
        {
            var workItem = db.WorkItems.Find(id);
            return workItem;
        }
        
    }


}

