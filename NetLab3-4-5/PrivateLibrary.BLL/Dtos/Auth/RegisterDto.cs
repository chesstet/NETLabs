namespace PrivateLibrary.BLL.Dtos.Auth
{
    public record RegisterDto
    {
        public string? Login { get; init; }
        public string? Name { get; init; }
        public string? Surname { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Password { get; init; }
    }
}
