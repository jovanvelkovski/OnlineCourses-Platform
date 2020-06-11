using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(platforma.Startup))]
namespace platforma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
