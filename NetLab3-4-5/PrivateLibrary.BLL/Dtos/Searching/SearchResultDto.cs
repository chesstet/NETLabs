namespace PrivateLibrary.BLL.Dtos.Searching
{
    public record SearchResultDto<TEntity>
    {
        public int TotalAmount { get; set; }

        public IReadOnlyCollection<TEntity>? Entities { get; set; }
    }
}
