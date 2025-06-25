using Microsoft.AspNetCore.Mvc;

namespace School_Management_System.Controllers
{
    public class ExamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
