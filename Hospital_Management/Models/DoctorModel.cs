using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Models
{
    public class DoctorModel
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor Name is required")]
        [StringLength(30)]
        [RegularExpression(@"^[A-Za-z .]+$")]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; }

        // 🔥 NEW FIELD
        public string? OtherSpecialization { get; set; }

        [Required(ErrorMessage = "Workplace is required")]
        public string WorkPlace { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 70)]
        public int Experience { get; set; }

        public List<SelectListItem>? SpecializationList { get; set; }
    }
}
