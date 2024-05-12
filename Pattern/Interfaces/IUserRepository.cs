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
        bool VerifyOTP(string email, string otp);
        Task<ResetPassword> GetResetPasswordByUserId(int userId);
		Task<bool> AddResetPassword(ResetPassword resetPassword);
        Task<bool> ApplyResetPassword(string email, string otp, string password);
	}
}
