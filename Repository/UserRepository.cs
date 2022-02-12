using eObrazci.Data;
using eObrazci.Interfaces;
using eObrazci.Models;

namespace eObrazci.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);

            return Save();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.Where(u=>u.Username == username).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(user => user.Username == username);
        }

        public bool UserVerify(string username, string password)
        {
            var user = _context.Users.Where(u => u.Username == username).FirstOrDefault();

            if (user.Password == password)
            {
                return true;
            }

            return false;
        }
    }
}
