using Microsoft.AspNetCore.Mvc;

namespace School_Management_System.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
