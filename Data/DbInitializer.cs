using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UsersContext contex)
        {
            if (contex.Users.Any())
            {
                return;
            }

            /*var user = new User[]
            {
                new User {Id = 0,FirstName = "Vladimir", LastName = "Vasilev", Login = "v1shn3vsk7", Password = "testPassword" },
                new User {}
            };*/
            var user = new User(0, "Vladimir", "Vasilev", "v1shn3vsk7", "testPassword");

            contex.Users.Add(user);

            contex.SaveChanges();
        }
    }
}
