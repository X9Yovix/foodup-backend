using Backend.Data;
using Backend.DTOS;
using Backend.Models;
using Backend.Pattern.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pattern.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> Login(User user)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                await _dataContext.Users.AddAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public bool VerifyOTP(string email, string otp)
        {
            var userOTP = _dataContext.Users.SingleOrDefault(u => u.Email == email && u.IsVerified == false  && u.Otp == otp && u.OtpExpirationTime > DateTime.Now);
            return userOTP != null;
        }

		public async Task<ResetPassword> GetResetPasswordByUserId(int userId)
		{
			return await _dataContext.ResetPasswords.FirstOrDefaultAsync(rp => rp.UserId == userId);
		}

		public async Task<bool> AddResetPassword(ResetPassword resetPassword)
		{
			try
			{
				await _dataContext.ResetPasswords.AddAsync(resetPassword);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> ApplyResetPassword(string email, string otp, string password)
		{
			var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null)
			{
				return false;
			}

			var resetPassword = await _dataContext.ResetPasswords.FirstOrDefaultAsync(rp => rp.UserId == user.Id && rp.OTP == otp && rp.ExpirationTime > DateTime.Now);
			if (resetPassword == null)
			{
				return false;
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(password);

			_dataContext.ResetPasswords.Remove(resetPassword);

			return true;
		}

	}
}
