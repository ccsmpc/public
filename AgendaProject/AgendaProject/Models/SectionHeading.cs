using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AgendaProject.DAL;
using AgendaProject.Models.ViewModels;

namespace AgendaProject.Models
{
    public partial class SectionHeading
    {
        public int Id { get; set; }
        public int AgendaSection { get; set; }
        public string Description { get; set; }

        AgendaContext db = new AgendaContext();
    }

    public partial class SectionHeading
    {
        public int AddAgendaItem(AddAgendaItemView item)
        {

            var sectionHeading = db.SectionHeadings.Find(item.AgendaSection);
            var agendaItem = new AgendaItem();

            agendaItem.Description = item.Description;

            if (sectionHeading.AgendaItems.Count == 0)
            {
                agendaItem.Prev = null;
                agendaItem.Nxt = null;
                sectionHeading.AgendaItems.Add(agendaItem);

                db.SaveChanges(); // to get ID set, and if its the first item, we are done.
            }
            else
            {


                int? endItem = 0;
                foreach (var oldItem in sectionHeading.AgendaItems)
                {
                    if (oldItem.Nxt == null)
                    {
                        endItem = oldItem.Id;
                    }
                }
                var lastItem = db.AgendaItems.Find(endItem);

                sectionHeading.AgendaItems.Add(agendaItem);
                db.SaveChanges();  // to get id set

                lastItem.Nxt = agendaItem.Id;
                agendaItem.Prev = lastItem.Id;
                agendaItem.Nxt = null;
                db.SaveChanges();
            }




            return sectionHeading.AgendaId;

        }

        public void MoveAgendaItem(int agendaItemId, int destSectionHeadingId)
        {
            var agendaItem = db.AgendaItems.Find(agendaItemId);
            var oldSection = db.SectionHeadings.Find(agendaItem.SectionHeadingId);
            var newSection = db.SectionHeadings.Find(destSectionHeadingId);

            var previousAgendaItem = db.AgendaItems.Find(agendaItem.Prev);
            var nextAgendaItem = db.AgendaItems.Find(agendaItem.Nxt);

            if (previousAgendaItem != null)
            {
                previousAgendaItem.Nxt = agendaItem.Nxt;
                db.Entry(previousAgendaItem).State = EntityState.Modified;
            }
            if (nextAgendaItem != null)
            {
                nextAgendaItem.Prev = agendaItem.Prev;
                db.Entry(nextAgendaItem).State = EntityState.Modified;
            }

            oldSection.AgendaItems.Remove(agendaItem);

            // done with the old section

            if (newSection.AgendaItems.Any())
            {
                // need to find the end of the list
                foreach (var item in newSection.AgendaItems)
                {
                    if (item.Nxt == null)
                    {
                        // you have the end, so
                        item.Nxt = agendaItem.Id;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        agendaItem.Nxt = null;
                        agendaItem.Prev = item.Id;
                    }
                }
            }
            else
            {
                // there are no entrys in the new section
                agendaItem.Prev = null;
                agendaItem.Nxt = null;

            }

            newSection.AgendaItems.Add(agendaItem);
            db.SaveChanges();
        }
    }

    public partial class SectionHeading
    {
        // Data Access
        public SectionHeading GetSectionHeading(int id)
        {
            AgendaContext db = new AgendaContext();
            var sectionHeading = db.SectionHeadings.Find(id);
            return sectionHeading;
        }
    }

    public partial class SectionHeading
    {
        // Navigation Properties
        public int AgendaId { get; set; }
        public virtual ICollection<AgendaItem> AgendaItems { get; set; }
    }
}