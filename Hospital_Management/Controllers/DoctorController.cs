using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital_Management.Controllers
{
    /// <summary>
    /// Handles Doctor related UI requests.
    /// No database logic here.
    /// </summary>
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorController(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        public IActionResult Index()
        {
            var doctors = _doctorRepo.GetAllDoctors();
            return View(doctors);
        }

        public IActionResult Create()
        {
            return View(new DoctorModel
            {
                SpecializationList = GetSpecializations()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoctorModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SpecializationList = GetSpecializations();
                return View(model);
            }

            _doctorRepo.AddDoctor(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var doctor = _doctorRepo.GetDoctorById(id);
            if (doctor == null) return NotFound();

            doctor.SpecializationList = GetSpecializations();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoctorModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SpecializationList = GetSpecializations();
                return View(model);
            }

            _doctorRepo.UpdateDoctor(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _doctorRepo.DeleteDoctor(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Dropdown specialization list.
        /// </summary>
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
