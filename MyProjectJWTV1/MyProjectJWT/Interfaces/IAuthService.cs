using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Interfaces
{
    public interface IAuthService
    {
        UserDTO AddUser(CreateUserDTO userDto);
        UserDTO GetUserById(int id);
        IEnumerable<UserDTO> GetAllUsers();

        IEnumerable<AdminDTO> AdminDoctors();
        UserDTO UpdateUser(int id, UpdateUserDTO userDto);
        bool DeleteUser(int id);
        LoginResponse Login(LoginRequest loginRequest);
       
       
       
    }
}
