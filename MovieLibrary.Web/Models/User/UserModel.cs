using System;
using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Web.Models
{
    public class UserModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(512)]
        public string Address { get; set; }
        [Required]
        [MaxLength(50)]
        public string IDNumber { get; set; }
        [Required]
        public int MaritalStatusID { get; set; }
        [Required]
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}