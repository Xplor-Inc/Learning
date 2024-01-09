namespace XploringMe.BlazorApp.Models.Dtos.Finance.Tags;

public class TransactionTaggingDto
{
    public long TagId               { get; set; }
    public long TransactionId       { get; set; }
    public CategoryDto? Tag       { get; set; } = default!;
}
