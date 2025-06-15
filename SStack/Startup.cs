using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SStack.Startup))]
namespace SStack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
