namespace XploringMe.BlazorApp.Models.Dtos.Finance.Budgets;

public class CreateNextPaymentDto
{
    public DateTimeOffset   PaymentDate     { get; set; }
    public long?            TransactionId   { get; set; }
}
