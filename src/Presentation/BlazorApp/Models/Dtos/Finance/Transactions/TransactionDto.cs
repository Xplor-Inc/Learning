namespace XploringMe.BlazorApp.Models.Dtos.Finance.Transactions;
public class TransactionDto : AuditableDto
{
    public decimal              Credit                  { get; set; }
    public decimal              Debit                   { get; set; }
    public long                 TransactionAccountId    { get; set; }
    public long                 CategoryId              { get; set; }
    public string               Particular              { get; set; } = default!;
    public long?                CreditAccountId         { get; set; }
    public decimal              CurrentBalance          { get; set; }
    public string?              InvoicePath             { get; set; }
    public long?                ParentId                { get; set; }
    public bool                 IsRefunded              { get; set; }
    public DateTimeOffset       PaymentDate             { get; set; }
    public decimal              PreviousBalance         { get; set; }
    public DateTimeOffset       TransactionDate         { get; set; }
    public TransactionType      TransactionType         { get; set; }
    public TransactionSource    Source                  { get; set; }
    public DebtStatus           DebtStatus              { get; set; }

    #region Navigation Properties
    public CategoryDto?                 Category            { get; set; }
    public List<RefundHistoryDto>?      RefundHistories     { get; set; }
    public List<long>                   TagIds              { get; set; } = [];
    public List<string>                 TagNames            { get; set; } = [];
    public TransactionAccountDto?       TransactionAccount  { get; set; }
    public List<TransactionDto>?        ChildTransactions   { get; set; }
    public List<TransactionTaggingDto>  Tagging             { get; set; } = [];

    #endregion
}