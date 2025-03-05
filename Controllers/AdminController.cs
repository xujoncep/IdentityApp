using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        public AdminController( UserManager<IdentityUser> userManager)
        {
            _userManager = userManager; 
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        public IActionResult All()
        {
            var data = _userManager.Users.ToList();
             return Json(data);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, string password)
        {
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }

            else
            {
                IdentityUser appUser = new IdentityUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, password);

                if (result.Succeeded == true)
                {
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                
            }

            return View(user);
        }
    }
}
