using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookLive.Startup))]
namespace BookLive
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
