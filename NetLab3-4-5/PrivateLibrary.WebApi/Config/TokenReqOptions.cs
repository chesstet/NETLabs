namespace PrivateLibrary.WebApi.Config
{
    public class TokenReqOptions
    {
        public const string Name = "Jwt";

        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int DurationMinutes { get; set; }
    }
}
