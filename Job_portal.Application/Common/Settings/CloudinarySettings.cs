namespace Job_portal.Application.Common.Settings
{
    public class CloudinarySettings
    {
        public const string SectionName = "Cloudinary";

        public string CloudName { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public string ApiSecret { get; init; } = string.Empty;
    }
}
