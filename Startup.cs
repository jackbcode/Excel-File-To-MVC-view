using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DailyCalls.Startup))]
namespace DailyCalls
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
