//using Fluent.Infrastructure.FluentModel;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using School_Management_System.Models;

//namespace School_Management_System.Controllers
//{
//    public class AccountController : Controller
//    {
//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new ApplicationUser
//                {
//                    UserName = model.Email,
//                    Email = model.Email
//                };

//                var result = await _userManager.CreateAsync(user, model.Password);

//                if (result.Succeeded)
//                {
//                    // 👇 هنا نضيفه في Role
//                    await _userManager.AddToRoleAsync(user, "Student");

//                    // 👇 نسجل دخوله على طول بعد التسجيل
//                    await _signInManager.SignInAsync(user, isPersistent: false);

//                    return RedirectToAction("Index", "Student"); // يروح على Dashboard
//                }

//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }

//            return View(model);
//        }

//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

//            if (result.Succeeded)
//            {
//                var user = await _userManager.FindByEmailAsync(model.Email);
//                var roles = await _userManager.GetRolesAsync(user);

//                if (roles.Contains("Admin"))
//                    return RedirectToAction("Index", "Admin");

//                if (roles.Contains("Teacher"))
//                    return RedirectToAction("Index", "Teacher");

//                if (roles.Contains("Student"))
//                    return RedirectToAction("Index", "Student");

//                return RedirectToAction("Index", "Home");
//            }

//            ModelState.AddModelError("", "Invalid login attempt.");
//            return View(model);
//        }


//    }
//}
