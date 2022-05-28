using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using System.Diagnostics;
using SendingEmail;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public IActionResult Services()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return View("~/Views/Home/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        /*var user = db.Users.Include(u => u.RubleAccount)
           .Include(u => u.DollarAccount)
           .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == userId);

        IEnumerable<Currency> currencies = new List<Currency>
        {
        new Currency {Balance=user.RubleAccount.Balance, CurrencyType="RUB"},
        new Currency {Balance=user.DollarAccount.Balance, CurrencyType="USD"},
        new Currency {Balance=user.EuroAccount.Balance, CurrencyType="EUR"}
        };

        ViewBag.Currency = new SelectList(currencies, "Balance", "CurrencyType");*/

        return View();
    }

    public IActionResult SendMoney()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.RubleAccount)
           .Include(u => u.DollarAccount)
           .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == userId);

        /*IEnumerable<Currency> currencies = new List<Currency>
        {
        new Currency {Balance=user.RubleAccount.Balance, CurrencyType="RUB"},
        new Currency {Balance=user.DollarAccount.Balance, CurrencyType="USD"},
        new Currency {Balance=user.EuroAccount.Balance, CurrencyType="EUR"}
        };*/

       /* ViewBag.Currency = new SelectList(currencies, "Balance", "CurrencyType");*/

        return View(new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance,
            CurrencyPick= "₽",
            Result =""
        });
    }

    public IActionResult ChangeToRubleAccount(SendMoneyViewModel model)
    {
        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance,
            CurrencyPick= " ₽",
            Result = ""
        });
    }

    public IActionResult ChangeToDollarAccount(SendMoneyViewModel model)
    {
        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance,
            CurrencyPick = " $",
            Result = ""
        });
    }

    public IActionResult ChangeToEuroAccount(SendMoneyViewModel model)
    {
        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance,
            CurrencyPick = " €",
            Result = ""
        });
    }

    public IActionResult Authorization()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    /*public IActionResult SignUpPage()
    {
        return View("~/Views/Home/SignUp/SignUpPage.cshtml", new SignUpViewModel { result = "" });
    }*/
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Account()
    {
        int userId;

        if (HttpContext.Session.GetInt32("Id") == null)
        {
            return View("~/Views/Home/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        userId = (int)HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.RubleAccount)
            .Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == userId);

        if (user.Mail == null) //directs user to verification page if he didnt verificate email
        {
            return View("~/Views/Home/PhoneVerification.cshtml");
        }
        else
        {
            return View("~/Views/Home/Account.cshtml",
            new AccountViewModel
            {
                rubleAccount = user.RubleAccount,
                dollarAccount = user.DollarAccount,
                euroAccount = user.EuroAccount,
                currentBalance = SetCurrencyForPrint(user.RubleAccount.Balance.ToString()) + " ₽",
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
            return View("~/Views/Home/Authorization.cshtml", new LoginViewModel
            {
                user = model.user,
                result = "Incorrect login or password"
            });
        }

    }

    [HttpPost]
    public IActionResult ValidateCode()
    {
        string inputCode = Request.Form["code"];

        if (int.Parse(inputCode) == (int)HttpContext.Session.GetInt32("verificationCode"))
        {
            User currentUser = db.Users.Find((int)HttpContext.Session.GetInt32("Id")); //get current user

            currentUser.PhoneNumber = HttpContext.Session.GetString("phone");
            currentUser.Mail = HttpContext.Session.GetString("mail");
            db.SaveChanges();

            return View("~/Views/Home/Account.cshtml");
        }
        else
        {
            return View("~/Views/Home/PhoneVerification.cshtml");
        }
    }

    [HttpPost]
    public IActionResult Verification(User user)
    {
        //User currentUser = db.Users.Find((int)HttpContext.Session.GetInt32("Id")); //get current user

        Random rn = new();
        var verificationCode = rn.Next(100000, 999999);
        HttpContext.Session.SetInt32("verificationCode", verificationCode);

        Service.SendEmail(user.Mail, verificationCode);

        HttpContext.Session.SetString("mail", user.Mail);
        HttpContext.Session.SetString("phone", user.PhoneNumber);

        return View("~/Views/Home/PhoneVerification.cshtml");
    }

    public string SetCurrencyForPrint(string str)
    {
        if (str.Length < 4) return str;

        for (int i = str.Length - 3; i >= 0; i -= 3)
        {
            str = str.Insert(i, " ");
        }

        return str;
    }

    public IActionResult SetRubleAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance.ToString()) + " ₽"
        });

    public IActionResult SetDollarAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance.ToString()) + " $"
        });

    public IActionResult SetEuroAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance.ToString()) + " €"
        });

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
