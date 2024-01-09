using XploringMe.Core.Models.Entities.Audits;
using XploringMe.Core.Models.Entities.Counters;
using XploringMe.Core.Models.Entities.Enquiries;
using XploringMe.Core.Models.Entities.Finance.Budgets;
using XploringMe.Core.Models.Entities.Finance.Categories;
using XploringMe.Core.Models.Entities.Finance.Tags;
using XploringMe.Core.Models.Entities.Finance.Transactions;
using XploringMe.Core.Models.Entities.Users;

namespace XploringMe.Core.Interfaces.Data;
public interface IXploringMeContext : IContext
{
    IQueryable<AccountRecovery>           AccountRecoveries           { get; }
    IQueryable<ChangeLog>                 ChangeLogs                  { get; }
    IQueryable<Counter>                   Counters                    { get; }
    IQueryable<Enquiry>                   Enquiries                   { get; }
    IQueryable<ResumeDownload>            ResumeDownloads             { get; }
    IQueryable<UserLogin>                 UserLogins                  { get; }

    #region Finance
    IQueryable<Category>                  Categories                  { get; }
    IQueryable<TransactionAccount>        TransactionAccounts         { get; }
    IQueryable<Transaction>               Transactions                { get; }
    IQueryable<RefundHistory>             RefundHistories             { get; }
    IQueryable<Budget>                    Budgets                     { get; }
    IQueryable<RecurringBill>             RecurringBills              { get; }
    IQueryable<TransactionTagging>        Taggings                    { get; }

    #endregion

}