namespace XploringMe.Core.Models.Finance.Reports;

public class AccountSumary
{
    public decimal  Income   { get; set; }
    public decimal  Expense  { get; set; }
    public decimal  Saving   { get { return Income - Expense; } }
    public string   Month    { get; set; } = string.Empty;
}
