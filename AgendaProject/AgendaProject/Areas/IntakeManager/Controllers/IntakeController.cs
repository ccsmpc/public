using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaProject.DAL;
using AgendaProject.Models;
using AgendaProject.Models.Lib;
using AgendaProject.Models.ViewModels;

namespace AgendaProject.Areas.IntakeManager.Controllers
{
    public class IntakeController : Controller
    {
        AgendaContext db = new AgendaContext();
        public ActionResult Index()
        {
            WorkItem workItem = new WorkItem();
            
            return View(workItem.GetActiveWorkItems());
        }

        public ActionResult CreateWorkItem()
        {
            var workItem = new WorkItem();
            workItem.Recieved = DateTime.Today;

            var petitonType = db.DropDowns.Where(s => s.DropDownName == "PetitionType").Select(s => new SelectListItem { Text = s.Text, Value = s.Value});
            ViewBag.Type = petitonType.OrderBy(s => s.Text).ToList();


            return View(workItem);
        }

        [HttpPost]
        public ActionResult CreateWorkItem(WorkItem workItem)
        {
            Employee employee = new Employee();
            var deptHead = employee.GetDepartmentHead();
            workItem.AssignedTo = deptHead;
            workItem.ReadyForReview = false;
            workItem.WasWithdrawn = false;
            workItem.IsComplete = false;

            workItem.AddWorkItem();

            var sendTo = MailHelper.GetEmailAddress(deptHead);
            var from = "administrator@thempc.org";
            var subject = "File number " + workItem.Filenumber + " has been created.";
            var body = "Once all intake documents have been added to the file, it will be ready for assignment.";

            MailHelper.SendMailMessage(sendTo, from, subject, body);

            return RedirectToAction("Index");
        }

        public ActionResult AddAttachment(int id)
        {
            var attachment = new Attachment();
            attachment.WorkItemId = id;
            return View(attachment);
        }

        [HttpPost]
        public ActionResult AddAttachment(FormCollection formCollection)
        {
            WorkItem item = new WorkItem();
            Attachment attachment = new Attachment();
            attachment.WorkItemId = int.Parse(formCollection["WorkItemId"]);
            attachment.Description = formCollection["Description"];
            HttpPostedFileBase file = Request.Files["MyFile"];
            item = item.FindById(int.Parse(formCollection["WorkItemId"]));

            attachment.AddWorkItemAttachment(item.Recieved.Year.ToString(), item.Filenumber, file);

            return RedirectToAction("Index");
        }

        public ActionResult AddAddress(int id)
        {
            var address = new Address();
            address.WorkItemId = id;
            return View(address);
        }

        [HttpPost]
        public ActionResult AddAddress(Address address)
        {
            address.AddAddress();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var workItem = new WorkItem();

            return View(workItem.FindById(id));
        }

        public ActionResult Edit(int id)
        {
            var workItem = new WorkItem();
            workItem= workItem.FindById(id);

            var petitionType = db.DropDowns.Where(s => s.DropDownName == "PetitionType")
                .Select(s => new SelectListItem { Text = s.Text, Value = s.Value, Selected = (s.Text == workItem.PetitionType)});
            ViewBag.Type = petitionType.OrderBy(s => s.Text).ToList();

            var employee = db.Employees.Where(s => s.Inactive != true)
                .Select(s => new SelectListItem { Text = s.FirstName + " " + s.LastName, Value = s.FirstName + " " + s.LastName });
            ViewBag.Employee = employee.OrderBy(s => s.Text).ToList();



            return View(workItem);
        }

        [HttpPost]
        public ActionResult Edit(WorkItem workItem)
        {
            // if the item is marked complete, perform ReviewComplete actions, and if successful, save the workitem otherwise abort.
            // if the item is an edit save, save the item.
            // DO NOT PERFORM EDITS AND MARK COMPLETE
            var oldWorkItem = new WorkItem();
            oldWorkItem = oldWorkItem.FindById(workItem.Id);


            if (workItem.IsComplete)
            {
                if (workItem.ReviewComplete()) workItem.SaveWorkItem(oldWorkItem.AssignedTo);
            }
            else
            {
                workItem.SaveWorkItem(oldWorkItem.AssignedTo);
            }


            return RedirectToAction("Index");
        }

        public ActionResult AddStaffDecision(int id)
        {
            var staffDecisionSelector = db.DropDowns.Where(s => s.DropDownName == "StaffDecision").Select(s => new SelectListItem { Text = s.Text, Value = s.Value });
            ViewBag.StaffDecision = staffDecisionSelector.OrderBy(s => s.Text).ToList();

            var staffDecision = new StaffDecision();
            staffDecision.WorkItemId = id;
            return View(staffDecision);
        }

        [HttpPost]
        public ActionResult AddStaffDecision(StaffDecision staffDecision)
        {
            staffDecision.AddStaffDecision();
            return RedirectToAction("Index");
        }

        public ActionResult EditDescription(int id, int workItemId)
        {
            var attachmentView = new EditAttachment();
            var attachment = new Attachment();
            attachmentView.Attachment = attachment.GetAttachment(id);
            attachmentView.WorkItemId = workItemId;

            return View(attachmentView);

        }

        [HttpPost]
        public ActionResult EditDescription(EditAttachment vm)
        {
            var attachment = new Attachment();
            

            attachment.EditAttachmentDescription(vm.Attachment);
            return RedirectToAction("Edit", new { id = vm.WorkItemId });
        }

        public ActionResult DeleteAttachment(int id, int workItemId)
        {
            var attachment = new Attachment();
            attachment.DeleteAttachment(id, workItemId);
            return RedirectToAction("Edit", new {id = workItemId});

        }

    }
}