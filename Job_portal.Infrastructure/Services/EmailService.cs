using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace Job_portal.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendApplicationStatusEmailAsync(string toEmail, string fullName, string jobTitle, string companyName, string status)
        {
            var subject = $"Your application for {jobTitle} was {status}";
            var isAccepted = status.ToLower() == "accepted";
            var color = isAccepted ? "#16A34A" : "#DC2626";
            var emoji = isAccepted ? "🎉" : "😔";
            StringBuilder mailBody = new StringBuilder();

            mailBody.Append($"<p>Dear {fullName},</p></br></br>");
            mailBody.Append($"<p>We are writing to inform you about the status of your application for the position of {jobTitle} at {companyName}.</p></br>");
            mailBody.Append($"<p style='color:{color}; font-size: 18px;'>Your application has been <strong>{status.ToUpper()}</strong> {emoji}</p></br>");
            mailBody.Append($"<p>Thank you for your interest in joining {companyName}. We appreciate the time and effort you put into your application.</p></br></br>");
            mailBody.Append($"<p>Best regards,</p>");
            mailBody.Append("<br /><br />");
            mailBody.Append("Customer Support.");

            await SendEmailAsync(toEmail, subject, mailBody.ToString());
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string fullName, string confirmationLink)
        {
            var subject = "Confirm your Job Portal account";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #1A56DB;'>Welcome to Job Portal!</h2>
                    <p>Hi <strong>{fullName}</strong>,</p>
                    <p>Thanks for registering. Please confirm your email address by clicking the button below.</p>
                    <p>This link expires in <strong>24 hours</strong>.</p>
                    <a href='{confirmationLink}'
                       style='display: inline-block; padding: 12px 24px;
                              background-color: #1A56DB; color: white;
                              text-decoration: none; border-radius: 6px;
                              font-weight: bold; margin: 16px 0;'>
                        Confirm Email
                    </a>
                    <p style='color: #64748B; font-size: 13px;'>
                        If you did not create an account, ignore this email.
                    </p>
                </div>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendForgotPasswordEmailAsync(string toEmail, string fullName, string resetLink)
        {
            var subject = "Reset your Job Portal password";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #1A56DB;'>Password Reset Request</h2>
                    <p>Hi <strong>{fullName}</strong>,</p>
                    <p>We received a request to reset your password. Click the button below to reset it.</p>
                    <p>This link expires in <strong>24 hours</strong>.</p>
                    <a href='{resetLink}'
                       style='display: inline-block; padding: 12px 24px;
                              background-color: #1A56DB; color: white;
                              text-decoration: none; border-radius: 6px;
                              font-weight: bold; margin: 16px 0;'>
                        Reset Password
                    </a>
                    <p style='color: #64748B; font-size: 13px;'>
                        If you did not request a password reset, ignore this email.
                    </p>
                </div>";

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mailMessage.To.Add(MailboxAddress.Parse(toEmail));

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;

            mailMessage.Body = bodyBuilder.ToMessageBody();
            mailMessage.Subject = subject;

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPassword); 
            
            await smtpClient.SendAsync(mailMessage);

            await smtpClient.DisconnectAsync(true); 
        }
    }
}
