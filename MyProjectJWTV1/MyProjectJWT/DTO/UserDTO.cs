using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyProjectJWT.Models;
namespace MyProjectJWT.DTO
{
    public class UserDTO
    {
        
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public Role UserRole { get; set; }
    }
}
