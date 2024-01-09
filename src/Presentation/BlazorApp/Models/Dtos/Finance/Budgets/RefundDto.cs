namespace XploringMe.BlazorApp.Models.Dtos.Finance.Budgets;

public class RefundDto
{
    public decimal          Refund              { get; set; }
    public string           Remark              { get; set; } = default!;
    public DateTimeOffset   Date                { get; set; }
    public long?            RefundAccountId     { get; set; }
}
