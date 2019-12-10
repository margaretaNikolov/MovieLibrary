using System;

namespace MovieLibrary.Web.Models
{
    public class GenreModel
    {   
        public int GenreID { get; set; }     
        public string Name { get; set; }   
        public string Comment { get; set; }
        public DateTime InsertDate { get; set; }
    }
}