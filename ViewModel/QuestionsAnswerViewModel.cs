namespace School_Management_System.ViewModel
{
    public class QuestionAnswerViewModel
    {
        public int QuestionID { get; set; }
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public string? StudentAnswer { get; set; }
        public int Mark { get; set; }
    }
}
