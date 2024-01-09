using XploringMe.Core.Enumerations.Finance;
using XploringMe.Core.Models.Entities.Finance.Transactions;

namespace XploringMe.Core.Models.Entities.Finance.Budgets;

public class RecurringBill : Auditable
{
    public RecurringFrequency  Frequency        { get; set; }
    public decimal             Amount           { get; set; }
    public string              BillName         { get; set; } = default!;
    public string              AccountNo        { get; set; } = default!;
    public DateTimeOffset      StartDate        { get; set; }
    public bool                Paid             { get; set; }
    public bool                AutoDebit        { get; set; }
    public long?               DebitAccountId   { get; set; }
    public int                 NextPaymentDays  {  get; set; }

    public TransactionAccount? DebitAccount { get; set; }
}
