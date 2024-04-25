using Backend.DTOS;
using Backend.Models;

namespace Backend.Pattern.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> Register(User user);
        Task<User> Login(User user);
        Task<User> GetUserByEmail(string email);
    }
}
