namespace XploringMe.BlazorApp.Models.Dtos.Finance.Budgets;
public class BudgetDto : AuditableDto
{
    public decimal          Actual      { get; set; }
    public string?          Description { get; set; }
    public decimal          Expected    { get; set; }
    public DateTimeOffset   Month       { get; set; }
}
