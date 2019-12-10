using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.Web.Models
{
    public class GenreAddModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenreID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } 
        [MaxLength(120)]
        public string Comment { get; set; }             
    }
}