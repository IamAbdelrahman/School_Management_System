using Microsoft.AspNetCore.Mvc.Rendering;
using School_Management_System.Models;
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

        [Required(ErrorMessage = "Please Enter Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [StringLength(30, ErrorMessage = "Addrees Can't exceed 30 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression(@"^\+?[0-9]{11}$", ErrorMessage = "Phone must be exactly 11 digits with optional '+'")]
        public string? Phone { get; set; }

        /*ForeignKeys*/
        [ForeignKey("Class")]
        [Display(Name = "Class")]
        public int? ClassID { get; set; }
        public Class? Class { get; set; }


    }
}
