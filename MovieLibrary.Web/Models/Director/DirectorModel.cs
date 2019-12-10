using System;

namespace MovieLibrary.Web.Models
{
    public class DirectorModel
    {        
        public int DirectorID { get; set; }       
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public DateTime InsertDate { get; set; }       
    }
}