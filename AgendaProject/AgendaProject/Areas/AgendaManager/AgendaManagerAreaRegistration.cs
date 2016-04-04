using System.Web.Mvc;

namespace AgendaProject.Areas.AgendaManager
{
    public class AgendaManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AgendaManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AgendaManager_default",
                "AgendaManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new []{ "AgendaProject.Areas.AgendaManager.Controllers" }
            );
        }
    }
}