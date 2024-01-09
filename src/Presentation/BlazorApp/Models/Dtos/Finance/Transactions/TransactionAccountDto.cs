namespace XploringMe.BlazorApp.Models.Dtos.Finance.Transactions;
public class TransactionAccountDto : AuditableDto
{
    public string           AccountNumber               { get; set; } = default!;
    public string           MobileNumber                { get; set; } = default!;
    public decimal          Balance                     { get; set; }
    public AccountType      AccountType                 { get; set; }
    public decimal          OpeningBalance              { get; set; }
    public bool             IsActive                    { get; set; }
    public decimal          LockBalance                 { get; set; }
    public DateTimeOffset   LockDate                    { get; set; }
    public string           Name                        { get; set; } = default!;
    public int              StatementDay                { get; set; }
    public string?          IFSCCode                    { get; set; }
    public string?          DebitCardNo                 { get; set; }
    public int?             DebitCardPIN                { get; set; }
    public int?             DebitCardCVV                { get; set; }
    public DateTimeOffset?  DebitCardExpireDate         { get; set; }
    public int?             MobileBankingPIN            { get; set; }
    public string?          UPIId                       { get; set; }
    public int?             UPIPIN                      { get; set; }
    public string?          NetBankingURL               { get; set; }
    public string?          NetBankingUserId            { get; set; }
    public string?          NetBankingPassword          { get; set; }
    public string?          NetBankingTransPassword     { get; set; }

}