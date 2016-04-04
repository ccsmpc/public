using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class Employee
    {
        [Key]
        public int StaffId { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(30, ErrorMessage = "First name cannot exceed 30 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30, ErrorMessage = "Last name cannot exceed 30 characters")]
        public string LastName { get; set; }

        [MaxLength(70, ErrorMessage = "Title cannot exceed 70 characters")]
        public string Title { get; set; }

        [MaxLength(30, ErrorMessage = "Department cannot exceed 30 characters")]
        public string Department { get; set; }

        [Display(Name = "Work Pone")]
        [MaxLength(15)]
        public string WorkPhone { get; set; }

        [Display(Name = "Work Email")]
        [MaxLength(70)]
        public string WorkEmail { get; set; }

        public int SupervisorId { get; set; }

        public Boolean Inactive { get; set; }

        public string Duties { get; set; }

        AgendaContext db = new AgendaContext();
    }

    public partial class Employee
    {
        public string GetDepartmentHead()
        {
            var deptHead = db.Employees.First(s => s.Title.Contains("Director"));

            return deptHead.FirstName + " " + deptHead.LastName;
        }
    }
}