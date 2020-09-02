using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RichlynnFinancialPortal.Startup))]
namespace RichlynnFinancialPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
