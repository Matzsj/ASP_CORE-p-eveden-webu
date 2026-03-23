using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Title"] = "Registrace - ";
            return View();
        }

        [HttpPost]
        public IActionResult Register(string fname, string email, string Heslo, string Hesloznovu)
        {
            if(fname != "" && Heslo != "" && Hesloznovu != "" && email != "")
            {
                if (Heslo == Hesloznovu)
                {
                    // Uložení uživatele do databáze
                    var newUser = new User 
                    { 
                        Username = fname,
                        Password = Heslo // Případně můžete přidat i email, pokud jej přidáte do Models\User.cs
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

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


        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewData["chyba"] = "Vyplňte všechna pole.";
                return View();
            }

            User? prihlasenyUzivatel = _context.Users.Where(u => u.Username == Username).FirstOrDefault();


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
