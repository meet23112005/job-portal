namespace Job_portal.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(
            string toEmail,
            string fullName,
            string confirmationLink);

        Task SendApplicationStatusEmailAsync(
            string toEmail,
            string fullName,
            string jobTitle,
            string companyName,
            string status); // "Accepted" or "Rejected"

        Task SendForgotPasswordEmailAsync(string toEmail, string fullName, string confirmationLink);
    }
}
