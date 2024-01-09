namespace XploringMe.BlazorApp.Models.Dtos.Finance.Budgets;

public class RecurringBillDto : AuditableDto
{    
    public RecurringFrequency   Frequency       { get; set; }
    public decimal              Amount          { get; set; }
    public string               BillName        { get; set; } = default!;
    public string               AccountNo       { get; set; } = default!;
    public DateTimeOffset       StartDate       { get; set; }
    public bool                 Paid            { get; set; }
    public bool                 AutoDebit       { get; set; }
    public long?                DebitAccountId  { get; set; }
    public int                  NextPaymentDays { get; set; }
    public TransactionAccountDto?    DebitAccount    { get; set; }

    public int GetNextPaymentDays()
    {
        if(NextPaymentDays > 0) return NextPaymentDays;
        var nxt = StartDate;
        if (Frequency == RecurringFrequency.Monthly) { return (nxt.AddMonths(1) - StartDate).Days; }
        if (Frequency == RecurringFrequency.Yearly) { return (nxt.AddMonths(12) - StartDate).Days; }
        if (Frequency == RecurringFrequency.HalfYearly) { return (nxt.AddMonths(6) - StartDate).Days; }
        if (Frequency == RecurringFrequency.Quarterly) { return (nxt.AddMonths(3) - StartDate).Days; }
        return 0;
    }

}