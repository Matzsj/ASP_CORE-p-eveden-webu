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
                    // Uložení uživatele do databáze s využitím BCrypt pro hashování hesla
                    var newUser = new User 
                    { 
                        Username = fname,
                        Password = BCrypt.Net.BCrypt.HashPassword(Heslo) 
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


        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Přihlášení - ";
            return View();
        }

        [HttpPost]
        public IActionResult Login(string fname, string Heslo)
        {
            if(string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(Heslo))
            {
                ViewData["chyba"] = "Vyplňte všechna pole.";
                return View();
            }

            User? prihlasenyUzivatel = _context.Users.Where(u => u.Username == fname).FirstOrDefault();

            if (prihlasenyUzivatel == null)
            {
                ViewData["chyba"] = "Uživatel nenalezen.";
                return View();

            }

            // Ověření zadaného hesla pomocí BCrypt.Verify
            if (!BCrypt.Net.BCrypt.Verify(Heslo, prihlasenyUzivatel.Password))
            {
                ViewData["chyba"] = "Neplatné heslo.";
                return View();

            }

            // ÚSPĚŠNÉ PŘIHLÁŠENÍ -> ULOŽENÍ DO SESSION
            HttpContext.Session.SetString("PrihlasenyUzivatel", prihlasenyUzivatel.Username);

            return Redirect("/User/Profil");
        }




        public IActionResult Profil()
        {
            ViewData["Title"] = "Profil - ";

            // Zkusíme najít jméno v Session
            string? prihlaseneJmeno = HttpContext.Session.GetString("PrihlasenyUzivatel");

            // Pokud není žádné jméno v session, uživatel není přihlášen, vyhodíme ho na Login
            if (string.IsNullOrEmpty(prihlaseneJmeno))
            {
                return Redirect("/User/Login");
            }

            // Najdeme přihlášeného uživatele v databázi, abychom ho mohli předat do HTML
            User? uzivatelDetail = _context.Users.FirstOrDefault(u => u.Username == prihlaseneJmeno);

            if (uzivatelDetail == null)
            {
                return Redirect("/User/Login");
            }

            // Pošleme objekt uživatele do View(Profil.cshtml)
            return View(uzivatelDetail);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            return Redirect("/Home/Index"); // Nebo kamkoliv jinam, třeba i s "/"  nebo "/User/Login"
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
