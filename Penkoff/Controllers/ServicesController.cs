using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class ServicesController : Controller
{
    UsersContext db;

    public ServicesController(UsersContext context)
    {
        db = context;
    }

    public IActionResult Services()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return View("~/Views/Authorization/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        return View();
    }

    public IActionResult Operations()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.Operations).FirstOrDefault(u => u.Id == userId);

        return View(new OperationsViewModel
        {
            User = user,
            Operations = user.Operations.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }

    public IActionResult SendMoney()
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
    }

    public IActionResult ChangeToRubleAccount()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "RUB");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.RubleAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .RubleAccount.Balance,
            CurrencyPick = " ₽",
            Result = ""
        });
    }

    public IActionResult ChangeToDollarAccount()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "USD");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.DollarAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .DollarAccount.Balance,
            CurrencyPick = " $",
            Result = ""
        });
    }

    public IActionResult ChangeToEuroAccount()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "EUR");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = db.Users.Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == (int)HttpContext.Session.GetInt32("Id"))
            .EuroAccount.Balance,
            CurrencyPick = " €",
            Result = ""
        });
    }

    public IActionResult Transaction(SendMoneyViewModel model)
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
            return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
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
            return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
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

                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
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

                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
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

                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.EuroAccount.Balance,
                    CurrencyPick = " €",
                    Result = ""
                });

            default:
                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.RubleAccount.Balance,
                    CurrencyPick = " ₽",
                    Result = "Something went wrong. Please try again"

                });
        }
    }

    public IActionResult GetPerformanceCard()
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

        user.Cards.Add(card);

        db.SaveChanges();

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

    public IActionResult GetBillyCard()
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.Cards).Include(u => u.RubleAccount).Include(u => u.DollarAccount).Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == userId);

        Random rn = new();

        var time = DateTime.Now.AddYears(5).ToString("M/yyyy");

        var card = new Card()
        {
            Pan = rn.NextInt64(4000000000000000, 9999999999999999),
            Type = Types.BillyEdition,
            ExpirationDate = time,
            CVV = (uint)rn.NextInt64(100, 999),
            UserId = userId,
            User = user,
        };

        user.Cards.Add(card);

        db.SaveChanges();

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

    public IActionResult GetUltimateCard()
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = db.Users.Include(u => u.Cards).Include(u => u.RubleAccount).Include(u => u.DollarAccount).Include(u => u.EuroAccount)
            .FirstOrDefault(u => u.Id == userId);

        Random rn = new();

        var time = DateTime.Now.AddYears(5).ToString("M/yyyy");

        var card = new Card()
        {
            Pan = rn.NextInt64(4000000000000000, 9999999999999999),
            Type = Types.UltimateEdition,
            ExpirationDate = time,
            CVV = (uint)rn.NextInt64(100, 999),
            UserId = userId,
            User = user,
        };

        user.Cards.Add(card);

        db.SaveChanges();

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

}

