namespace Penkoff.Storage.Entities;

public class EuroAccount
{
    public uint Balance { get; set; } = 0;

    [Key]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

}
