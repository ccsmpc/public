using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using AgendaProject.DAL;

namespace AgendaProject.Models.Lib
{
    public class MailHelper
    {
        /*
               The mail helper just sends the message.  Its the calling controller's job to handle 
                   message seperation functionality.
        */
        public static void SendMailMessage(string To, string From, string Subject, string Body)
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress(From);
            message.To.Add(new MailAddress(To));
            message.Subject = Subject;
            message.Body = Body;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Priority = MailPriority.Normal;

            SmtpClient mailer = new SmtpClient();
            mailer.Host = "rural-1";
            mailer.Port = 25;
            mailer.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailer.UseDefaultCredentials = true;

            try
            {
                mailer.Send(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Send failed: " + ex.Message);
            }

            message.Dispose();
        }

        public static string GetEmailAddress(string name)
        {
            AgendaContext db = new AgendaContext();

            var employee = db.Employees.First(s => s.FirstName + " " + s.LastName == name);
            return employee.WorkEmail;



        }
    }
}