using System.Threading.Tasks;
using Demo.Models;

namespace Demo.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string ssername, string password);
         Task<bool> UserExists(string username);
    }
}