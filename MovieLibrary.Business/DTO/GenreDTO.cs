using System;
using System.Collections.Generic;

namespace MovieLibrary.Business.DTO
{
    public class GenreDTO
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public virtual ICollection<MovieDTO> Movies { get; set; }
    }
}
