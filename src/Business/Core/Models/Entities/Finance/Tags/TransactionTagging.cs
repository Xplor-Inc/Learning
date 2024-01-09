using XploringMe.Core.Models.Entities.Finance.Transactions;

namespace XploringMe.Core.Models.Entities.Finance.Tags;

public class TransactionTagging : Auditable
{
    public long TagId           { get; set; }
    public long TransactionId   { get; set; }

    public ExpenseTag Tag           { get; set; } = default!;
    public Transaction Transaction  { get; set; } = default!;
}
