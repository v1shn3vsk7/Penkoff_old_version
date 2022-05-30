using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class MyCardsController : Controller
{
    UsersContext db;

    public MyCardsController(UsersContext context)
    {
        db = context;
    }

    public IActionResult MyCards()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            //return Account(); 
            return RedirectToAction("Account", "Login");
        }

        var user = db.Users.Include(u => u.Cards).FirstOrDefault(u => u.Id == userId);

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }
}

