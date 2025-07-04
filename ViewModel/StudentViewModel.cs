using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.ViewModel
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(30, ErrorMessage = "Name Can't be more that 30 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters or spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Age")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Age must be a number")]
        public int? Age { get; set; }

        public string? Gender { get; set; }

        [StringLength(30, ErrorMessage = "Addrees Can't exceed 30 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression(@"^\+?[0-9]{11}$", ErrorMessage = "Phone must be exactly 11 digits with optional '+'")]
        public string? Phone { get; set; }

        //Foreign Key
        [Display(Name ="Class Name")]
        public int? ClassID { get; set; }

        public List<SelectListItem>? Classes { get; set; }

        public string? ClassName { get; set; }


    }
}
