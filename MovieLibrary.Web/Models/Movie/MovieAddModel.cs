using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MovieLibrary.Web.Models
{
    public class MovieAddModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Caption { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public string SubmittedBy { get; set; }       
        [Required]      
        public int NumberOfAvailableCopies { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        public string ImageTitle { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public SelectedDirectorModel SelectedDirectorModel { get; set; }
        public SelectedGenreModel SelectedGenreModel { get; set; }

        //public MultiSelectList  Directors { get; set;}
        //public MultiSelectList Genres { get; set; }

        public MovieAddModel()
        {
            SelectedDirectorModel = new SelectedDirectorModel();
            SelectedGenreModel = new SelectedGenreModel();
        }
    }
}