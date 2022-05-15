namespace Penkoff.Logic.Users
{
    public interface IUserManager
    {
        Task<IList<User>> GetAll();
        Task Create(string Login, string Password, string FirstName, string LastName);
        Task Delete(int Id);
    }
}
