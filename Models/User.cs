namespace Penkoff_ASP.NET_Core_.Models;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public User(int Id, string FirstName, string LastName, string Login, string Password)
    {
        this.Id = Id;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Login = Login;
        this.Password = Password;
    }
}
