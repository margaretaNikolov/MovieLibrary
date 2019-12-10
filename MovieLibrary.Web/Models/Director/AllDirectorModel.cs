
using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class AllDirectorModel
    {
        public string[] DirectorIDs { get; set; }
        public IEnumerable<DirectorLightModel> DirectorCollection { get; set; }

        public AllDirectorModel()
        {
            DirectorCollection = new List<DirectorLightModel>();
        }
    }
}