using Dapper;
using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Repositories.Implementations
{
    /// <summary>
    /// Concrete implementation of IDoctorRepository.
    ///All database & stored procedure logic lives here.
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcon");
        }

        /// <summary>
        /// Fetch all doctors from database.
        /// </summary>
        public List<DoctorModel> GetAllDoctors()
        {
            using var db = new SqlConnection(_connectionString);
            return db.Query<DoctorModel>(
                "sp_Doctor_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        /// <summary>
        /// Fetch single doctor by Id.
        /// </summary>
        public DoctorModel GetDoctorById(int id)
        {
            using var db = new SqlConnection(_connectionString);
            return db.QueryFirstOrDefault<DoctorModel>(
                "SELECT * FROM Doctors WHERE DoctorId = @id",
                new { id }
            );
        }

        /// <summary>
        /// Insert new doctor record.
        /// </summary>
        public void AddDoctor(DoctorModel doctor)
        {
            using var db = new SqlConnection(_connectionString);

            var param = new DynamicParameters();
            param.Add("@DoctorName", doctor.DoctorName);
            param.Add("@Specialization", doctor.Specialization);
            param.Add("@WorkPlace", doctor.WorkPlace);
            param.Add("@Experience", doctor.Experience);

            db.Execute(
                "sp_Doctor_Insert",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Update existing doctor record.
        /// </summary>
        public void UpdateDoctor(DoctorModel doctor)
        {
            using var db = new SqlConnection(_connectionString);

            var param = new DynamicParameters();
            param.Add("@DoctorId", doctor.DoctorId);
            param.Add("@DoctorName", doctor.DoctorName);
            param.Add("@Specialization", doctor.Specialization);
            param.Add("@WorkPlace", doctor.WorkPlace);
            param.Add("@Experience", doctor.Experience);

            db.Execute(
                "sp_Doctor_Update",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Delete doctor by Id.
        /// </summary>
        public void DeleteDoctor(int id)
        {
            using var db = new SqlConnection(_connectionString);

            db.Execute(
                "sp_Doctor_Delete",
                new { DoctorId = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
