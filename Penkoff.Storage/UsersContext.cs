using Penkoff.Storage.Entities;

namespace Penkoff.Storage;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    public DbSet<RubleAccount> RubleAccounts { get; set; }

    public DbSet<DollarAccount> DollarAccounts { get; set; }

    public DbSet<EuroAccount> EuroAccounts { get; set; }

    public DbSet<Operation> Operations { get; set; }

    public DbSet<Card> Cards { get; set; }

    public DbSet<Deposit> Deposits { get; set; }

    public DbSet<Credit> Credits { get; set; }
}
