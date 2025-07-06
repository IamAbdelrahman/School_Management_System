using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class StudentExamViewModel : IValidatableObject
    {
        public int StudentExamID { get; set; }

        [Required(ErrorMessage = "Student is required.")]
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Exam is required.")]
        public int? ExamID { get; set; }

        [Required(ErrorMessage = "Grade is required.")]
        [Display(Name = "Student Grade")]
        public double? StudentGrade { get; set; }

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Exams { get; set; } = new();

        // For validation context
        public double? MaxExamGrade { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StudentGrade < 0)
            {
                yield return new ValidationResult("Student grade cannot be negative.", new[] { nameof(StudentGrade) });
            }

            if (MaxExamGrade.HasValue && StudentGrade > MaxExamGrade.Value)
            {
                yield return new ValidationResult($"Student grade cannot exceed the exam grade ({MaxExamGrade}).", new[] { nameof(StudentGrade) });
            }
        }
    }
}
