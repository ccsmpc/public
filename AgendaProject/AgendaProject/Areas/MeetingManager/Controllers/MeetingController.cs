using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AgendaProject.DAL;
using AgendaProject.Models;
using AgendaProject.Models.ViewModels;

namespace AgendaProject.Areas.MeetingManager.Controllers
{
    public class MeetingController : Controller
    {
        private readonly AgendaContext db = new AgendaContext();

        // GET: MeetingManager/Meeting
        public ActionResult Index()
        {
            var board = new BoardMember();
            ViewBag.BoardMembers = board.GetBoardMembersForDropdown(2);

            return View();
        }

        public ActionResult Manager()
        {
            var agenda = new Agenda();
            ViewBag.FinalAgendas = agenda.GetFinalAgendas();
            ViewBag.Motions =
                db.DropDowns.Where(s => s.DropDownName == "MotionSelector")
                    .Select(s => new SelectListItem {Text = s.Text, Value = s.Value});
            var board = new BoardMember();
            ViewBag.BoardMembers = board.GetBoardMembersForDropdown(2);

            ViewBag.ContinueTo = agenda.GetFutureMeetingDates();


            return View();
        }

        public ActionResult Presenter()
        {
            return View();
        }

        public ActionResult Tech()
        {
            return View();
        }

        public JsonResult GetSectionHeading(int sectionId)
        {
            var section = new SectionHeading();
            section = section.GetSectionHeading(sectionId);

            return Json(section.Description, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgenda(int agendaId)
        {
            var agenda = new Agenda();
            agenda = agenda.GetAgenda(agendaId);

            return Json(agenda, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegisterVoteItem(VoteItem voteItem, int agendaItemId)
        {
            db.VoteItems.Add(voteItem);

            db.SaveChanges();

            var agendaItem = new AgendaItem();
            agendaItem = agendaItem.GetAgendaItem(agendaItemId);
            agendaItem.VoteItems.Add(voteItem);
            db.SaveChanges();

            var actors = new Dictionary<string, string>();
            actors.Add("VoteItemId", voteItem.Id.ToString());

            return Json(actors);
        }


        [HttpPost]
        public JsonResult RecordVote(int voteItemId, int boardMemberId, string voteCast, string phase)
        {
            var vote = new Vote();
            vote.RecordVote(voteItemId, boardMemberId, voteCast, phase);
            return Json("Success");
        }

        [HttpPost]
        public JsonResult RecordAmendedText(int voteItemId, int votePhase, string amendedText)
        {
            var voteItem = new VoteItem();
            voteItem = db.VoteItems.Find(voteItemId);

            switch (votePhase)
            {
                case 2:
                    voteItem.apAmendment = amendedText;
                    break;
                case 1:
                    voteItem.aMotion = amendedText;
                    break;
                default:
                    break;
            }

            db.SaveChanges();
            return Json("Success");

        }


        [HttpPost]
        public JsonResult TallyVote(int voteItemId, int votePhase, int membersPresent)
        {
            var vote = new Vote();
            var resultString = vote.TallyVote(voteItemId, membersPresent);
            var split = resultString.Split(new[] {'.'}, 2);
            var result = split[0];
            var tally = split[1];
            var shortResult = result.Contains("passed") ? "passed" : "failed";

            var voteItem = new VoteItem();

           
            voteItem.RecordResult(voteItemId, votePhase, shortResult);

            var voteResult = new Dictionary<string, string>();
            voteResult.Add("result", result);
            voteResult.Add("tally", tally);
            Thread.Sleep(350);
            return Json(voteResult);
        }

        [HttpPost]
        public JsonResult HandleContinuedItem(int voteItemId, DateTime dateCertain)
        {
            AgendaContext db = new AgendaContext();  // must have the same object context, so we handle this one differently

            var voteItem = new VoteItem();
            voteItem = voteItem.GetVoteItemById(voteItemId);
            var oldAgendaItem = db.AgendaItems.Find(voteItem.AgendaItemId);
            
            
            var agenda = new Agenda();
            var section = new SectionHeading();
            agenda = agenda.GetAgendaByMeetingDate(dateCertain);
            var sectionHeadings = agenda.SectionHeadings;
            section = sectionHeadings.First(s => s.Description.Contains("VII"));
            var thisEntry = new AddAgendaItemView();
            thisEntry.AgendaSection = section.Id;
            thisEntry.Description = oldAgendaItem.Description;
            var notused = section.AddAgendaItem(thisEntry);

            // so, the new agendaItem is made, and the id is set, get it
            
           
            /*
            var newAgendaItem =
                db.AgendaItems.First(s => s.Description == oldAgendaItem.Description && s.SectionHeadingId == section.Id);
            newAgendaItem.WorkItemId = oldAgendaItem.WorkItemId;

            var workItem = oldAgendaItem.WorkItems.First();
            var workItems = new List<WorkItem>();
            workItems.Add(workItem);
            newAgendaItem.WorkItems = workItems;

            db.SaveChanges();
            */
            return Json("Success");
        }


        public ActionResult Chat()
        {
            return View();
        }
    }
}