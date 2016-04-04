using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgendaProject.DAL;

namespace AgendaProject.Models
{
    public partial class AgendaItem
    {
        public bool CreateAgendaItemFromWorkItem(int workItemId)
        {
            bool returnFlag = false;                    // Assume that it all fails - the default case :)

            AgendaContext db = new AgendaContext();

            var workItem = db.WorkItems.First(s => s.Id == workItemId);
            //WorkItems = new List<WorkItem>();
            //WorkItems.Add(workItem);
            var meetingdate = db.MeetingTimes.OrderBy(s => s.CutoffDate).First(s => s.CutoffDate >= workItem.Recieved);
            var agenda = db.Agendas.OrderBy(s => s.MeetingDateTime).First(s => s.MeetingDateTime == meetingdate.MeetingDate);
            var sections = agenda.SectionHeadings;

            switch (workItem.PetitionType)
            {
                case "Extension":
                    var extensionSection = sections.First(s => s.AgendaSection == 9);
                    var agendaItems = extensionSection.AgendaItems;

                    int? endOfList = null;
                    foreach (var item in agendaItems)
                    {
                        if (item.Nxt == null)  // have the last item
                        {
                            endOfList = item.Id;
                        }
                    }

                    this.Prev = endOfList;
                    extensionSection.AgendaItems.Add(this);
                    db.SaveChanges();

                    if (endOfList != null)
                    {
                        var updateItem = db.AgendaItems.Find(endOfList);
                        updateItem.Nxt = this.Id;
                        db.SaveChanges();
                    }

                    
                    returnFlag = true;
                    break;
                case "Board":
                    var regularSection = sections.First(s => s.AgendaSection == 8);
                    var regularAgendaItems = regularSection.AgendaItems;

                    int? regularEndOfList = null;
                    foreach (var item in regularAgendaItems)
                    {
                        if (item.Nxt == null)  // have the last item
                        {
                            regularEndOfList = item.Id;
                        }
                    }

                    this.Prev = regularEndOfList;
                    regularSection.AgendaItems.Add(this);
                    db.SaveChanges();

                    if (regularEndOfList != null)
                    {
                        var regularUpdateItem = db.AgendaItems.Find(regularEndOfList);
                        regularUpdateItem.Nxt = this.Id;
                        db.SaveChanges();
                    }
                    

                    returnFlag = true;
                    break;
                case "Staff":
                    var staffSection = sections.First(s => s.AgendaSection == 10);
                    var staffAgendaItems = staffSection.AgendaItems;

                    int? staffEndOfList = null;
                    foreach (var item in staffAgendaItems)
                    {
                        if (item.Nxt == null)  // have the last item
                        {
                            staffEndOfList = item.Id;
                        }
                    }

                    this.Prev = staffEndOfList;
                    staffSection.AgendaItems.Add(this);
                    db.SaveChanges();
                    if (staffEndOfList != null)
                    {
                        var staffUpdateItem = db.AgendaItems.Find(staffEndOfList);
                        staffUpdateItem.Nxt = this.Id;
                        db.SaveChanges();
                    }
                    

                    returnFlag = true;
                    break;
                default:
                    Debug.WriteLine("Error in case");
                    break;
            }

            if (returnFlag)
            {

            }

            return returnFlag;


        }

    }
}