using System.Web.Mvc;

namespace AgendaProject.Areas.IntakeManager
{
    public class IntakeManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "IntakeManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IntakeManager_default",
                "IntakeManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "AgendaProject.Areas.IntakeManager.Controllers" }
            );
        }
    }
}