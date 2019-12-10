using System;

namespace MovieLibrary.Business.DTO
{
    public class WWListDTO
    {        
        public int UserID { get; set; }
        public int MovieID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }           
    }
}
