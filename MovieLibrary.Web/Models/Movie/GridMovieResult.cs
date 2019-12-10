using System.Collections.Generic;

namespace MovieLibrary.Web.Models
{
    public class GridMovieResult
    {
        //public List<MovieGridModel> data { get; set; }
        public List<MovieModel> data { get; set; }
        public int itemsCount { get; set; }       
    }
}