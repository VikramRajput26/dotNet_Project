using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using MyProjectJWT.Context;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using MyProjectJWT.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MyProjectJWT.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly string _connectionString;

        public AuthService(JwtContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _connectionString = _configuration.GetConnectionString("Database");
        }

        public UserDTO AddUser(CreateUserDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            // Check if a user with the same email already exists
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // Hash the password
            var hashedPassword = HashPassword(userDto.Password);

            // Call the stored procedure to add the user
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the input parameters
                var parameters = new DynamicParameters();
                parameters.Add("UserFirstName", userDto.FirstName);
                parameters.Add("UserLastName", userDto.LastName);
                parameters.Add("UserEmail", userDto.Email);
                parameters.Add("UserPassword", hashedPassword);
                parameters.Add("UserContact", userDto.ContactNumber);
                parameters.Add("UserRole", userDto.UserRole.ToString());

                // Define the output parameter
                parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Call the stored procedure with both input and output parameters
                connection.Execute("AddUsers", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve the UserId from the output parameter
                var newUserId = parameters.Get<int>("@UserId");

                // Send registration success email
                string subject = "Registration Successful";
                string body = $"Hello {userDto.FirstName},<br/><br/>" +
                              "Thank you for registering. Your registration was successful!<br/><br/>" +
                              "Best regards,<br/>The Team";
                _emailService.SendEmail(userDto.Email, subject, body);

                // Convert the user entity to a UserDTO and return it
                return new UserDTO
                {
                    UserId = newUserId, // Use the newly generated UserId
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    ContactNumber = userDto.ContactNumber,
                    UserRole = userDto.UserRole
                };
            }
        }




        public UserDTO GetUserById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the input parameter
                var parameters = new DynamicParameters();
                parameters.Add("@User_Id", id);

                // Call the stored procedure to get the user by ID
                var user = connection.QuerySingleOrDefault<UserDTO>("GetUserById", parameters, commandType: CommandType.StoredProcedure);

                if (user == null)
                    throw new Exception("User not found");

                return user;
            }
        }


        public IEnumerable<UserDTO> GetAllUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Call the stored procedure to get all users
                var users = connection.Query<UserDTO>("GetAllUsers", commandType: CommandType.StoredProcedure);

                return users.ToList();
            }
        }


        public UserDTO UpdateUser(int id, UpdateUserDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Hash the password if it's provided in the update request
                string hashedPassword = null;
                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    hashedPassword = HashPassword(userDto.Password);
                }

                // Define the input parameters
                var parameters = new DynamicParameters();
                parameters.Add("NewUserId", id); // Ensure you're passing the correct UserId to update
                parameters.Add("UserFirstName", userDto.FirstName);
                parameters.Add("UserLastName", userDto.LastName);
                parameters.Add("UserEmail", userDto.Email);
                parameters.Add("UserPassword", hashedPassword ?? (object)DBNull.Value); // Use existing password if not updating
                parameters.Add("UserContact", userDto.ContactNumber);
                parameters.Add("UserRole", userDto.UserRole.ToString());

                // Call the stored procedure to update the user
                connection.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                // Fetch the updated user to return
                var updatedUser = GetUserById(id);

                return updatedUser;
            }
        }



        public bool DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the input parameter
                var parameters = new DynamicParameters();
                parameters.Add("@User_Id", id);

                // Call the stored procedure to delete the user
                connection.Execute("DeleteUser", parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
        }


        public LoginResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                throw new ArgumentNullException(nameof(loginRequest));

            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                throw new Exception("Credentials are not valid");

            var user = _context.Users.SingleOrDefault(s => s.Email == loginRequest.Username);
            if (user == null || !VerifyPassword(user.Password, loginRequest.Password))
                throw new Exception("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.UserId
            };
        }

        private static string HashPassword(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashed)}";
        }

        private static bool VerifyPassword(string storedPassword, string providedPassword)
        {
            var parts = storedPassword.Split('.');
            if (parts.Length != 2)
                throw new InvalidOperationException("Invalid password format");

            var storedSalt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var providedHash = KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            return providedHash.SequenceEqual(storedHash);
        }

        public IEnumerable<AdminDTO> AdminDoctors()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                 var users = connection.Query<AdminDTO>("AdminDoctor", commandType: CommandType.StoredProcedure);

                return users.ToList();
            }
        }
    }
}
