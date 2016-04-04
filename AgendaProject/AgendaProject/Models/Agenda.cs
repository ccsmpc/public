using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class Agenda
    {
        private readonly AgendaContext db = new AgendaContext();
        public int Id { get; set; }
        public string Location { get; set; }
        public string AgendaType { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public string Title { get; set; }
    }

    public partial class Agenda
    {
        public void CreateAgenda()
        {
        }

        public void ChangeAgendaStatus(int id)
        {
            var agenda = db.Agendas.Find(id);
            agenda.AgendaType = "Final Agenda";
            db.SaveChanges();
        }

        public void AddMinutes()
        {
        }

        public void EditMinutes()
        {
        }

        public void PublishAgenda()
        {
        }

        public void PrintAgenda()
        {
        }

        public void PrintRecentDecisions()
        {
        }

        public void PublishRecentDecisions()
        {
        }

        public void PrintMinutes()
        {
        }

        public void PublishMinutes()
        {
        }
    }

    public partial class Agenda
    {
        // Navigation Properties

        public virtual ICollection<SectionHeading> SectionHeadings { get; set; }
    }

    public partial class Agenda
    {
        public List<Agenda> GetPreliminaryAgendas()
        {
            var agendas = db.Agendas.Where(s => s.AgendaType == "Preliminary Agenda").OrderBy(s => s.MeetingDateTime);

            return agendas.ToList();
        }

        public Agenda GetAgenda(int id)
        {
            var agenda = db.Agendas.First(s => s.Id == id);
            
            return agenda;
        }

        public IQueryable GetFinalAgendas()
        {
            var finalAgendas =
                db.Agendas.Where(s => s.AgendaType == "Final Agenda")
                    .Where(s => s.MeetingDateTime >= DateTime.Today)
                    .Select(s => new SelectListItem
                    {
                        Text = s.MeetingDateTime.ToString(),
                        Value = s.Id.ToString()
                    });
            return finalAgendas;
        }

        public IQueryable GetFutureMeetingDates()
        {
            var futureMeetingDates =
                db.Agendas.Where(s => s.MeetingDateTime > DateTime.Today)
                    .Select(
                        s => new SelectListItem {Text = s.MeetingDateTime.ToString(), Value = s.MeetingDateTime.ToString() });
            return futureMeetingDates;
        }

        public Agenda GetAgendaByMeetingDate(DateTime meetingDateTime)
        {
            var agenda = db.Agendas.First(s => s.MeetingDateTime == meetingDateTime);
            return agenda;
        }

    }
}