using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using MyProjectJWT.Services;
using System.Collections.Generic;

namespace MyProjectJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("getallapt")]
        public ActionResult<List<AppointmentDTO>> GetAppointments()
        {
            var appointments = _appointmentService.GetAppointmentDetails();
            return Ok(appointments);
        }

        [HttpGet("getbyid/{id}")]
        public ActionResult<AppointmentDTO> GetAppointmentById(int id)
        {
            var appointment = _appointmentService.GetAppointmentById(id);
            return appointment != null ? Ok(appointment) : NotFound();
        }

        [HttpPost("addapt")]
        public ActionResult<AppointmentDTO> CreateAppointment([FromBody] CreateAppointmentDTO createAppointmentDto)
        {
            var appointment = _appointmentService.AddAppointment(createAppointmentDto);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentId }, appointment);
        }

        [HttpPut("updateapt/{id}")]
        public ActionResult<AppointmentDTO> UpdateAppointment(int id, [FromBody] AppointmentDTO appointmentDto)
        {
            if (id != appointmentDto.AppointmentId)
            {
                return BadRequest();
            }

            var updatedAppointment = _appointmentService.UpdateAppointment(appointmentDto);
            return Ok(updatedAppointment);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            _appointmentService.DeleteAppointment(id);
            return NoContent();
        }
    }
}
