using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.Interfaces;
using MyProjectJWT.DTO;

namespace MyProjectJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChildController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpGet("getallchild")]
        public IActionResult GetChildren()
        {
            var children = _childService.GetChildDetails();
            return Ok(children);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetChild(int id)
        {
            try
            {
                var child = _childService.GetChildById(id);
                return Ok(child);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("addchild")]
        public IActionResult AddChild([FromBody] CreateChildDTO childDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdChild = _childService.AddChild(childDto);
            return CreatedAtAction(nameof(GetChild), new { id = createdChild.ChildId }, createdChild);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateChild(int id, [FromBody] ChildDTO childDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != childDto.ChildId)
                return BadRequest("Child ID mismatch");

            try
            {
                var updatedChild = _childService.UpdateChild(childDto);
                return Ok(updatedChild);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteChild(int id)
        {
            try
            {
                var result = _childService.DeleteChild(id);
                if (result)
                    return NoContent();

                return BadRequest("Failed to delete child");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
