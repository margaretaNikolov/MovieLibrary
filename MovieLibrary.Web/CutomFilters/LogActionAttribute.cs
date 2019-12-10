using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MovieLibrary.Web.CutomFilters
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RequestContext.RouteData.Values["Controller"];
            var action = filterContext.RequestContext.RouteData.Values["Action"]; 
            string message =" Controller:" + controller + " Action:" + action + " Date: " + DateTime.Now.ToString() + Environment.NewLine;                      
            //saving the data in a log file 
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/movielibraryWebLog"), message);

            base.OnActionExecuting(filterContext);
        }

    }
}