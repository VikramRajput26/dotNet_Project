using System;

namespace MyProjectJWT.Models
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public int ChildId { get; set; }

        public int DoctorId { get; set; }

        public int VaccineId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }
}
