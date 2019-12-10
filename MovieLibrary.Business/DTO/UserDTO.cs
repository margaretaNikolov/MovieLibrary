using System;

namespace MovieLibrary.Business.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public int MaritalStatusID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public string ImageTitle { get; set; }

        public virtual MaritalStatusDTO MaritalStatus { get; set; }
    }
}
