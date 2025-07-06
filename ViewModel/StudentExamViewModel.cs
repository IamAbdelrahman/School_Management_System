using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class StudentExamViewModel : IValidatableObject
    {
        public int StudentExamID { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int? StudentID { get; set; }

        [Required]
        [Display(Name = "Exam")]
        public int? ExamID { get; set; }

        [Display(Name = "Student Grade")]
        [Range(0, double.MaxValue, ErrorMessage = "Grade must be a positive number.")]
        public double? StudentGrade { get; set; }

        public double? ExamGrade { get; set; } // set in controller before validation

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Exams { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StudentGrade.HasValue && ExamGrade.HasValue && StudentGrade > ExamGrade)
            {
                yield return new ValidationResult(
                    $"Student grade ({StudentGrade}) cannot exceed exam grade ({ExamGrade}).",
                    new[] { nameof(StudentGrade) });
            }
        }
    }
}
