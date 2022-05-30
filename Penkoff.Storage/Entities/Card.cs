namespace Penkoff.Storage.Entities;

public class Card
{
    [Key]
    public Int64 Pan { get; set; } //card number

    public DateTime ExpirationDate { get; set; }

    public uint CVV { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}

