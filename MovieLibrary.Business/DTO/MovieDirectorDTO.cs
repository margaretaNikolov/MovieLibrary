
namespace MovieLibrary.Business.DTO
{
    public class MovieDirectorDTO
    {
        public int ID { get; set; }
        public int DirectorID { get; set; }
        public int MovieID { get; set; }

        public  DirectorDTO Director { get; set; }
   
    }
}
