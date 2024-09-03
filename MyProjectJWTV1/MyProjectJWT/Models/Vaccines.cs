using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectJWT.Models
{
    public class Vaccines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineId { get; set; }

        [Required]
        [MaxLength(100)]  
        public string VaccineName { get; set; }

        [MaxLength(500)]  
        public string Description { get; set; }

        public int RecommendedAge { get; set; }

        [Required]
       
        [Column(TypeName = "varchar(10)")]
        public AgeUnit AgeUnit { get; set; }  

        [MaxLength(500)]  
        public string SideEffects { get; set; }
    }
}
