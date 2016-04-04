using System.Web;

namespace AgendaProject.Areas.Admin.Models
{
    public class PhotoViewModel
    {
        public int BoardMemberId { get; set; }
        HttpPostedFileBase Photo { get; set; } 
    }
}