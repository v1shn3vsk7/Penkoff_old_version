
namespace Penkoff.Storage.Entities;

public class Operation
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    public string Currency { get; set; }

    public string Type { get; set; }

    public uint Amount { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}

