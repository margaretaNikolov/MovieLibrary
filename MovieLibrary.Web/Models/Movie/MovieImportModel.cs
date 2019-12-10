using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class MovieImportModel
    {     
        public string Caption { get; set; }
        public int ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public int NumberOfAvailableCopies { get; set; }
        public List<string> MovieGenres { get; set; }
        public List<string> MovieDirectors { get; set; }
    }
}