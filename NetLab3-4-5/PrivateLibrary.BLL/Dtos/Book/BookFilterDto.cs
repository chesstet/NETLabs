namespace PrivateLibrary.BLL.Dtos.Book
{
    public record BookFilterDto : ShortBookFilterDto
    {
        public string SearchText { get; init; } = string.Empty;
    }
}
