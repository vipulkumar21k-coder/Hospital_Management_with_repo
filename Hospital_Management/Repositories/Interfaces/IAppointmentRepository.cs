using Hospital_Management.Models;

namespace Hospital_Management.Repositories.Interfaces
{
    /// <summary>
    /// Contract for Appointment related data operations.
    /// Handles listing, filtering, pagination and CRUD.
    /// </summary>
    public interface IAppointmentRepository
    {
        List<AppointmentModel> GetAppointments(
            string searchType,
            string search,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize,
            out int totalRecords
        );

        AppointmentModel GetById(int id);

        int AddAppointment(AppointmentModel model);

        int UpdateAppointment(AppointmentModel model);

        void DeleteAppointment(int id);

        List<DoctorModel> GetDoctors();

        List<PatientModel> GetPatients();
    }
}
