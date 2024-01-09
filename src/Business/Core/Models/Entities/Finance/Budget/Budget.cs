namespace XploringMe.Core.Models.Entities.Finance.Budgets;
public class Budget : Auditable
{
    public decimal          Actual      { get; set; }
    public string?          Description { get; set; }
    public decimal          Expected    { get; set; }
    public DateTimeOffset   Month       { get; set; }
}
