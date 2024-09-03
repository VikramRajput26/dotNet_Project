using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyProjectJWT.Models
{
    [Index(nameof(Email), IsUnique = true)]  // Adding unique constraint on Email
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is Mandatory!!!")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Mandatory!!!")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Mandatory!!!")]
        [EmailAddress(ErrorMessage = "Email format is Invalid!!!")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Mandatory!!!")]
        [MinLength(3)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Contact Number is Mandatory!!!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number should be 10 digits")]
        [MaxLength(10)]
        public string ContactNumber { get; set; }

        [Required]
        public Role UserRole { get; set; }

        // Navigation property for related Children
        public ICollection<Child> Children { get; set; } = new List<Child>();

        // Navigation property for related Appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
