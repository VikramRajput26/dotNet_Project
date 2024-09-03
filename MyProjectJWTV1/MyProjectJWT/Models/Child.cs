using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyProjectJWT.Models
{
    public class Child
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChildId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(3)]
        public string BloodType { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        // Foreign key to the parent User
        [Required]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }

        // Navigation property for related Appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
