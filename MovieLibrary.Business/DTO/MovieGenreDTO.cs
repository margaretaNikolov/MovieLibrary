
namespace MovieLibrary.Business.DTO
{
    public class MovieGenreDTO
    {
        public int ID { get; set; }
        public int GenreID { get; set; }
        public int MovieID { get; set; }

        public  GenreDTO Genre { get; set; }       
    }
}
