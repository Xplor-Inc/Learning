namespace XploringMe.BlazorApp.Models.Dtos.Finance.Categories;
public class CategoryDto : AuditableDto
{
    public string       Name        { get; set; } = default!;
    public string       Description { get; set; } = default!;
    public CategoryType Type        { get; set; }
    public string       Color       { get; set; } = default!;
    public bool         IsActive    { get; set; }
}
