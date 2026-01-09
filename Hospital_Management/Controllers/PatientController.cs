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

        // ===================== INDEX =====================
        public IActionResult Index(string search, int page = 1)
        {
            int pageSize = 5;

            var patients = _patientRepo.GetAllPatients();

            if (!string.IsNullOrWhiteSpace(search))
            {
                patients = patients
                    .Where(x =>
                        x.PatientName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.Contact.Contains(search))
                    .ToList();
            }

            // 🔢 PAGINATION
            int totalRecords = patients.Count;

            var pagedPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.Search = search;

            return View(pagedPatients);
        }

        // ===================== CREATE (GET) =====================
        public IActionResult Create()
        {
            return View();
        }

        // ===================== CREATE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.AddPatient(model);
            return RedirectToAction(nameof(Index));
        }

        // ===================== EDIT (GET) =====================
        public IActionResult Edit(int id)
        {
            var patient = _patientRepo.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // ===================== EDIT (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _patientRepo.UpdatePatient(model);
            return RedirectToAction(nameof(Index));
        }

        // ===================== DELETE =====================
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
