
namespace MovieLibrary.Web.Models
{
    public class MovieGridModel
    {
        public int MovieID { get; set; }
        public string Caption { get; set; }
        public int ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public int NumberOfAvailableCopies { get; set; }    
    }
}