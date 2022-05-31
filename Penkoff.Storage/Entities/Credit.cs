namespace Penkoff.Storage.Entities;

public class Credit
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Rate { get; set; }

    public uint Amount { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}

