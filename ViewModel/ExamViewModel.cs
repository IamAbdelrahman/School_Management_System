using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class ExamViewModel
    {
        public int ExamID { get; set; }

        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Grade must be between 1 and 100.")]
        public double Grade { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        public int? CourseID { get; set; }

        public List<SelectListItem> Courses { get; set; } = new();
    }
}
