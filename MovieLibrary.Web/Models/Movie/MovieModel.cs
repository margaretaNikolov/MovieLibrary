using System.Collections.Generic;
using System.ComponentModel;

namespace MovieLibrary.Web.Models
{
    public class MovieModel
    {//List - Display Movie Model
        public int MovieID { get; set; }
        public string Caption { get; set; }
        public int ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public int NumberOfAvailableCopies { get; set; }
        public byte[] Image { get; set; }   
        [DisplayName("Cover Poster")]
        public string ImageTitle { get; set; }

        public List<GenreLightModel> MovieGenres { get; set; }
        public List<DirectorLightModel> MovieDirectors { get; set; }
    }
}