namespace Job_portal.Application.Common.Settings
{
    public class EmailSettings
    {
        public const string SectionName = "Email";

        public string SmtpHost { get; init; } = string.Empty;
        public int SmtpPort { get; init; } = 587;
        public string SmtpUser { get; init; } = string.Empty;
        public string SmtpPassword { get; init; } = string.Empty;
        public string SenderEmail { get; init; } = string.Empty;
        public string SenderName { get; init; } = string.Empty;
    }
}
