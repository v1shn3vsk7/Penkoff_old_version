namespace Penkoff.Storage.Entities;

public class DollarAccount
{
    public uint Balance { get; set; } = 0;

    [Key]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

}