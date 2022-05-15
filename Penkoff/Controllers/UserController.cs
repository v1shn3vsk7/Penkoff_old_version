﻿using Microsoft.AspNetCore.Mvc;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class UserController : Controller
{
    private readonly IUserManager _manager;

    UsersContext db;

    public UserController(IUserManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    [Route("user")]
    public  Task<IList<User>> GetAll() =>  _manager.GetAll();

    [HttpPut]
    [Route("user")]
    public Task Create([FromBody] CreateUserRequest request) => _manager.Create(request.Login, request.Password, request.FirstName, request.LastName);

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
 
}

