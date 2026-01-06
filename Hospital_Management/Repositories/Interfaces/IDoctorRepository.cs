using Hospital_Management.Models;

namespace Hospital_Management.Repositories.Interfaces
{
    /// <summary>
    /// Contract for Doctor data operations.
    /// Controller sirf isi interface se baat karega.
    /// </summary>
    public interface IDoctorRepository
    {
        List<DoctorModel> GetAllDoctors();
        DoctorModel GetDoctorById(int id);
        void AddDoctor(DoctorModel doctor);
        void UpdateDoctor(DoctorModel doctor);
        void DeleteDoctor(int id);
    }
}
