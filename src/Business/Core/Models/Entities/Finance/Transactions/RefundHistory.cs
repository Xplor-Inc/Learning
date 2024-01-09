namespace XploringMe.Core.Models.Entities.Finance.Transactions;

public class RefundHistory : Entity
{
    public decimal          Total               { get; set; }
    public decimal          Balance             { get; set; }
    public DateTimeOffset   Date                { get; set; }
    public decimal          Refund              { get; set; }
    public long?            RefundAccountId     { get; set; }
    public string           Remark              { get; set; } = string.Empty;
    public long             TransactionId       { get; set; }

    public TransactionAccount?  RefundAccount   { get; set; }
    public Transaction          Transaction     { get; set; } = default!;
}
