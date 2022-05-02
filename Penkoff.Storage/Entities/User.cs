namespace Penkoff.Storage.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

   // [Required]
}

