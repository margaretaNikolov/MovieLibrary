
namespace MovieLibrary.Business.DTO
{
    public class GridMovieFilterDTO
    {
        public string Caption { get; set; }
        public int? ReleaseYear { get; set; }
        public string SubmittedBy { get; set; }
        public int? NumberOfAvailableCopies { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortField { get; set; }   // the name of sorting field
        public string sortOrder { get; set; }   // the order of sorting as string "asc"|"desc"
        public int Skip => pageSize * (pageIndex - 1);
    }
}
