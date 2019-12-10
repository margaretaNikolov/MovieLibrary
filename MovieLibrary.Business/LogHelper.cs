
namespace MovieLibrary.Business
{
    public class LogHelper
    {
        public static log4net.ILog GetLogger(string filename="")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
