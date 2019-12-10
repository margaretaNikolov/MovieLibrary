using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class SelectedDirectorModel
    {
        public string[] SelectedDirectorIDs { get; set; }          
        public IEnumerable<DirectorLightModel> DirectorCollection { get; set; }

        public SelectedDirectorModel()
        {
            DirectorCollection = new List<DirectorLightModel>();
        }
        
    }
}