using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EzyPeezy.Startup))]
namespace EzyPeezy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
