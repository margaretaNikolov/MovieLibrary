using MovieLibrary.Web.CutomFilters;
using System.Web;
using System.Web.Mvc;

namespace MovieLibrary.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogActionAttribute());
        }
    }
}
