using Microsoft.AspNetCore.Mvc;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using System.Diagnostics;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class HomeController : Controller
{
    UsersContext db;

    //private readonly ILogger<HomeController> _logger;

    /*public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }*/

    public HomeController(UsersContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Authorization()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult SignUpPage()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]

    public ActionResult Login(User user)
    {
        if (user.Login == null || user.Password == null || //check for empty fields
            (user.Login == null && user.Password == null))
        {
            //print error
        }


        User? attemp = null;

        attemp = db.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);

        if (attemp is not null)
        {
            //FormsAuthentication.SetAuthCookie(model.Name, true);
            return View("~/Views/Home/Index.cshtml"); //сделать переход в аккаунт
        }

        else
        {
            //return error
            return View();
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
