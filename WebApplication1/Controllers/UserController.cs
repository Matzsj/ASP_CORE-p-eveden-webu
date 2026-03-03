using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Register()
        {
            ViewData["Title"] = "Registrace - ";
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Přihlášení - ";
            return View();
        }

        public IActionResult Profil()
        {
            ViewData["Title"] = "Profil - ";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
