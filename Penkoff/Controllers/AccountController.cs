using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class AccountController : Controller
{
    UsersContext db;

    public AccountController(UsersContext context)
    {
        db = context;
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult SetRubleAccount() => View("~/Views/Account/Account.cshtml",
       new AccountViewModel
       {
           currentBalance = SetCurrency.Print(db.Users.Include(u => u.RubleAccount)
           .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
           .RubleAccount.Balance.ToString()) + " ₽"
       });

    public IActionResult SetDollarAccount() => View("~/Views/Account/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrency.Print(db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance.ToString()) + " $"
        });

    public IActionResult SetEuroAccount() => View("~/Views/Account/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrency.Print(db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance.ToString()) + " €"
        });

}
