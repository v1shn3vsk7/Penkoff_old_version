﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using System.Diagnostics;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class HomeController : Controller
{
    UsersContext db;

    //private readonly ILogger<HomeController> _logger;

    /*public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }*/

    public HomeController(UsersContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Authorization()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult SignUpPage()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Account()
    {
        int userId;

        if (HttpContext.Session.GetInt32("Id") != null)
        {
            userId = (int)HttpContext.Session.GetInt32("Id");

            return View();
        }

        else
        {
            return View("~/Views/Home/Authorization.cshtml"); //direct user to auth page if he is not logged in
        }

    }

    [HttpPost]
    public ActionResult Login(User user)
    {
        User? attempt = null;

        attempt = db.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);

        if (attempt is not null)
        {
            HttpContext.Session.SetInt32("Id", attempt.Id);

            return View("~/Views/Home/Account.cshtml");
        }

        else
        {
            //return error
            return View();
        }

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
