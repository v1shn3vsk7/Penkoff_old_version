using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class LoginController : Controller
{
    private readonly IUserManager _manager;

    public LoginController(IUserManager manager)
    {
        _manager = manager;
    }

    public async Task<IActionResult> Account()
    {
        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return View("~/Views/Authorization/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithAccounts(userId);

        if (user.Mail == null) //directs user to verification page if he didnt verificate email
        {
            return View("~/Views/SignUp/PhoneVerification.cshtml");
        }
        else
        {
            return View("~/Views/Account/Account.cshtml",
            new AccountViewModel
            {
                rubleAccount = user.RubleAccount,
                dollarAccount = user.DollarAccount,
                euroAccount = user.EuroAccount,
                currentBalance = SetCurrency.Print(user.RubleAccount.Balance.ToString()) + " ₽",
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        User? attempt = null;
        attempt = await _manager.GetUser(model.user.Login, model.user.Password);

        if (attempt is not null)
        {
            HttpContext.Session.SetInt32("Id", attempt.Id);

            return RedirectToAction("Account", "Login");
        }
        else
        {
            return View("~/Views/Authorization/Authorization.cshtml", new LoginViewModel
            {
                user = model.user,
                result = "Incorrect login or password"
            });
        }

    }

    public IActionResult Authorization()
    {
        return View();
    }
}
