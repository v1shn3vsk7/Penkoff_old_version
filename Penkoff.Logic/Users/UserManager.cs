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

    public async Task Create(string login, string password, string firstName, string lastName)
    {
        var user = new User { Login = login, Password = password, FirstName = firstName, LastName = lastName };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        //var user = _context.Users.FirstOrDefault(u => u.Id == id);

        var user = _context.Users.Find(id);

        if (user != null)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }
}
