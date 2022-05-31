using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Penkoff.Logic.Users;
using Penkoff.Storage;
using Penkoff.Storage.Entities;
using Penkoff_ASP.NET_Core_.Models;
using SendingEmail;

namespace Penkoff_ASP.NET_Core_.Controllers;

public class ServicesController : Controller
{
    private readonly IUserManager _manager;

    public ServicesController(IUserManager manager)
    {
        _manager = manager;
    }

    public IActionResult Services()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        if (userId is null)
        {
            return View("~/Views/Authorization/Authorization.cshtml", //directs user to auth page if he is not logged in
                new LoginViewModel { result = "" });
        }

        HttpContext.Session.SetString("Currency", "RUB");

        return View(new ServicesViewModel { });
    }

    public async Task<IActionResult> Operations()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithOperations((int)userId);

        return View(new OperationsViewModel
        {
            User = user,
            Operations = user.Operations.ToList().Where(u => u.UserId == userId).Reverse()
        });
    }

    public async Task<IActionResult> SendMoney()
    {
        var userId = HttpContext.Session.GetInt32("Id");

        HttpContext.Session.SetString("currency", "RUB"); //set currency for future usings in Transactions

        var user = await _manager.GetUserWithAccounts((int)userId);

        return View(new SendMoneyViewModel
        {
            Balance = user.RubleAccount.Balance,
            CurrencyPick = "₽",
            Result = ""
        });
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(ServicesViewModel model)
    {
        var userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserAll(userId);

        var currency = HttpContext.Session.GetString("Currency");

        var deposit = new Deposit
        {
            Amount = model.Amount,
            StartDate = DateTime.Now.ToString("dd-MM-yyyy"),
            EndDate = DateTime.Now.AddMonths(36).ToString("dd-MM-yyyy"),
            Rate = 20,
            UserId = userId,
            User = user
        };

        var operation = new Operation()
        {
            UserId = userId,
            Currency = currency,
            Type="Transfer",
            Amount = model.Amount,
            User = user
        };

        await _manager.AddOperation(user, operation);

        await _manager.ChangeUserBalance(user, currency, model.Amount, true);

        await _manager.AddDeposit(user, deposit);
        
        return View("~/Views/Services/Services.cshtml");
    }

    public async Task<IActionResult> Credit(ServicesViewModel model)
    {
        var userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserAll(userId);

        var currency = HttpContext.Session.GetString("Currency");

        var credit = new Credit()
        {
            Amount = model.Amount,
            StartDate = DateTime.Now.ToString("dd-MM-yyyy"),
            EndDate = DateTime.Now.AddMonths(69).ToString("dd-MM-yyyy"),
            Rate = 420,
            UserId = userId,
            User = user
        };

        var operation = new Operation()
        {
            UserId = userId,
            Currency = currency,
            Type = "Incoming",
            Amount = model.Amount,
            User = user
        };

        await _manager.AddOperation(user, operation);

        await _manager.ChangeUserBalance(user, currency, model.Amount, false);

        await _manager.AddCredit(user, credit);

        return View("~/Views/Services/Services.cshtml");
    }

    public async Task<IActionResult> ChangeToRubleAccount()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "RUB");

        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithSpecialAccount(userId, "RUB");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = user.RubleAccount.Balance,
            CurrencyPick = " ₽",
            Result = ""
        });
    }

    public async Task<IActionResult> ChangeToDollarAccountAsync()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "USD");

        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithSpecialAccount(userId, "USD");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = user.DollarAccount.Balance,
            CurrencyPick = " $",
            Result = ""
        });
    }

    public async Task<IActionResult> ChangeToEuroAccountAsync()
    {
        HttpContext.Session.Remove("currency");
        HttpContext.Session.SetString("currency", "EUR");

        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithSpecialAccount(userId, "EUR");

        return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
        {
            Balance = user.EuroAccount.Balance,
            CurrencyPick = " €",
            Result = ""
        });
    }

    public async Task<IActionResult> Transaction(SendMoneyViewModel model)
    {
        var UserId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithAccounts(UserId);

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

        var receiver = await _manager.GetUser(model.ReceiverPhone);

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
                await _manager.ChangeUserBalance(user, "RUB", model.Amount, true);
                await _manager.ChangeUserBalance(receiver, "RUB", model.Amount, false);

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

                await _manager.AddOperation(user, TransferRUB);
                await _manager.AddOperation(receiver, IncomingRUB);

                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.RubleAccount.Balance,
                    CurrencyPick = " ₽",
                    Result = ""
                });

            case "USD":
                await _manager.ChangeUserBalance(user, "USD", model.Amount, true);
                await _manager.ChangeUserBalance(receiver, "USD", model.Amount, false);

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


                await _manager.AddOperation(user, TransferUSD);
                await _manager.AddOperation(receiver, IncomingUSD);

                return View("~/Views/Services/SendMoney.cshtml", new SendMoneyViewModel
                {
                    Balance = user.DollarAccount.Balance,
                    CurrencyPick = " $",
                    Result = ""
                });

            case "EUR":
                await _manager.ChangeUserBalance(user, "EUR", model.Amount, true);
                await _manager.ChangeUserBalance(receiver, "EUR", model.Amount, false);

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


                await _manager.AddOperation(user, TransferEUR);
                await _manager.AddOperation(receiver, IncomingEUR);

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

    public IActionResult SetRuble()
    {
        HttpContext.Session.Remove("Currency");
        HttpContext.Session.SetString("Currency", "RUB");

        return View("~/Views/Services/Services.cshtml");
    }

    public IActionResult SetDollar()
    {
        HttpContext.Session.Remove("Currency");
        HttpContext.Session.SetString("Currency", "USD");

        return View("~/Views/Services/Services.cshtml");
    }

    public IActionResult SetEuro()
    {
        HttpContext.Session.Remove("Currency");
        HttpContext.Session.SetString("Currency", "EUR");

        return View("~/Views/Services/Services.cshtml");
    }

    public async Task<IActionResult> GetPerformanceCard()
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithCardsAndAccounts(userId);

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

        await _manager.AddCard(user, card);

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

    public async Task<IActionResult> GetBillyCard()
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithCardsAndAccounts(userId);

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

        await _manager.AddCard(user, card);

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

    public async Task<IActionResult> GetUltimateCard()
    {
        int userId = (int)HttpContext.Session.GetInt32("Id");

        var user = await _manager.GetUserWithCardsAndAccounts(userId);

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

        await _manager.AddCard(user, card);

        return View("~/Views/Services/MyCards.cshtml", new MyCardsViewModel
        {
            User = user,
            Cards = user.Cards
        });
    }

}

