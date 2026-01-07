using Hospital_Management.Models;

namespace Hospital_Management.Repositories.Interfaces
{
    /// <summary>
    /// Contract for Patient related database operations.
    /// Controller will only depend on this interface.
    /// </summary>
    public interface IPatientRepository
    {
        List<PatientModel> GetAllPatients();
        PatientModel GetPatientById(int id);
        void AddPatient(PatientModel patient);
        void UpdatePatient(PatientModel patient);
        void DeletePatient(int id);
    }
}
