using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class SignUpController : Controller
{
    UsersContext db;

    public SignUpController(UsersContext context)
    {
        db = context;
    }
    /*public async Task<IActionResult> SignUpPage()
    {
        return View(await db.Users.ToArrayAsync());
    }*/

    public IActionResult SignUpPage()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return View("~/Views/Home/Index.cshtml");
    }
}

