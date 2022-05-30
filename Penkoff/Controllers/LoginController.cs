using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class LoginController : Controller
{
    UsersContext db;

    public LoginController(UsersContext context)
    {
        db = context;
    }

    public IActionResult Account()
    {
        int userId;

        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return View("~/Views/Authorization/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        userId = (int)HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.RubleAccount)
            .Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == userId);

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
