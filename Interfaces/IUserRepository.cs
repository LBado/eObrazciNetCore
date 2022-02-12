using eObrazci.Models;

namespace eObrazci.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        bool UserExists(string username);
        bool UserVerify(string username, string password);
        bool CreateUser(User user);
        User GetUserByUsername(string username);
        bool Save();

    }
}
