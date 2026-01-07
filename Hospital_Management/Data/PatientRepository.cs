using Dapper;
using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Repositories.Implementations
{
    /// <summary>
    /// Handles all Patient database operations.
    /// Stored procedures + SQL logic lives here.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcon");
        }

        /// <summary>
        /// Returns all patients.
        /// </summary>
        public List<PatientModel> GetAllPatients()
        {
            using var db = new SqlConnection(_connectionString);

            return db.Query<PatientModel>(
                "sp_Patient_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        /// <summary>
        /// Returns a single patient by Id.
        /// </summary>
        public PatientModel GetPatientById(int id)
        {
            using var db = new SqlConnection(_connectionString);

            return db.QueryFirstOrDefault<PatientModel>(
                "SELECT * FROM Patients WHERE PatientId = @id",
                new { id }
            );
        }

        /// <summary>
        /// Inserts a new patient.
        /// </summary>
        public void AddPatient(PatientModel patient)
        {
            using var db = new SqlConnection(_connectionString);

            db.Execute(
                "sp_Patient_Insert",
                patient,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Updates existing patient details.
        /// </summary>
        public void UpdatePatient(PatientModel patient)
        {
            using var db = new SqlConnection(_connectionString);

            db.Execute(
                "sp_Patient_Update",
                patient,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Deletes patient by Id.
        /// </summary>
        public void DeletePatient(int id)
        {
            using var db = new SqlConnection(_connectionString);

            db.Execute(
                "sp_Patient_Delete",
                new { PatientId = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
