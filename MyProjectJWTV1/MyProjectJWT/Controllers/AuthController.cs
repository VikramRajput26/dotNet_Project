using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using System;

namespace MyProjectJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest obj)
        {
            try
            {
                var response = _auth.Login(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

   

        [HttpPost("addUser")]
        public IActionResult AddUser([FromBody] CreateUserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("User data is null.");

            try
            {
                var addedUser = _auth.AddUser(userDto);
                return CreatedAtAction(nameof(GetUserById), new { id = addedUser.UserId }, addedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      

        [HttpGet("getUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _auth.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _auth.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllDoctors")]
        public IActionResult AdminDoctor()
        {
            try
            {
                var users = _auth.AdminDoctors();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDTO userDto)
        {
            if (userDto == null || userDto.UserId != id)
                return BadRequest("Invalid UserId or data is null.");

            try
            {
                var updatedUser = _auth.UpdateUser(id, userDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("deleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var success = _auth.DeleteUser(id);
                return success ? NoContent() : NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     

    }
}
