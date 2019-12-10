using PagedList;
using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class UserRentAMovieModel
    {
        public IPagedList<MovieModel> MovieModels { get; set; }
        public UserDisplayModel UserDisplayModel { get; set; }
        public List<string> WishMoviesListIds { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
    }
}