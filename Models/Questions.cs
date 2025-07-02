using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.Models
{
    public enum QuestionType
    {
        SingleChoice = 1,
        TrueFalse = 2
    }
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public string Body { get; set; }

        public string? Header { get; set; }

        [Range(1, 100)]
        public int Mark { get; set; }

        [Required]
        public int CorrectAnswer { get; set; }

        public QuestionType Type { get; set; }

        [ForeignKey("Exam")]
        public int ExamID { get; set; }
        public Exam Exam { get; set; }
    }

}
