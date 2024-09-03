using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.Interfaces;
using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        [HttpGet("getallvaccines")]
        public IActionResult GetVaccines()
        {
            var vaccines = _vaccineService.GetAllVaccines();
            return Ok(vaccines);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetVaccine(int id)
        {
            try
            {
                var vaccine = _vaccineService.GetVaccineById(id);
                return Ok(vaccine);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("addvaccine")]
        public IActionResult AddVaccine([FromBody] CreateVaccineDTO createVaccineDto)
        {
            var vaccine = _vaccineService.AddVaccine(createVaccineDto);
            return CreatedAtAction(nameof(GetVaccine), new { id = vaccine.VaccineId }, vaccine);
        }

        [HttpPut("updatevaccine")]
        public IActionResult UpdateVaccine([FromBody] VaccineDTO vaccineDto)
        {
            var vaccine = _vaccineService.UpdateVaccine(vaccineDto);
            return Ok(vaccine);
        }

        [HttpDelete("deletevaccine/{id}")]
        public IActionResult DeleteVaccine(int id)
        {
            var result = _vaccineService.DeleteVaccine(id);
            return result ? (IActionResult)Ok() : NotFound();
        }
    }
}
