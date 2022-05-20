using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using System.Diagnostics;
using SendingEmail;

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

    public IActionResult Account()
    {
        int userId;

        if (HttpContext.Session.GetInt32("Id") != null)
        {
            userId = (int)HttpContext.Session.GetInt32("Id");

            var user = db.Users.Find(userId);

            if (user.Mail == null) //directs user to verification page if he didnt verificate email
            {
                return View("~/Views/Home/PhoneVerification.cshtml");
            }

                return View("~/Views/Home/Account.cshtml");
        }

        else
        {
            return View("~/Views/Home/Authorization.cshtml", new LoginViewModel { result=""}); //directs user to auth page if he is not logged in
        }

    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        User? attempt = null;

        attempt = db.Users.FirstOrDefault(u => u.Login == model.user.Login && u.Password == model.user.Password);

        if (attempt is not null)
        {
            HttpContext.Session.SetInt32("Id", attempt.Id);

            return Account();
        }

        else
        {
            return View("~/Views/Home/Authorization.cshtml", new LoginViewModel { user = model.user, result= "Incorrect login or password" });
        }

    }

    [HttpPost]
    public IActionResult ValidateCode()
    {
        string inputCode = Request.Form["code"];

        if (int.Parse(inputCode) == (int)HttpContext.Session.GetInt32("verificationCode"))
        {
            User currentUser = db.Users.Find((int)HttpContext.Session.GetInt32("Id")); //get current user

            currentUser.PhoneNumber = HttpContext.Session.GetString("phone");
            currentUser.Mail = HttpContext.Session.GetString("mail");
            db.SaveChanges();

            return View("~/Views/Home/Account.cshtml");
        }
        else
        {
            return View("~/Views/Home/PhoneVerification.cshtml");
        }
    }

    [HttpPost]
    public IActionResult Verification(User user)
    {
        User currentUser = db.Users.Find((int)HttpContext.Session.GetInt32("Id")); //get current user

        //currentUser.PhoneNumber = user.PhoneNumber;
        //db.SaveChanges();

        //string inputEmail = user.Mail;
        Random rn = new();
        var verificationCode = rn.Next(100000, 999999);
        HttpContext.Session.SetInt32("verificationCode", verificationCode);

        Service.SendEmail(user.Mail, verificationCode);

        HttpContext.Session.SetString("mail", user.Mail);
        HttpContext.Session.SetString("phone", user.PhoneNumber);

        return View("~/Views/Home/PhoneVerification.cshtml");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
