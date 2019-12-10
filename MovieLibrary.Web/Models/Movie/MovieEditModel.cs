using System;
using System.ComponentModel;
using System.Web;

namespace MovieLibrary.Web.Models
{
    public class MovieEditModel
    {
        public int MovieID { get; set; }
        public string Caption { get; set; }
        public int ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime InsertDate { get; set; }
        public int NumberOfAvailableCopies { get; set; }
        public byte[] Image { get; set; }
        [DisplayName("Cover Poster")]
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string[] SelectedDirectorIDs { get; set; }
        public string[] SelectedGenreIDs { get; set; }
        public AllDirectorModel AllDirectorModel { get; set; }
        public AllGenreModel AllGenreModel { get; set; }

        public MovieEditModel()
        {
            AllGenreModel = new AllGenreModel();
            AllDirectorModel = new AllDirectorModel();
        }
    }
}