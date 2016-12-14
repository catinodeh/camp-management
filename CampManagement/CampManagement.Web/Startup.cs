using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampManagement.Web.Startup))]
namespace CampManagement.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
