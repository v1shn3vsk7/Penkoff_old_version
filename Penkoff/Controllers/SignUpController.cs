using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class SignUpController : Controller
{
    private readonly IUserManager _manager;

    public SignUpController(IUserManager manager)
    {
        _manager = manager;
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
        attempt = await _manager.FindUser(model.user.Login);

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

        await _manager.AddUser(model.user);

        return View("~/Views/SignUp/PhoneVerification.cshtml");
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
    public async Task<IActionResult> ValidateCode()
    {
        string inputCode = Request.Form["code"];

        if (inputCode.Length == 0)
        {
            return View("~/Views/SignUp/PhoneVerification.cshtml");
        }

        if (int.Parse(inputCode) == (int)HttpContext.Session.GetInt32("verificationCode"))
        {
            var currentUser = await _manager.GetUser((int)HttpContext.Session.GetInt32("Id"));

            var Email = HttpContext.Session.GetString("mail");
            var PhoneNumber = HttpContext.Session.GetString("phone");

            await _manager.AddPhoneAndEmail(currentUser, PhoneNumber, Email);

            return RedirectToAction("Account", "Login");
        }
        else
        {
            return View("~/Views/SignUp/PhoneVerification.cshtml");
        }
    }
}

