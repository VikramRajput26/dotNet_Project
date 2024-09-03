using MyProjectJWT.DTO;
using MyProjectJWT.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MyProjectJWT.Interfaces;

namespace MyProjectJWT.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly string _connectionString;

        public VaccineService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Database");
        }

        public List<VaccineDTO> GetAllVaccines()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var vaccines = connection.Query<VaccineDTO>("GetAllVaccines", commandType: CommandType.StoredProcedure).ToList();
                return vaccines;
            }
        }

        public VaccineDTO GetVaccineById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@VaccineId", id);

                var vaccine = connection.QuerySingleOrDefault<VaccineDTO>("GetVaccineById", parameters, commandType: CommandType.StoredProcedure);
                if (vaccine == null)
                {
                    throw new Exception("Vaccine not found");
                }

                return vaccine;
            }
        }

        public VaccineDTO AddVaccine(CreateVaccineDTO createVaccineDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@VaccineName", createVaccineDto.VaccineName);
                parameters.Add("@Description", createVaccineDto.Description);
                parameters.Add("@RecommendedAge", createVaccineDto.RecommendedAge);
                parameters.Add("@AgeUnit", createVaccineDto.AgeUnit.ToString());  // Convert enum to string
                parameters.Add("@SideEffects", createVaccineDto.SideEffects);

                parameters.Add("@VaccineId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("AddVaccine", parameters, commandType: CommandType.StoredProcedure);

                var newVaccineId = parameters.Get<int>("@VaccineId");

                return new VaccineDTO
                {
                    VaccineId = newVaccineId,
                    VaccineName = createVaccineDto.VaccineName,
                    Description = createVaccineDto.Description,
                    RecommendedAge = createVaccineDto.RecommendedAge,
                    AgeUnit = createVaccineDto.AgeUnit,
                    SideEffects = createVaccineDto.SideEffects
                };
            }
        }

        public VaccineDTO UpdateVaccine(VaccineDTO vaccineDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@VaccineId", vaccineDto.VaccineId);
                parameters.Add("@VaccineName", vaccineDto.VaccineName);
                parameters.Add("@Description", vaccineDto.Description);
                parameters.Add("@RecommendedAge", vaccineDto.RecommendedAge);
                parameters.Add("@AgeUnit", vaccineDto.AgeUnit.ToString());  // Convert enum to string
                parameters.Add("@SideEffects", vaccineDto.SideEffects);

                connection.Execute("UpdateVaccine", parameters, commandType: CommandType.StoredProcedure);

                return vaccineDto;
            }
        }

        public bool DeleteVaccine(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@VaccineId", id);

                connection.Execute("DeleteVaccine", parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
        }
    }
}
