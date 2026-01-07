using Dapper;
using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Repositories.Implementations
{
    /// <summary>
    /// Handles all Appointment database operations.
    /// Includes filtering, pagination and validation checks.
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcon");
        }

        /// <summary>
        /// Returns paginated and filtered appointment list.
        /// </summary>
        public List<AppointmentModel> GetAppointments(
            string searchType,
            string search,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize,
            out int totalRecords)
        {
            using var db = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@SearchType", searchType);
            parameters.Add("@Search", search);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            using var multi = db.QueryMultiple(
                "sp_Appointment_GetAll_Filtered",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var data = multi.Read<AppointmentModel>().ToList();
            totalRecords = multi.Read<int>().Single();

            return data;
        }

        /// <summary>
        /// Returns single appointment by Id.
        /// </summary>
        public AppointmentModel GetById(int id)
        {
            using var db = new SqlConnection(_connectionString);

            return db.QueryFirstOrDefault<AppointmentModel>(
                "sp_Appointment_GetById",
                new { AppointmentId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Inserts appointment with conflict checks.
        /// Returns result code from stored procedure.
        /// </summary>
        public int AddAppointment(AppointmentModel model)
        {
            using var db = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@DoctorId", model.DoctorId);
            parameters.Add("@PatientId", model.PatientId);
            parameters.Add("@AppointmentDate", model.AppointmentDate);
            parameters.Add("@AppointmentTime", model.AppointmentTime);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            db.Execute(
                "sp_Appointment_Insert_With_Check",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@Result");
        }

        /// <summary>
        /// Updates appointment with conflict checks.
        /// </summary>
        public int UpdateAppointment(AppointmentModel model)
        {
            using var db = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentId", model.AppointmentId);
            parameters.Add("@DoctorId", model.DoctorId);
            parameters.Add("@PatientId", model.PatientId);
            parameters.Add("@AppointmentDate", model.AppointmentDate);
            parameters.Add("@AppointmentTime", model.AppointmentTime);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            db.Execute(
                "sp_Appointment_Update_With_Check",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@Result");
        }

        public void DeleteAppointment(int id)
        {
            using var db = new SqlConnection(_connectionString);

            db.Execute(
                "sp_Appointment_Delete",
                new { AppointmentId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<DoctorModel> GetDoctors()
        {
            using var db = new SqlConnection(_connectionString);

            return db.Query<DoctorModel>(
                "sp_Doctor_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public List<PatientModel> GetPatients()
        {
            using var db = new SqlConnection(_connectionString);

            return db.Query<PatientModel>(
                "sp_Patient_GetAll",
                commandType: CommandType.StoredProcedure
            ).ToList();
        }
    }
}
