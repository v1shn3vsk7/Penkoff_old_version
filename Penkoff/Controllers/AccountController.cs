using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class AccountController : Controller
{
    private readonly IUserManager _manager;

    public AccountController(IUserManager manager)
    {
        _manager = manager;
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SetRubleAccount()
    {
        var user = await _manager.GetUserWithSpecialAccount((int)HttpContext.Session.GetInt32("Id"), "RUB");

        return View("~/Views/Account/Account.cshtml", new AccountViewModel
        {
            currentBalance = SetCurrency.Print(user.RubleAccount.Balance.ToString()) + " ₽"
        });

    }

    public async Task<IActionResult> SetDollarAccountAsync()
    {
        var user = await _manager.GetUserWithSpecialAccount((int)HttpContext.Session.GetInt32("Id"), "USD");

        return View("~/Views/Account/Account.cshtml", new AccountViewModel
        {
            currentBalance = SetCurrency.Print(user.DollarAccount.Balance.ToString()) + " $"
        });
    }

    public async Task<IActionResult> SetEuroAccountAsync()
    {
        var user = await _manager.GetUserWithSpecialAccount((int)HttpContext.Session.GetInt32("Id"), "EUR");

        return View("~/Views/Account/Account.cshtml", new AccountViewModel
        {
            currentBalance = SetCurrency.Print(user.EuroAccount.Balance.ToString()) + " €"
        });
    }

}
