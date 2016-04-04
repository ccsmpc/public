using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class BoardMember
    {
        [Key]
        public int BoardMemeberId { get; set; }

        public int BoardId { get; set; }

        public string Honorific { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Suffix")]
        public string GenerationalSuffix { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }

        public string PhotoName { get; set; }

        [Display(Name = "Board Position")]
        public string Position { get; set; }

        [Display(Name = "Affiliated with")]
        public string Organization { get; set; }

        [Display(Name = "Appointed By")]
        public string AppointedBy { get; set; }


        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentStartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AppointmentEndDate { get; set; }

        [Display(Name = "Ex-Officio")]
        public Boolean IsExOfficio { get; set; }

        AgendaContext db = new AgendaContext();
    }

    public partial class BoardMember
    {
        public IQueryable GetBoardMembersForDropdown(int boardId)
        {
            var boardMembers =
                db.BoardMembers.Where(s => s.BoardId == boardId).Where(s => s.AppointmentEndDate > DateTime.Today).Where(s => s.AppointmentStartDate < DateTime.Today)
                    .Select(
                        s =>
                            new SelectListItem
                            {
                                Text = s.FirstName + " " + s.LastName,
                                Value = s.BoardMemeberId.ToString()
                            });

            return boardMembers;
        }

        public string GetBoardMemberKey(string fullName, int boardId)
        {
            string[] name = fullName.Split(' ');

            var boardMembers =
                db.BoardMembers.Where(s => s.BoardId == boardId).Where(s => s.AppointmentEndDate > DateTime.Today).Where(s => s.AppointmentStartDate < DateTime.Today)
                    .Select(
                        s =>
                            new SelectListItem
                            {
                                Text = s.FirstName + " " + s.LastName,
                                Value = s.BoardMemeberId.ToString()
                            });

            string id = boardMembers.Where(s => s.Text == fullName).Select(s => s.Value).First();


            return id;
        }

        public string GetBoardMemberNameFromId(int boardMemberId)
        {
            var boardMember = db.BoardMembers.Find(boardMemberId);
            return boardMember.FirstName + " " + boardMember.LastName;
        }
    }
}