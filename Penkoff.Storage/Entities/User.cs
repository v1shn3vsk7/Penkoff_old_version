namespace Penkoff.Storage.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? PhoneNumber { get; set; } = null;

    public string? Mail { get; set; } = null;

    public RubleAccount RubleAccount { get; set; } 

    public DollarAccount DollarAccount { get; set; }

    public EuroAccount EuroAccount { get; set; }

    public List<Operation> Operations { get; set; } = new();

    public List<Card> Cards { get; set; } = new();

    public List<Deposit> Deposits { get; set; } = new();

    public List<Credit> Credits { get; set; } = new();

}

