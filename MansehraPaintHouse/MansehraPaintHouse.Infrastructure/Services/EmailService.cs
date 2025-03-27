using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using MansehraPaintHouse.Core.Interfaces.IServices;
using Microsoft.Extensions.Logging;

namespace MansehraPaintHouse.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
                if (smtpSettings == null)
                {
                    throw new Exception("Email settings not configured");
                }

                using (var client = new SmtpClient(smtpSettings.SmtpServer, smtpSettings.SmtpPort))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings.FromEmail, smtpSettings.FromName),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent successfully to {email}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}");
                throw;
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var subject = "Reset Your Password - Mansehra Paint House";
            var message = $@"
                <h2>Password Reset Request</h2>
                <p>You have requested to reset your password. Click the link below to reset it:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you did not request this reset, please ignore this email.</p>
                <p>This link will expire in 1 hour.</p>
                <p>Best regards,<br>Mansehra Paint House Team</p>";

            await SendEmailAsync(email, subject, message);
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var subject = "Confirm Your Email - Mansehra Paint House";
            var message = $@"
                <h2>Email Confirmation</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <p><a href='{confirmationLink}'>Confirm Email</a></p>
                <p>If you did not create an account, please ignore this email.</p>
                <p>Best regards,<br>Mansehra Paint House Team</p>";

            await SendEmailAsync(email, subject, message);
        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
} 