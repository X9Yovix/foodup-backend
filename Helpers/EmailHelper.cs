using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace Backend.Helpers
{
    public class EmailHelper
    {
        public static string GenerateOTP()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task SendEmailAsync(string email, string otp)
        {
            var client = new SmtpClient("smtp.ethereal.email")
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("devyn53@ethereal.email", "TRfcNkz4UyJzR57BFV")
            };

            var message = new MailMessage
            {
                From = new MailAddress("foodup@contact.com"),
                Subject = "OTP Verification",
                Body = $"Your OTP code is: {otp}"
            };

            message.To.Add(email);
            await client.SendMailAsync(message);
        }
    }
}
