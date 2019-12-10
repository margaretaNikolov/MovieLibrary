using MovieLibrary.Web.WebAutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MovieLibrary.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //automapper:Instance - Business Layer
            //MapperConfiguration.Configure();
            //automapper:Instance - Web Apps Layer
            MVCMapperConfiguration.Configure();
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
