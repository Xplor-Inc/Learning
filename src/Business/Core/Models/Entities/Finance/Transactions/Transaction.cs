using XploringMe.Core.Enumerations.Finance;
using XploringMe.Core.Models.Entities.Finance.Categories;
using XploringMe.Core.Models.Entities.Finance.Tags;

namespace XploringMe.Core.Models.Entities.Finance.Transactions;

public class Transaction : Auditable
{
    public decimal              Credit              { get; set; }
    public decimal              Debit               { get; set; }
    public long                 TransactionAccountId{ get; set; }
    public long                 CategoryId          { get; set; }
    public decimal              CurrentBalance      { get; set; }
    public string?              InvoicePath         { get; set; }
    public long?                ParentId            { get; set; }
    public bool                 IsRefunded          { get; set; }
    public string               Particular          { get; set; } = default!;
    public DateTimeOffset       PaymentDate         { get; set; }
    public decimal              PreviousBalance     { get; set; }
    public DateTimeOffset       TransactionDate     { get; set; }
    public TransactionType      TransactionType     { get; set; }
    public TransactionSource    Source              { get; set; }
    public DebtStatus           DebtStatus          { get; set; }

    #region Navigation Properties
    public virtual Category                     Category             { get; set; } = default!;
    public virtual TransactionAccount           TransactionAccount   { get; set; } = default!;
    public virtual Transaction?                 Parent               { get; set; }
    public virtual List<Transaction>?           ChildTransactions    { get; set; }
    public virtual List<RefundHistory>?         RefundHistories      { get; set; }
    public virtual List<TransactionTagging>?    Tagging              { get; set; }
    #endregion
}
