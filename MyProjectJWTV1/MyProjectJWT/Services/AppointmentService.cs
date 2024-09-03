using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;

namespace MyProjectJWT.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly string _connectionString;

        public AppointmentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Database");
        }

        public List<AppointmentDTO> GetAppointmentDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var appointments = connection.Query<AppointmentDTO>("GetAllAppointments", commandType: CommandType.StoredProcedure).ToList();
                return appointments;
            }
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@AppointmentId", id);

                var appointment = connection.QuerySingleOrDefault<AppointmentDTO>("GetAppointmentById", parameters, commandType: CommandType.StoredProcedure);
                if (appointment == null)
                {
                    throw new Exception("Appointment not found");
                }

                return appointment;
            }
        }

        public AppointmentDTO AddAppointment(CreateAppointmentDTO createAppointmentDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("Reason", createAppointmentDto.Reason);
                parameters.Add("Status", createAppointmentDto.Status);
                parameters.Add("ChildId", createAppointmentDto.ChildId);
                parameters.Add("DoctorId", createAppointmentDto.DoctorId);
                parameters.Add("VaccineId", createAppointmentDto.VaccineId);
                parameters.Add("AppointmentDate", createAppointmentDto.AppointmentDate);
                parameters.Add("AppointmentTime", createAppointmentDto.AppointmentTime);

                parameters.Add("@AppointmentId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("AddAppointment", parameters, commandType: CommandType.StoredProcedure);

                var newAppointmentId = parameters.Get<int>("@AppointmentId");

                return new AppointmentDTO
                {
                    AppointmentId = newAppointmentId,
                    Reason = createAppointmentDto.Reason,
                    Status = createAppointmentDto.Status,
                    ChildId = createAppointmentDto.ChildId,
                    DoctorId = createAppointmentDto.DoctorId,
                    VaccineId = createAppointmentDto.VaccineId,
                    AppointmentDate = createAppointmentDto.AppointmentDate,
                    AppointmentTime = createAppointmentDto.AppointmentTime
                };
            }
        }

        public AppointmentDTO UpdateAppointment(AppointmentDTO appointmentDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("AppointmentId", appointmentDto.AppointmentId);
                parameters.Add("Reason", appointmentDto.Reason);
                parameters.Add("Status", appointmentDto.Status);
                parameters.Add("ChildId", appointmentDto.ChildId);
                parameters.Add("DoctorId", appointmentDto.DoctorId);
                parameters.Add("VaccineId", appointmentDto.VaccineId);
                parameters.Add("AppointmentDate", appointmentDto.AppointmentDate);
                parameters.Add("AppointmentTime", appointmentDto.AppointmentTime);

                connection.Execute("UpdateAppointment", parameters, commandType: CommandType.StoredProcedure);

                return appointmentDto;
            }
        }

        public bool DeleteAppointment(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@AppointmentId", id);

                connection.Execute("DeleteAppointment", parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
        }
    }
}
