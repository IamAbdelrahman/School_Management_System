using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.ViewModel
{
    public class StudentExamViewModel : IValidatableObject
    {
        public int StudentExamID { get; set; }

        [Required(ErrorMessage = "Student is required")]
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Exam is required")]
        public int? ExamID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Student grade must be a non-negative number")]
        public double? StudentGrade { get; set; }

        public double? ExamGrade { get; set; }

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Exams { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StudentGrade < 0)
            {
                yield return new ValidationResult("Student grade cannot be negative", new[] { nameof(StudentGrade) });
            }

            if (ExamGrade.HasValue && StudentGrade.HasValue && StudentGrade > ExamGrade)
            {
                yield return new ValidationResult($"Grade cannot exceed Exam grade ({ExamGrade})", new[] { nameof(StudentGrade) });
            }
        }
    }
}
