namespace MyProjectJWT.Models
{
    public class VaccineDTO
    {
        public int VaccineId { get; set; }

        public string VaccineName { get; set; }

        public string Description { get; set; }

        public int RecommendedAge { get; set; }

        public AgeUnit AgeUnit { get; set; }  // Include the AgeUnit enum

        public string SideEffects { get; set; }
    }
}
