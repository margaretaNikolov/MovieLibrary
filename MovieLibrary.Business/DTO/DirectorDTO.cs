using System;
using System.Collections.Generic;

namespace MovieLibrary.Business.DTO
{
    public class DirectorDTO
    {
        public int DirectorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }

        public virtual ICollection<MovieDTO> Movies { get; set; }
    }
}
