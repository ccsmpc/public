using System.Web.Mvc;

namespace AgendaProject.Areas.MeetingManager
{
    public class MeetingManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MeetingManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MeetingManager_default",
                "MeetingManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new []{ "AgendaProject.Areas.MeetingManager.Controllers" }
            );
        }
    }
}