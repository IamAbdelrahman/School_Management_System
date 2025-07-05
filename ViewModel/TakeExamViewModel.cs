namespace School_Management_System.ViewModel
{

    public class TakeExamViewModel
    {
        public int ExamID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public List<QuestionAnswerViewModel> Questions { get; set; } = new();
    }
}
