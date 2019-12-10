using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MovieLibrary.Web.Models
{
    public class UserEditModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
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
        public int MaritalStatusID { get; set; }       
        [Required]
        public DateTime InsertDate { get; set; }    
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        public string ImageTitle { get; set; }
        public byte[] Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public List<MaritalStatuseModel> MaritalStatuses { get; set; }        

        public UserEditModel()
        {
            MaritalStatuses = new List<MaritalStatuseModel>();
        }
    }
}