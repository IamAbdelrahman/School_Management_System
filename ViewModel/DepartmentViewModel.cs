namespace School_Management_System.ViewModel
{
    /* سم - كود - المواد - اسم انستراكتور المادة - اسم مدير القسم*/
    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string CoursesNames { get; set; } // ["Math", "Science" ]

        public string? TeacherName { get; set; }

        public int StudensCount { get; set; }   
    }
}