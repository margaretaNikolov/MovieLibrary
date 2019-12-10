using System;

namespace MovieLibrary.Business.DTO
{
    public class RentedMovieDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
       
        public  MovieDTO Movie { get; set; }
        public  UserDTO User { get; set; }
    }
}
