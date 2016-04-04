using System.ComponentModel.DataAnnotations;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class Address
    {
        public int Id { get; set; }

        [Display(Name="Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Pin { get; set; }
    }

    public partial class Address
    {
        public void AddAddress()
        {
            AgendaContext db = new AgendaContext();
            db.Addresses.Add(this);
            db.SaveChanges();
        }
    }

    public partial class Address
    {
        // Navigation properties
        public int WorkItemId { get; set; }
    }
}