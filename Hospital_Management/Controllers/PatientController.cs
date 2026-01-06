using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    /// <summary>
    /// Handles Patient UI requests.
    /// No database or SQL logic here.
    /// </summary>
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepo;

        public PatientController(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        /// <summary>
        /// List all patients.
        /// </summary>
        public IActionResult Index()
        {
            var patients = _patientRepo.GetAllPatients();
            return View(patients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.AddPatient(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var patient = _patientRepo.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.UpdatePatient(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _patientRepo.DeletePatient(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
