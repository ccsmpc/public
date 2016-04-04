using Microsoft.Owin;
using Owin;
using AgendaProject;

[assembly: OwinStartup(typeof(AgendaProject.Startup))]

namespace AgendaProject
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
