using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaProject.Models;
using AgendaProject.Models.Lib;
using AgendaProject.Models.ViewModels;

namespace AgendaProject.Areas.AgendaManager.Controllers
{
    public class AgendaController : Controller
    {
        // GET: Agenda
        public ActionResult Index()
        {
            var agenda = new Agenda();
            return View(agenda.GetPreliminaryAgendas());
        }

        public ActionResult Details(int id)
        {
            var agenda = new Agenda();
            return View(agenda.GetAgenda(id));
        }

        public ActionResult Add(int id)
        {
            var agenda = new Agenda();
            agenda = agenda.GetAgenda(id);

            var sections = agenda.SectionHeadings;
            ViewBag.SectionHeadings =
                sections.Select(s => new SelectListItem { Text = s.Description, Value = s.AgendaSection.ToString() });

            return View();
        }

        [HttpPost]
        public ActionResult Add(AddAgendaItemView item)
        {
            var sectionHeading = new SectionHeading();
            return RedirectToAction("Details", new { id = sectionHeading.AddAgendaItem(item) });
        }

        public ActionResult Edit(int id)
        {
            var agenda = new Agenda();
            agenda = agenda.GetAgenda(id);

            var sections = agenda.SectionHeadings;

            var sectionHeadings =
                sections.Select(s => new SelectListItem { Text = s.Description, Value = s.AgendaSection.ToString() });
            var SectionList = new List<SelectListItem>();
            foreach (var item in sectionHeadings)
            {
                SectionList.Add(item);
            }

            var defaultItem = new SelectListItem();
            defaultItem.Text = "Move To";
            defaultItem.Value = "0";
            defaultItem.Disabled = true;
            defaultItem.Selected = true;
            SectionList.Add(defaultItem);
            ViewBag.SectionHeadings = SectionList;


            return View(agenda);
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddAttachment(int id)
        {
            var attachment = new Attachment();
            attachment.AgendaItemId = id;
            return View(attachment);
        }

        [HttpPost]
        public ActionResult AddAttachment(FormCollection formCollection)
        {
            var attachment = new Attachment();
            var agendaItem = new AgendaItem();
            attachment.AgendaItemId = int.Parse(formCollection["AgendaItemId"]);
            attachment.Description = formCollection["Description"];
            HttpPostedFileBase file = Request.Files["MyFile"];

            agendaItem = agendaItem.GetAgendaItem((attachment.AgendaItemId ?? default(int)));
            var sectionHeading = new SectionHeading();
            sectionHeading = sectionHeading.GetSectionHeading(agendaItem.SectionHeadingId ?? default(int));

            var agenda = new Agenda();
            agenda = agenda.GetAgenda(sectionHeading.AgendaId);

            attachment.FilePath = FileFolders.SaveAgendaItemAttachment(agenda.MeetingDateTime.Year.ToString(),
                agenda.MeetingDateTime.ToString("MMM-dd"), file);

            agendaItem.AddAttachment(attachment);


            return RedirectToAction("Edit", new { id = sectionHeading.AgendaId });
        }

        public ActionResult DeleteAttachment(int id, int agendaId)
        {
            var attachment = new Attachment();
            attachment = attachment.GetAttachment(id);
            FileFolders.DeleteAttachment(attachment.FilePath);

            var agendaItem = new AgendaItem();
            agendaItem.RemoveAttachment(attachment);
            
            



            return RedirectToAction("Edit", new {id = agendaId});
        }


        public ActionResult MoveAgendaItem(int itemId, int sectionHeadingId, int agendaId)
        {
            var sectionHeading = new SectionHeading();
            sectionHeading.MoveAgendaItem(itemId, sectionHeadingId);

            return RedirectToAction("Edit", new { id = agendaId });
        }

        public ActionResult EditItem(int id)
        {
            var agendaItem = new AgendaItem();
            agendaItem = agendaItem.GetAgendaItem(id);
            return View(agendaItem);
        }

        [HttpPost]
        public ActionResult EditItem(AgendaItem agendaItem)
        {
            var agendaId = agendaItem.EditAgendaItem();


            return RedirectToAction("Edit", new { id = agendaId });
        }

        public ActionResult MoveUp(int id)
        {
            var agendaItem = new AgendaItem();
            agendaItem = agendaItem.GetAgendaItem(id);
            return RedirectToAction("Edit", new { id = agendaItem.MoveUp() });
        }

        public ActionResult MoveDown(int id)
        {
            var agendaItem = new AgendaItem();
            agendaItem = agendaItem.GetAgendaItem(id);
            return RedirectToAction("Edit", new { id = agendaItem.MoveDown() });
        }

        public ActionResult DeleteAgendaItem(int id, int sid)
        {
            var agendaItem = new AgendaItem();
            agendaItem = agendaItem.GetAgendaItem(id);

            var agendaId = agendaItem.DeleteAgendaItem(sid);


            return RedirectToAction("Edit", new { id = agendaId });
        }

        public ActionResult Finalize(int id)
        {
            var agenda = new Agenda();
            agenda.ChangeAgendaStatus(id);

            return RedirectToAction("Details", new { id });
        }

        public ActionResult AddVoteItem(int id)
        {
            var voteItem = new VoteItem();
            voteItem = voteItem.SetDefault(id);

            return View(voteItem);
        }

        [HttpPost]
        public ActionResult AddVoteItem(VoteItem voteItem)
        {
            var agendaId = voteItem.Save(voteItem);
            return RedirectToAction("Edit", new { id = agendaId });
        }
    }
}