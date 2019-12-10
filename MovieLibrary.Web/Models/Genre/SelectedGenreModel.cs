using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class SelectedGenreModel
    {
        public string[] SelectedGenreIDs { get; set; }
        public IEnumerable<GenreLightModel> GenreCollection { get; set; }

        public SelectedGenreModel()
        {
            GenreCollection = new List<GenreLightModel>();
        }
    }
}