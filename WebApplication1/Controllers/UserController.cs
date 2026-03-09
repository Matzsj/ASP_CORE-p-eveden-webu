using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
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

        public IActionResult Register(string fname, string email, string Heslo, string Hesloznovu)
        {
            if(fname != "" && Heslo != "" && Hesloznovu != "" && email != "")
            {
                if (Heslo == Hesloznovu)
                {
                    return Redirect("/User/Login");
                }
                else
                {
                    ViewData["chyba"] = "Hesla se neshodují.";
                }
            }
            else
            {
                ViewData["chyba"] = "Vyplňte všechna pole.";
            }

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
