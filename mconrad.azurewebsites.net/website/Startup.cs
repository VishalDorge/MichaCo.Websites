using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mconrad.azurewebsites.net.Startup))]
namespace mconrad.azurewebsites.net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
