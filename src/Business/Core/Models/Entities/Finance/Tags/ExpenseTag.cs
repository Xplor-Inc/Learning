namespace XploringMe.Core.Models.Entities.Finance.Tags;

public class ExpenseTag : Auditable
{
    public string Color         { get; set; } = default!;
    public string Description   { get; set; } = default!;
    public string Name          { get; set; } = default!;
    public bool   IsActive      { get; set; }
}
