using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof( ConoceTe.App_Start.StartUp))]
namespace ConoceTe.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}