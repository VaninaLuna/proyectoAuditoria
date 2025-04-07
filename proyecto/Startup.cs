using System.Configuration;
using System.Reflection;
using proyecto;
using DNF.Release.Bussines;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace proyecto
{
    public partial class Startup
    {


        public void Configuration(IAppBuilder app)
        {

        }
    }
}

