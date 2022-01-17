namespace PrivateLibrary.BLL.Dtos.Auth
{
    public record LoginDto
    {
        public string? Login { get; init; }
        public string? Password { get; init; }
        public bool Remember { get; init; }
    }
}
