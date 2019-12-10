using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class AllGenreModel
    {
        public string[] GenreIDs { get; set; }
        public IEnumerable<GenreLightModel> GenreCollection { get; set; }

        public AllGenreModel()
        {
            GenreCollection = new List<GenreLightModel>();
        }
    }
}