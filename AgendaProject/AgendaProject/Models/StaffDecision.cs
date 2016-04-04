using System.ComponentModel.DataAnnotations;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class StaffDecision
    {
        public int Id { get; set; }
        public string Decision { get; set; }
        public string Conditions { get; set; }

        [Display(Name = "Decision Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}", ApplyFormatInEditMode = true)]
        public string DecisionDate { get; set; }
    }

    public partial class StaffDecision
    {
        public void AddStaffDecision()
        {
            AgendaContext db = new AgendaContext();
            db.StaffDecisions.Add(this);
            db.SaveChanges();
        }
    }

    public partial class StaffDecision
    {
        // Navigation Properties
        public int WorkItemId { get; set; }
    }

}