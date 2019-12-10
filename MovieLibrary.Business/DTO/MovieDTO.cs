using System;
using System.Collections.Generic;

namespace MovieLibrary.Business.DTO
{
    public class MovieDTO
    {
        public int MovieID { get; set; }
        public string Caption { get; set; }      
        public int ReleaseYear { get; set; }     
        public string SubmittedBy { get; set; }        
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }       
        public int? NumberOfAvailableCopies { get; set; }
        public string[] SelectedDirectorIDs { get; set; }
        public string[] SelectedGenreIDs { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public string ImageTitle { get; set; }

        public virtual List<MovieGenreDTO> MovieGenres { get; set; }
        public virtual List<MovieDirectorDTO> MovieDirectors { get; set; }
    }
}
