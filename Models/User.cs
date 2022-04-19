namespace Penkoff_ASP.NET_Core_.Models;
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User()
        {

        }
    }
