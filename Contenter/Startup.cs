using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Contenter.Startup))]
namespace Contenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
