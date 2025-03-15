using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ShopApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username.Trim(), model.Password, false, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Menu");
                }
                TempData["LoginError"] = "Credenciales inválidas.";
            }
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(!Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Email", "Formato de email inválido.");
                    return View(model);
                }
                if(model.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "La contraseña debe tener al menos 6 caracteres.");
                    return View(model);
                }
                if(model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
                    return View(model);
                }
                
                var user = new ApplicationUser
                {
                    UserName = model.Username.Trim(),
                    Email = model.Email.Trim(),
                    Nombre = model.Nombre.Trim(),
                    Apellidos = model.Apellidos.Trim()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Menu");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
