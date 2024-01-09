using XploringMe.Core.Enumerations.Finance;

namespace XploringMe.Core.Models.Entities.Finance.Categories;

public class Category : Auditable
{
    public CategoryType Type        { get; set; }
    public string       Name        { get; set; } = default!;
    public string       Description { get; set; } = default!;
    public string       Color       { get; set; } = default!;
    public bool         IsActive    { get; set; } = true;
}