using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentExpenseTracker.Models;

namespace StudentExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        // constructor injection to get instances of SignInManager and UserManager
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        //  GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // attempt to sign in the user using provided username and password
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    // if sign-in succeeds, redirent to the home page
                    return RedirectToAction("Index", "Home");
                }

                // if sign-in fails, add an error message to the ModelState
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
			return View();
		}

        // POST: /Account/Register
        [HttpPost]
		public async Task<IActionResult> Register(Register model)
		{
            if (ModelState.IsValid)
            {
                // create a new user with provided information
                User user = new()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                };

                // attempt to create the user in the database
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // if user creation succeeds, sign in the user and redirect to the home page
                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }

                // if user creation fails, add error descriptions to the ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
			return View();
		}

        // POST: /Account/Logout
		public async Task<IActionResult> Logout()
        {
            // sign out the currently authenticated user
            await signInManager.SignOutAsync();

            // redirect to the home page after signing out
            return RedirectToAction("Index", "Home");
        }
    }
}
