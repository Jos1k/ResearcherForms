using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResearcherForms.Startup))]
namespace ResearcherForms
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
