using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Interfaces
{
    public interface IAppointmentService
    {
        public List<AppointmentDTO> GetAppointmentDetails();
        public AppointmentDTO GetAppointmentById(int id);
        public AppointmentDTO AddAppointment(CreateAppointmentDTO createAppointment);
        public AppointmentDTO UpdateAppointment(AppointmentDTO appointmentDTO);
        public bool DeleteAppointment(int id);
    }
}
