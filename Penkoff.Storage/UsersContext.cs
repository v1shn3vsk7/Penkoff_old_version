using Microsoft.EntityFrameworkCore;
using Penkoff.Storage.Entities;

namespace Penkoff.Storage;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
}
