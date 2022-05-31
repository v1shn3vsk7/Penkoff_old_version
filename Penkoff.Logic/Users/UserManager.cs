using Microsoft.EntityFrameworkCore;
using Penkoff.Storage;

namespace Penkoff.Logic.Users;

public class UserManager : IUserManager
{
    private readonly UsersContext _context;

    public UserManager(UsersContext context)
    {
        _context = context;
    }

    public async Task<IList<User>> GetAll() => await _context.Users.ToListAsync();

    public async Task<User> GetUserWithAccounts(int Id)
    {
        var user = await _context.Users.Include(u => u.RubleAccount)
            .Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> GetUser(int Id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> GetUserWithSpecialAccount(int Id, string Currency)
    {
        User? user = Currency switch
        {
            "RUB" => await _context.Users.Include(u => u.RubleAccount)
                       .FirstOrDefaultAsync(u => u.Id == Id),
            "USD" => await _context.Users.Include(u => u.DollarAccount)
                .FirstOrDefaultAsync(u => u.Id == Id),
            "EUR" => await _context.Users.Include(u => u.EuroAccount)
                .FirstOrDefaultAsync(u => u.Id == Id),
            _ => null,
        };

        return user;
    }

    public async Task<User> GetUserWithCards(int Id)
    {
        var user = await _context.Users.Include(u => u.Cards).FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> GetUserWithOperations(int Id)
    {
        var user = await _context.Users.Include(u => u.Operations).FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> GetUserWithCardsAndAccounts(int Id)
    {
        var user = await _context.Users.Include(u => u.Cards).Include(u => u.RubleAccount).
            Include(u => u.DollarAccount).Include(u => u.EuroAccount).FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> GetUser(string Login, string Password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == Login && u.Password == Password);

        return user;
    }

    public async Task<User> GetUser(string PhoneNumber)
    {
        var user = await _context.Users.Include(u => u.RubleAccount).Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber);

        return user;
    }

    public async Task<User> GetUserAll(int Id)
    {
        var user = await _context.Users.Include(u => u.RubleAccount).Include(u => u.DollarAccount)
            .Include(u => u.EuroAccount).Include(u => u.Deposits).Include(u => u.Operations)
            .FirstOrDefaultAsync(u => u.Id == Id);

        return user;
    }

    public async Task<User> FindUser(string Login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == Login);

        return user;
    }

    public async Task ChangeUserBalance(User user, string Currency, uint Amount, bool isSubstracion)
    {
        switch (Currency)
        {
            case "RUB":
                if (isSubstracion == true)
                    user.RubleAccount.Balance -= Amount;
                else user.RubleAccount.Balance += Amount;
                break;

            case "USD":
                if (isSubstracion == true)
                    user.DollarAccount.Balance -= Amount;
                else user.DollarAccount.Balance += Amount;
                break;

            case "EUR":
                if (isSubstracion == true)
                    user.EuroAccount.Balance -= Amount;
                else user.EuroAccount.Balance += Amount;
                break;

            default:
                break;
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddOperation(User user, Operation operation)
    {
        user.Operations.Add(operation);

        await _context.SaveChangesAsync();
    }

    public async Task AddCard(User user, Card card)
    {
        user.Cards.Add(card);

        await _context.SaveChangesAsync();
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    public async Task AddPhoneAndEmail(User user, string PhoneNumber, string Email)
    {
        user.PhoneNumber = PhoneNumber;
        user.Mail = Email;

        await _context.SaveChangesAsync();
    }

    public async Task AddDeposit(User user, Deposit deposit)
    {
        user.Deposits.Add(deposit);

        await _context.SaveChangesAsync();
    }

    public async Task AddCredit(User user, Credit credit)
    {
        user.Credits.Add(credit);

        await _context.SaveChangesAsync();
    }

    public async Task Create(string login, string password, string firstName, string lastName)
    {
        var user = new User { Login = login, Password = password, FirstName = firstName, LastName = lastName };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var user = _context.Users.Find(id);

        if (user != null)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }
}
