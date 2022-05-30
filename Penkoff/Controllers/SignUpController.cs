using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class SignUpController : Controller
{
    UsersContext db;

    public SignUpController(UsersContext context)
    {
        db = context;
    }
    
    public IActionResult SignUpPage()
    {
        return View("~/Views/SignUp/SignUpPage.cshtml", new SignUpViewModel { result = "" });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SignUpViewModel model)
    {
        User? attempt = null;
        attempt = db.Users.FirstOrDefault(u => u.Login == model.user.Login);

        if (attempt is not null)
        {
            return View("~/Views/SignUp/SignUpPage.cshtml", new SignUpViewModel {user = model.user, result = "Account with this login already exists" });
        }

        var dollarAccount = new DollarAccount();
        var euroAccount = new EuroAccount();
        var rubleAccount = new RubleAccount();

        model.user.DollarAccount = dollarAccount;
        model.user.EuroAccount = euroAccount;
        model.user.RubleAccount = rubleAccount;

        db.Users.Add(model.user);
        await db.SaveChangesAsync();

        return View("~/Views/Home/Index.cshtml");
    }

    [HttpPost]
    public IActionResult Verification(User user)
    {
        Random rn = new();
        var verificationCode = rn.Next(100000, 999999);
        HttpContext.Session.SetInt32("verificationCode", verificationCode);

        Service.SendEmail(user.Mail, verificationCode);

        HttpContext.Session.SetString("mail", user.Mail);
        HttpContext.Session.SetString("phone", user.PhoneNumber);

        return View("~/Views/SignUp/PhoneVerification.cshtml");
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

            //return Account();
            return RedirectToAction("Account", "Login");
        }
        else
        {
            return View("~/Views/SignUp/PhoneVerification.cshtml");
        }
    }
}

