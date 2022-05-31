using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class MyCardsController : Controller
{
    private readonly IUserManager _manager;

    public MyCardsController(IUserManager manager)
    {
        _manager = manager;
    }

    public async Task<IActionResult> MyCards()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return RedirectToAction("Account", "Login");
        }

        var user = await _manager.GetUserWithCards((int)userId);

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }
}

