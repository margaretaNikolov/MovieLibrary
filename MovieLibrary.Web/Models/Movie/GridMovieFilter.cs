
namespace MovieLibrary.Web.Models
{
    public class GridMovieFilter
    {
        public string Caption { get; set; }
        public int? ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public int? NumberOfAvailableCopies { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortField { get; set; }
        public string sortOrder { get; set; }

        public GridMovieFilter()
        {
            pageIndex = 0;
            pageSize = 10;
        }
    }

   
}