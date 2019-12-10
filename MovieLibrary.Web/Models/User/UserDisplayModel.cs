using System;
using System.Drawing;

namespace MovieLibrary.Web.Models
{
    public class UserDisplayModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public string Caption { get; set; }
        public DateTime InsertDate { get; set; }
        public byte[] Image { get; set; }
        public string ImageTitle { get; set; }     
    }
}