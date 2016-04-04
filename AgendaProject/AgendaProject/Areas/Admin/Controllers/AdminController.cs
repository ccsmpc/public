using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaProject.Models;
using System.Data.Entity;
using System.IO;
using AgendaProject.DAL;
using AgendaProject.Models.Lib;
using AgendaProject.Areas.Admin.Models;
using PagedList;


namespace AgendaProject.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        
        AgendaContext db = new AgendaContext();

        const int boardId = 2;
        const string startTime = "1:00:00 PM";
        const string cutoffTime = "5:00:00 PM";
        
        


        // GET: Hbr/Admin
        public ActionResult Index(int? page)
        {
            ViewBag.BoardId = boardId;
            ViewBag.StartTime = startTime;
            ViewBag.CutoffTime = cutoffTime;

            var schedule = db.MeetingTimes.Where(s => s.BoardId == boardId).Where(s => s.MeetingDate > DateTime.Today);
            schedule = schedule.OrderBy(s => s.MeetingDate);
            int pageSize = 12;
            int pageNumber = (page ?? 1);


            return View(schedule.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddYear()
        {
            int year = DateTime.Today.Year + 1;
            MeetingTime mDate = new MeetingTime();

            for (int i = 1; i < 13; i++)
            {
                DateTime meetingDate = DateTime.Parse(year.ToString() + "-" + i.ToString() + "-01 " + startTime);

                while (meetingDate.DayOfWeek != DayOfWeek.Wednesday)
                {
                    meetingDate = meetingDate.AddDays(1);
                }
                mDate.MeetingDate = meetingDate.AddDays(7);
                mDate.CutoffDate = mDate.MeetingDate.AddDays(-28).AddHours(4);
                mDate.BoardId = boardId;

                db.MeetingTimes.Add(mDate);
                db.SaveChanges();

                // once the meetingdate is in the calendar, its ok to make the folder structure by calling the make folder routine:
                //  first, we need the correct variables:  board, year, and monthDay

               
                // year is int year.tostring() from above
                var month = mDate.MeetingDate.ToString("MMM");
                var day = mDate.MeetingDate.Date.Day;

                FileFolders.CreateAgendaFolder(year.ToString(), month + day);
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddSpecial()
        {
            MeetingTime mDate = new MeetingTime();
            mDate.BoardId = boardId;
            mDate.MeetingDate = DateTime.Today;
            mDate.CutoffDate = DateTime.Today;

            return View(mDate);
        }

        [HttpPost]
        public ActionResult AddSpecial(MeetingTime mDate)
        {
            db.MeetingTimes.Add(mDate);
            db.SaveChanges();

            // once the meetingdate is in the calendar, its ok to make the folder structure by calling the make folder routine:
            //  first, we need the correct variables:  board, year, and monthDay

           
            var year = mDate.MeetingDate.Year;
            var month = mDate.MeetingDate.ToString("MMM");
            var day = mDate.MeetingDate.Date.Day;

            FileFolders.CreateAgendaFolder(year.ToString(), month + day);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var remove = db.MeetingTimes.Find(id);
            db.MeetingTimes.Remove(remove);
            db.SaveChanges();

            // once the meetingdate is gone from the calendar, its ok to make the folder structure delete by calling the delete folder routine:
            //  first, we need the correct variables:  board, year, and monthDay

            
            var year = remove.MeetingDate.Year;
            var month = remove.MeetingDate.ToString("MMM");
            var day = remove.MeetingDate.Date.Day;

            FileFolders.DeleteAgendaFolder(year.ToString(), month + day);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var mDate = db.MeetingTimes.Find(id);

            return View(mDate);
        }

        [HttpPost]
        public ActionResult Edit(MeetingTime mDate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mDate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mDate);
        }

        public ActionResult BoardMembers()
        {
            var currentBoardMembers =
                db.BoardMembers.Where(s => s.BoardId == boardId)
                    .Where(s => s.AppointmentStartDate < DateTime.Today)
                    .Where(s => s.AppointmentEndDate >= DateTime.Today);

            return View(currentBoardMembers);
        }

        public ActionResult CreateBoardMember()
        {
            var honorific = db.DropDowns.Where(s => s.DropDownName == "Honorific")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Honorific = honorific.OrderBy(s => s.Text).ToList();

            var authority = db.DropDowns.Where(s => s.DropDownName == "Authority")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Authority = authority.OrderBy(s => s.Text).ToList();

            var boardPosition = db.DropDowns.Where(s => s.DropDownName == "BoardPosition")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.BoardPosition = boardPosition.OrderBy(s => s.Text).ToList();

            BoardMember boardMember = new BoardMember();
            boardMember.AppointmentStartDate = DateTime.Today;
            boardMember.AppointmentEndDate = DateTime.Today;
            boardMember.BoardId = boardId;

            return View(boardMember);
        }

        [HttpPost]
        public ActionResult CreateBoardMember(BoardMember boardMember)
        {
            if (ModelState.IsValid)
            {
                db.BoardMembers.Add(boardMember);
                db.SaveChanges();
                return RedirectToAction("BoardMembers");
            }

            var honorific = db.DropDowns.Where(s => s.DropDownName == "Honorific")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.HonorificDD = honorific.OrderBy(s => s.Text).ToList();

            var authority = db.DropDowns.Where(s => s.DropDownName == "Authority")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Authority = authority.OrderBy(s => s.Text).ToList();

            var boardPosition = db.DropDowns.Where(s => s.DropDownName == "BoardPosition")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.BoardPosition = boardPosition.OrderBy(s => s.Text).ToList();

            return View(boardMember);
        }

        public ActionResult EditBoardMember(int id)
        {
            var boardMember = db.BoardMembers.Find(id);

            var honorific = db.DropDowns.Where(s => s.DropDownName == "Honorific")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Honorific = honorific.OrderBy(s => s.Text).ToList();

            var authority = db.DropDowns.Where(s => s.DropDownName == "Authority")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Authority = authority.OrderBy(s => s.Text).ToList();

            var boardPosition = db.DropDowns.Where(s => s.DropDownName == "BoardPosition")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.BoardPosition = boardPosition.OrderBy(s => s.Text).ToList();

            return View(boardMember);
        }

        [HttpPost]
        public ActionResult EditBoardMember(BoardMember boardMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boardMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BoardMembers");
            }

            var honorific = db.DropDowns.Where(s => s.DropDownName == "Honorific")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Honorific = honorific.OrderBy(s => s.Text).ToList();

            var authority = db.DropDowns.Where(s => s.DropDownName == "Authority")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.Authority = authority.OrderBy(s => s.Text).ToList();

            var boardPosition = db.DropDowns.Where(s => s.DropDownName == "BoardPosition")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.BoardPosition = boardPosition.OrderBy(s => s.Text).ToList();

            return View(boardMember);

        }

        public ActionResult DeleteBoardMember(int id)
        {
            var boardMember = db.BoardMembers.Find(id);
            db.BoardMembers.Remove(boardMember);
            return RedirectToAction("BoardMembers");
        }

        public ActionResult DetailBoardMember(int id)
        {
            var boardMember = db.BoardMembers.Find(id);
            return View(boardMember);
        }

        public ActionResult UploadPhoto()
        {
            var members =
                db.BoardMembers.Where(s => s.BoardId == boardId)
                    .Where(s => s.AppointmentStartDate < DateTime.Today)
                    .Where(s => s.AppointmentEndDate >= DateTime.Today).Select(s => new SelectListItem { Text = s.FirstName + " " + s.LastName, Value = s.BoardMemeberId.ToString() });
            ViewBag.Members = members.OrderBy(s => s.Text).ToList();

            PhotoViewModel vm = new PhotoViewModel();

            return View(vm);
        }

        

        [HttpPost]
        public ActionResult UploadPhoto(PhotoViewModel vm)
        {
            var boardMember = db.BoardMembers.Find(vm.BoardMemberId);
            var Photo = Request.Files[0];
            if (Photo != null && Photo.ContentLength > 0)
            {
                var fileName = boardMember.FirstName + boardMember.LastName + ".jpg";
                var path = Path.Combine(Server.MapPath("~/Shoebox/Images/"), fileName);
                Photo.SaveAs(path);

                boardMember.PhotoName = fileName;
                db.Entry(boardMember).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("BoardMembers");
        }


    }
}
