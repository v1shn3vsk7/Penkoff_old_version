namespace Penkoff.Logic.Users
{
    public interface IUserManager
    {
        Task<IList<User>> GetAll();

        Task<User> GetUserWithAccounts(int Id);

        Task<User> GetUser(int Id);

        Task<User> GetUser(string Login, string Password);

        Task<User> GetUser(string PhoneNumber);

        Task<User> GetUserWithSpecialAccount(int Id, string Currency);

        Task<User> GetUserWithCards(int Id);

        Task<User> GetUserWithOperations(int Id);

        Task<User> GetUserWithCardsAndAccounts(int Id);

        Task<User> FindUser(string Login);

        Task ChangeUserBalance(User user, string Currency, uint Amount, bool isSubstracion);

        Task AddOperation(User user, Operation operation);

        Task AddCard(User user, Card card);

        Task AddUser(User user);

        Task AddPhoneAndEmail(User user, string PhoneNumber, string Email);

        Task Create(string Login, string Password, string FirstName, string LastName);

        Task Delete(int Id);
    }
}
