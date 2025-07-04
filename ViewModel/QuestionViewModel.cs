using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
using System.ComponentModel.DataAnnotations;

public class QuestionViewModel : IValidatableObject
{
    public int QuestionID { get; set; }

    [Required]
    public string Body { get; set; }

    public string? Header { get; set; }

    [Range(1, 100, ErrorMessage = "Mark must be between 1 and 100.")]
    public int Mark { get; set; }

    [Required]
    public int CorrectAnswer { get; set; }

    [Required]
    public QuestionType Type { get; set; }

    [Required(ErrorMessage = "Please select an exam")]
    public int? ExamID { get; set; }

    public List<SelectListItem> Exams { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CorrectAnswer < 1)
        {
            yield return new ValidationResult("CorrectAnswer cannot be negative number.", new[] { nameof(CorrectAnswer) });
        }

        if (Type == QuestionType.TrueFalse && (CorrectAnswer != 1 && CorrectAnswer != 2))
        {
            yield return new ValidationResult("For True/False questions, CorrectAnswer must be 1 (True) or 2 (False).", new[] { nameof(CorrectAnswer) });
        }

        if (Type == QuestionType.SingleChoice && (CorrectAnswer < 1 || CorrectAnswer > 3))
        {
            yield return new ValidationResult("For Single Choice questions, CorrectAnswer must be 1, 2, or 3.", new[] { nameof(CorrectAnswer) });
        }
    }
}
