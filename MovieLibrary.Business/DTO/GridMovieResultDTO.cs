using System.Collections.Generic;

namespace MovieLibrary.Business.DTO
{
    public class GridMovieResultDTO
    {
        public List<MovieDTO> data { get; set; }
        public int itemsCount { get; set; }
        public List<List<DirectorDTO>> dataDirectors { get; set; }
        public List<List<GenreDTO>> dataGenres { get; set; }
    }
}
