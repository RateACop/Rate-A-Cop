using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rate_A_Cop.Startup))]
namespace Rate_A_Cop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
