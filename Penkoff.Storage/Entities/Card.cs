namespace Penkoff.Storage.Entities;
public enum Types
{
    PerformanceEdition,
    BillyEdition,
    UltimateEdition
}
public class Card
{
    [Key]
    public long Pan { get; set; } //card number

    public Types Type { get; set; }

    public DateTime ExpirationDate { get; set; }

    public uint CVV { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}

