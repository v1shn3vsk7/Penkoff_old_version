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

    //[ForeignKey(nameof(Id))]
    public RubleAccount RubleAccount { get; set; } 

    //[ForeignKey(nameof(Id))]
    public DollarAccount DollarAccount { get; set; }

    //[ForeignKey(nameof(Id))]
    public EuroAccount EuroAccount { get; set; }

    public List<Operation> Operations { get; set; } = new();

    public List<Card> Cards { get; set; } = new();

}

