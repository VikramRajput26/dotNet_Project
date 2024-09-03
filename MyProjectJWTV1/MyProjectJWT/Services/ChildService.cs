using MyProjectJWT.Interfaces;
using MyProjectJWT.DTO;
using MyProjectJWT.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MyProjectJWT.Services
{
    public class ChildService : IChildService
    {
        private readonly string _connectionString;

        public ChildService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Database");
        }

        public List<ChildDTO> GetChildDetails()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var children = connection.Query<ChildDTO>("GetAllChildren", commandType: CommandType.StoredProcedure).ToList();
                return children;
            }
        }

        public ChildDTO GetChildById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@ChildId", id);

                var child = connection.QuerySingleOrDefault<ChildDTO>("GetChildById", parameters, commandType: CommandType.StoredProcedure);
                if (child == null)
                {
                    throw new Exception("Child not found");
                }

                return child;
            }
        }

        public ChildDTO AddChild(CreateChildDTO createChildDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("FirstName", createChildDto.FirstName);
                parameters.Add("LastName", createChildDto.LastName);
                parameters.Add("BloodType", createChildDto.BloodType);
                parameters.Add("Gender", createChildDto.Gender.ToString());
                parameters.Add("DateOfBirth", createChildDto.DateOfBirth);
                parameters.Add("UserId", createChildDto.UserId);

                parameters.Add("@ChildId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("AddChild", parameters, commandType: CommandType.StoredProcedure);

                var newChildId = parameters.Get<int>("@ChildId");

                return new ChildDTO
                {
                    ChildId = newChildId,
                    FirstName = createChildDto.FirstName,
                    LastName = createChildDto.LastName,
                    BloodType = createChildDto.BloodType,
                    Gender = createChildDto.Gender,
                    DateOfBirth = createChildDto.DateOfBirth,
                    UserId = createChildDto.UserId
                };
            }
        }

        public ChildDTO UpdateChild(ChildDTO childDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("ChildId", childDto.ChildId);
                parameters.Add("FirstName", childDto.FirstName);
                parameters.Add("LastName", childDto.LastName);
                parameters.Add("BloodType", childDto.BloodType);
                parameters.Add("Gender", childDto.Gender.ToString());
                parameters.Add("DateOfBirth", childDto.DateOfBirth);
                parameters.Add("UserId", childDto.UserId);

                connection.Execute("UpdateChild", parameters, commandType: CommandType.StoredProcedure);

                return childDto;
            }
        }

        public bool DeleteChild(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@ChildId", id);

                connection.Execute("DeleteChild", parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
        }
    }
}
