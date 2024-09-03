using MyProjectJWT.Models;

namespace MyProjectJWT.DTO
{
    public class ChildDTO
    {
        public int ChildId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodType { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserId { get; set; }
    }
}
