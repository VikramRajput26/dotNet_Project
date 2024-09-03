using MyProjectJWT.Models;


namespace MyProjectJWT.DTO
{
    public class CreateChildDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

      
        public string BloodType { get; set; }

       
        public Gender Gender { get; set; }

      
        public DateTime DateOfBirth { get; set; }

    
       
        public int UserId { get; set; }
    }
}
