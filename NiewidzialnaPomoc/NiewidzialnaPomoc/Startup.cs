using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NiewidzialnaPomoc.Startup))]
namespace NiewidzialnaPomoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
