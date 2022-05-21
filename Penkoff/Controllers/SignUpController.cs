using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

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

        db.Users.Add(model.user);
        await db.SaveChangesAsync();

        return View("~/Views/Home/Index.cshtml");
    }
}

