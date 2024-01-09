using XploringMe.Core.Models.Entities.Finance.Transactions;

namespace XploringMe.Core.Models.Entities.Finance.Budgets;

public class RecurringPayment : Auditable
{
    public long?            AccountId       { get; set; }
    public decimal          Amount          { get; set; }
    public string           BillName        { get; set; } = default!;
    public DateTimeOffset   PaymentDate     { get; set; }
    public bool             Paid            { get; set; }
    public bool             AutoDebit       { get; set; }
    public long?            TransactionId   { get; set; }

    public TransactionAccount?  Account     { get; set; }
    public Transaction?         Transaction { get; set; }
}
