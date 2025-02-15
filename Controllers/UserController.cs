﻿using Backend.Data;
using Backend.DTOS;
using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _uow.UserRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(DTOS.LoginRequest loginRequest)
        {
            var user = await _uow.UserRepository.GetUserByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BadRequest(new { Status = "Error", Message = "Invalid email or password" });
            }
            if (user.IsVerified == false)
            {
                return BadRequest(new { Status = "Error", Message = "Verify your account first" });
            }
            var token = JwtHelper.GenerateJwtToken(user);
            var userDto = new LoginResponse
            {
                Message = "Logged in successfully",
                Token = token
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(DTOS.RegisterRequest registerRequest)
        {
            var existingUser = await _uow.UserRepository.GetUserByEmail(registerRequest.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Status = "Error", Message = "Email already exists" });
            }

            registerRequest.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            var otp = EmailHelper.GenerateOTP();
            var expirationTime = DateTime.Now.AddMinutes(10);
            var user = new User()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Gender = registerRequest.Gender,
                Role = "user",
                Otp = otp,
                OtpExpirationTime = expirationTime,
                IsVerified = false
            };
            var data = await _uow.UserRepository.Register(user);
            await _uow.SaveChangesAsync();
            await EmailHelper.SendEmailAsync(user.Email, otp);
            return Ok(new { Status = "Success", Message = "Account created successfully, check your email to verify account" });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP(DTOS.VerifyOTPRequest verifyOTPRequest)
        {
            var isVerified = _uow.UserRepository.VerifyOTP(verifyOTPRequest.Email, verifyOTPRequest.Otp);
            if (isVerified)
            {
                var user = _uow.UserRepository.GetUserByEmail(verifyOTPRequest.Email).Result;
                user.IsVerified = true;
                _uow.SaveChangesAsync();
                return Ok(new { Status = "Success", Message = "OTP verified successfully" });
            }
            else
            {
                return BadRequest(new { Status = "Error", Message = "Invalid OTP or OTP expired" });
            }
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOTP(DTOS.ResendOTPRequest resendOTPRequest)
        {
            var user = await _uow.UserRepository.GetUserByEmail(resendOTPRequest.Email);
            if (user == null)
            {
                return BadRequest(new { Status = "Error", Message = "Email not found" });
            }

            if (!user.IsVerified && IsOtpExpired(resendOTPRequest.Email))
            {
                var otp = EmailHelper.GenerateOTP();
                var expirationTime = DateTime.Now.AddMinutes(10);
                user.Otp = otp;
                user.OtpExpirationTime = expirationTime;

                await _uow.SaveChangesAsync();
                await EmailHelper.SendEmailAsync(user.Email, otp);
                return Ok(new { Status = "Success", Message = "OTP resent successfully" });
            }

            return BadRequest(new { Status = "Error", Message = "OTP not expired or user already verified" });
        }

        private bool IsOtpExpired(string email)
        {
            var user = _uow.UserRepository.GetUserByEmail(email).Result;
            if (user != null && user.OtpExpirationTime < DateTime.Now)
            {
                return true;
            }
            return false;
        }

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
		{
			var user = await _uow.UserRepository.GetUserByEmail(resetPasswordRequest.Email);
			if (user == null)
			{
				return BadRequest(new { Status = "Error", Message = "User not found" });
			}

			var existingResetPassword = await _uow.UserRepository.GetResetPasswordByUserId(user.Id);
			if (existingResetPassword != null && existingResetPassword.ExpirationTime > DateTime.Now)
			{
				return BadRequest(new { Status = "Error", Message = "Password reset request already sent" });
			}

			string otp = EmailHelper.GenerateOTP();
			DateTime expirationTime = DateTime.Now.AddMinutes(10);

			var resetPassword = new ResetPassword
			{
				OTP = otp,
				ExpirationTime = expirationTime,
				UserId = user.Id
			};

			var result = await _uow.UserRepository.AddResetPassword(resetPassword);
			await _uow.SaveChangesAsync();
			if (!result)
			{
				return StatusCode(500, new { Status = "Error", Message = "Failed to reset password. Please try again later" });
			}

			await EmailHelper.SendEmailAsync(user.Email, otp);

			return Ok(new { Status = "Success", Message = "Password reset request sent. Check your email for OTP" });
		}


		[HttpPut("reset-password/apply")]
		public async Task<IActionResult> ApplyResetPassword(ApplyResetPasswordRequest applyResetPasswordRequest)
		{
			var result = await _uow.UserRepository.ApplyResetPassword(
				applyResetPasswordRequest.Email,
				applyResetPasswordRequest.Otp,
				applyResetPasswordRequest.Password
			);

			if (result)
			{
				await _uow.SaveChangesAsync();
				return Ok(new { Status = "Success", Message = "Password updated" });
			}
			else
			{
				return BadRequest(new { Status = "Error", Message = "Failed to apply password reset" });
			}
		}

		[Authorize(Roles = "admin")]
		[HttpGet("dashboard/count")]
		public async Task<IActionResult> GetNumberOfUsers()
		{
			var count = await _uow.UserRepository.GetNumberOfUsers();
			return Ok(new { count = count });
		}
	}
}
