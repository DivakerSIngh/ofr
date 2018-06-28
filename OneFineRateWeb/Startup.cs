using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneFineRateWeb.Startup))]
namespace OneFineRateWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR();
            ConfigureAuth(app);
            //app.SetLoggerFactory(new DiagnosticsLoggerFactory()); 
        }
    }
}
