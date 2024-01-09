using XploringMe.Core.Enumerations.Finance;

namespace XploringMe.Core.Models.Entities.Finance.Transactions;

public class TransactionAccount : Auditable
{
    public string           AccountNumber   { get; set; } = default!;
    public decimal          Balance         { get; set; }
    public AccountType      AccountType     { get; set; }
    public decimal          InitialBalance  { get; set; }
    public bool             IsActive        { get; set; }
    public string           Name            { get; set; } = default!;
    public int              PaymentDueDay   { get; set; }
    public int              StatementDay    { get; set; }
}