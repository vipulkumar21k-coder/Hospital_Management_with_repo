using Hospital_Management.Models;
using Hospital_Management.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    /// <summary>
    /// Handles Appointment UI requests.
    /// All DB logic is delegated to Repository.
    /// </summary>
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _repo;
        private const int PageSize = 10;

        public AppointmentController(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index(
            string searchType,
            string search,
            DateTime? fromDate,
            DateTime? toDate,
            int page = 1)
        {
            var data = _repo.GetAppointments(
                searchType,
                search,
                fromDate,
                toDate,
                page,
                PageSize,
                out int totalRecords
            );

            ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            ViewBag.CurrentPage = page;

            ViewBag.SearchType = searchType;
            ViewBag.Search = search;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            ViewBag.Doctors = _repo.GetDoctors();
            ViewBag.Patients = _repo.GetPatients();

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.Doctors = _repo.GetDoctors();
            ViewBag.Patients = _repo.GetPatients();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppointmentModel model)
        {
            int result = _repo.AddAppointment(model);

            if (result == -1)
                ModelState.AddModelError("", "This doctor is already booked.");
            else if (result == -2)
                ModelState.AddModelError("", "This patient already has an appointment.");

            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _repo.GetDoctors();
                ViewBag.Patients = _repo.GetPatients();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var appointment = _repo.GetById(id);
            if (appointment == null) return NotFound();

            ViewBag.Doctors = _repo.GetDoctors();
            ViewBag.Patients = _repo.GetPatients();
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppointmentModel model)
        {
            int result = _repo.UpdateAppointment(model);

            if (result == -1)
                ModelState.AddModelError("", "This doctor is already booked.");
            else if (result == -2)
                ModelState.AddModelError("", "This patient already has an appointment.");

            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _repo.GetDoctors();
                ViewBag.Patients = _repo.GetPatients();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteAppointment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
