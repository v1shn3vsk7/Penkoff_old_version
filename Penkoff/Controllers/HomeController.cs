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

    /*public IActionResult Services()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return View("~/Views/Home/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        return View();
    }*/

    /*public IActionResult MyCards()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return Account(); //RedirectToAction("Account", "Login");
        }

        var user = db.Users.Include(u => u.Cards).FirstOrDefault(u => u.Id == userId);

        return View(new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }*/

   /* public IActionResult GetPerformanceCard(MyCardsViewModel model)
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.Cards).Include(u => u.RubleAccount).Include(u => u.DollarAccount).Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == userId);

        Random rn = new();

        var time = DateTime.Now.AddYears(5).ToString("M/yyyy");

        var card = new Card()
        {
            Pan = rn.NextInt64(4000000000000000, 9999999999999999),
            Type = Types.PerformanceEdition,
            ExpirationDate = time,
            CVV = (uint)rn.NextInt64(100, 999),
            UserId = userId,
            User = user,
        };

        //db.Entry(user).State = EntityState.Added;
        user.Cards.Add(card);
        //db.Cards.Add(card);

        db.SaveChanges();

        return View("~/Views/Home/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }*/

    /*public IActionResult Operations()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.Operations).FirstOrDefault(u => u.Id == userId);

        return View(new OperationsViewModel
        {
            User = user,
            Operations = user.Operations.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }*/

    /*public IActionResult SendMoney()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        HttpContext.Session.SetString("currency", "RUB"); //set currency for future usings in Transactions

        var user = db.Users.Include(u => u.RubleAccount)
           .Include(u => u.DollarAccount)
           .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == userId);

        return View(new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance,
            CurrencyPick = "₽",
            Result = ""
        });
    }*/

    /*public IActionResult ChangeToRubleAccount(SendMoneyViewModel model)
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "RUB");

        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance,
            CurrencyPick = " ₽",
            Result = ""
        });
    }*/

    /*public IActionResult ChangeToDollarAccount(SendMoneyViewModel model)
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "USD");

        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance,
            CurrencyPick = " $",
            Result = ""
        });
    }*/

    /*public IActionResult ChangeToEuroAccount(SendMoneyViewModel model)
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "EUR");

        return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance,
            CurrencyPick = " €",
            Result = ""
        });
    }*/

    /*public IActionResult Transaction(SendMoneyViewModel model)
    {
        var UserId = (int)HttpContext.Session.GetInt32("Id");
        var user = db.Users.Include(u => u.RubleAccount)
            .Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefault(u => u.Id == UserId);

        try
        {
            model.Amount = uint.Parse(Request.Form["amount"]);      //if user input is wrong
            model.ReceiverPhone = Request.Form["phone_number"];
            model.CurrencyPick = HttpContext.Session.GetString("currency");
        }
        catch (InvalidOperationException)
        {
            return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
            {
                Balance = user.RubleAccount.Balance,
                CurrencyPick = " ₽",
                Result = "Something is wrong. Please try again"
            });
        }

        var receiver = db.Users.Include(u => u.RubleAccount)
            .Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefault(u => u.PhoneNumber == model.ReceiverPhone);

        if (receiver is null)
        {
            return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
            {
                Result = "Incorrect information. Receiver is not found!"
            });
        }

        switch (model.CurrencyPick)
        {
            case "RUB":
                user.RubleAccount.Balance -= model.Amount;
                receiver.RubleAccount.Balance += model.Amount;

                Operation TransferRUB = new()
                {
                    Amount = model.Amount,
                    Currency = "RUB",
                    Type = "Transfer",
                    User = user,
                    UserId = UserId
                };
                Operation IncomingRUB = new()
                {
                    Amount = model.Amount,
                    Currency = "RUB",
                    Type = "Incoming",
                    User = receiver,
                    UserId = receiver.Id
                };

                user.Operations.Add(TransferRUB);
                receiver.Operations.Add(IncomingRUB);

                db.SaveChanges();

                return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.RubleAccount.Balance,
                    CurrencyPick = " ₽",
                    Result = ""
                });

            case "USD":
                user.DollarAccount.Balance -= model.Amount;
                receiver.DollarAccount.Balance += model.Amount;

                Operation TransferUSD = new()
                {
                    Amount = model.Amount,
                    Currency = "USD",
                    Type = "Transfer",
                    User = user,
                    UserId = UserId
                };
                Operation IncomingUSD = new()
                {
                    Amount = model.Amount,
                    Currency = "USD",
                    Type = "Incoming",
                    User = receiver,
                    UserId = receiver.Id
                };

                user.Operations.Add(TransferUSD);
                receiver.Operations.Add(IncomingUSD);

                db.SaveChanges();

                return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.DollarAccount.Balance,
                    CurrencyPick = " $",
                    Result = ""
                });

            case "EUR":
                user.EuroAccount.Balance -= model.Amount;
                receiver.EuroAccount.Balance += model.Amount;

                Operation TransferEUR = new()
                {
                    Amount = model.Amount,
                    Currency = "EUR",
                    Type = "Transfer",
                    User = user,
                    UserId = UserId
                };
                Operation IncomingEUR = new()
                {
                    Amount = model.Amount,
                    Currency = "EUR",
                    Type = "Incoming",
                    User = receiver,
                    UserId = receiver.Id
                };

                user.Operations.Add(TransferEUR);
                receiver.Operations.Add(IncomingEUR);

                db.SaveChanges();

                return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.EuroAccount.Balance,
                    CurrencyPick = " €",
                    Result = ""
                });

            default:
                return View("~/Views/Home/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.RubleAccount.Balance,
                    CurrencyPick = " ₽",
                    Result = "Something went wrong. Please try again"

                });
        }
    }*/

    /*public IActionResult Authorization()
    {
        return View();
    }*/

    /*public IActionResult Login()
    {
        return View();
    }*/

    public IActionResult Privacy()
    {
        return View();
    }

    /*public IActionResult Account()
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
    }*/

    /*[HttpPost]
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

    }*/

    /*[HttpPost]
    public IActionResult ValidateCode()
    {
        string inputCode = Request.Form["code"];

        if (int.Parse(inputCode) == (int)HttpContext.Session.GetInt32("verificationCode"))
        {
            User currentUser = db.Users.Find((int)HttpContext.Session.GetInt32("Id")); //get current user

            currentUser.PhoneNumber = HttpContext.Session.GetString("phone");
            currentUser.Mail = HttpContext.Session.GetString("mail");
            db.SaveChanges();

            return Account();
        }
        else
        {
            return View("~/Views/Home/PhoneVerification.cshtml");
        }
    }*/

    /*[HttpPost]
    public IActionResult Verification(User user)
    {
        Random rn = new();
        var verificationCode = rn.Next(100000, 999999);
        HttpContext.Session.SetInt32("verificationCode", verificationCode);

        Service.SendEmail(user.Mail, verificationCode);

        HttpContext.Session.SetString("mail", user.Mail);
        HttpContext.Session.SetString("phone", user.PhoneNumber);

        return View("~/Views/Home/PhoneVerification.cshtml");
    }*/

    /*public string SetCurrencyForPrint(string str)
    {
        if (str.Length < 4) return str;

        for (int i = str.Length - 3; i >= 0; i -= 3)
        {
            str = str.Insert(i, " ");
        }

        return str;
    }*/

   /* public IActionResult SetRubleAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance.ToString()) + " ₽"
        });*/

    /*public IActionResult SetDollarAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance.ToString()) + " $"
        });*/

    /*public IActionResult SetEuroAccount() => View("~/Views/Home/Account.cshtml",
        new AccountViewModel
        {
            currentBalance = SetCurrencyForPrint(db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance.ToString()) + " €"
        });*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }

}
