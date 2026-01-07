using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Hospital_Management.Controllers
{
    /// <summary>
    /// Handles Patient UI requests.
    /// Database logic is handled by Repository.
    /// </summary>
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepo;

        public PatientController(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        /// <summary>
        /// List all patients
        /// </summary>
        public IActionResult Index()
        {
            var patients = _patientRepo.GetAllPatients();
            return View(patients);
        }

        /// <summary>
        /// Create patient - GET
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create patient - POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.AddPatient(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit patient - GET
        /// </summary>
        public IActionResult Edit(int id)
        {
            var patient = _patientRepo.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        /// <summary>
        /// Edit patient - POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.UpdatePatient(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete patient
        /// Shows alert if appointment exists
        /// </summary>
        public IActionResult Delete(int id)
        {
            try
            {
                _patientRepo.DeletePatient(id);
            }
            catch (SqlException ex)
            {
                TempData["DeleteError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
