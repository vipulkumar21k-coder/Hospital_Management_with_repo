using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Hospital_Management.Controllers
{
    /// <summary>
    /// Handles Doctor related UI requests.
    /// Database logic is handled by Repository.
    /// </summary>
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorController(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            var doctors = _doctorRepo.GetAllDoctors();
            return View(doctors);
        }

        // ===================== CREATE (GET) =====================
        public IActionResult Create()
        {
            return View(new DoctorModel
            {
                SpecializationList = GetSpecializations()
            });
        }

        // ===================== CREATE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoctorModel model)
        {
            // 🔹 OTHER specialization handling
            if (model.Specialization == "Other")
            {
                if (string.IsNullOrWhiteSpace(model.OtherSpecialization))
                {
                    ModelState.AddModelError(
                        nameof(model.OtherSpecialization),
                        "Please enter specialization"
                    );
                }
                else
                {
                    model.Specialization = model.OtherSpecialization.Trim();
                }
            }

            if (!ModelState.IsValid)
            {
                model.SpecializationList = GetSpecializations();
                return View(model);
            }

            _doctorRepo.AddDoctor(model);
            return RedirectToAction(nameof(Index));
        }

        // ===================== EDIT (GET) =====================
        public IActionResult Edit(int id)
        {
            var doctor = _doctorRepo.GetDoctorById(id);
            if (doctor == null)
                return NotFound();

            doctor.SpecializationList = GetSpecializations();
            return View(doctor);
        }

        // ===================== EDIT (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoctorModel model)
        {
            // 🔹 OTHER specialization handling
            if (model.Specialization == "Other")
            {
                if (string.IsNullOrWhiteSpace(model.OtherSpecialization))
                {
                    ModelState.AddModelError(
                        nameof(model.OtherSpecialization),
                        "Please enter specialization"
                    );
                }
                else
                {
                    model.Specialization = model.OtherSpecialization.Trim();
                }
            }

            if (!ModelState.IsValid)
            {
                model.SpecializationList = GetSpecializations();
                return View(model);
            }

            _doctorRepo.UpdateDoctor(model);
            return RedirectToAction(nameof(Index));
        }

        // ===================== DELETE =====================
        public IActionResult Delete(int id)
        {
            try
            {
                _doctorRepo.DeleteDoctor(id);
            }
            catch (SqlException ex)
            {
                TempData["DeleteError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ===================== SPECIALIZATION DROPDOWN =====================
        private List<SelectListItem> GetSpecializations()
        {
            return new()
            {
                new SelectListItem { Text = "Cardiologist", Value = "Cardiologist" },
                new SelectListItem { Text = "Dermatologist", Value = "Dermatologist" },
                new SelectListItem { Text = "Neurologist", Value = "Neurologist" },
                new SelectListItem { Text = "Orthopedic", Value = "Orthopedic" },
                new SelectListItem { Text = "Pediatrician", Value = "Pediatrician" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };
        }
    }
}
