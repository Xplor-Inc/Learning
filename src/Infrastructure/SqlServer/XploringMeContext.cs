using XploringMe.Core.Interfaces.Data;
using XploringMe.SqlServer.Extensions;
using XploringMe.SqlServer.Maps.Audits;
using XploringMe.SqlServer.Maps.Counters;
using XploringMe.SqlServer.Maps.Enquiries;
using XploringMe.SqlServer.Maps.Finance.Budgets;
using XploringMe.SqlServer.Maps.Finance.Categories;
using XploringMe.SqlServer.Maps.Finance.Tags;
using XploringMe.SqlServer.Maps.Finance.Transactions;
using XploringMe.SqlServer.Maps.Users;

namespace XploringMe.SqlServer;
public class XploringMeContext : DataContext<User>, IXploringMeContext
{
    #region Properties
    public DbSet<AccountRecovery>           AccountRecoveries           { get; set; }
    public DbSet<Counter>                   Counters                    { get; set; } 
    public DbSet<Enquiry>                   Enquiries                   { get; set; }
    public DbSet<ResumeDownload>            ResumeDownloads             { get; set; }
    public DbSet<UserLogin>                 UserLogins                  { get; set; }

    #region Finance
    public DbSet<Category>                  Categories                  { get; set; }
    public DbSet<TransactionAccount>        TransactionAccounts         { get; set; }
    public DbSet<Transaction>               Transactions                { get; set; }
    public DbSet<RefundHistory>             RefundHistories             { get; set; }
    public DbSet<Budget>                    Budgets                     { get; set; }
    public DbSet<RecurringBill>             RecurringBills              { get; set; }
    public DbSet<TransactionTagging>        Taggings                    { get; set; }
    #endregion

    #endregion

    #region Constructor
    public XploringMeContext(string connectionString, ILoggerFactory loggerFactory)
        : base(connectionString, loggerFactory)
    {
    }

    public XploringMeContext(IConnection connection, ILoggerFactory loggerFactory)
        : base(connection, loggerFactory)
    {
    }

    #endregion

    #region IXploringMeContext Implementation
    IQueryable<AccountRecovery>         IXploringMeContext.AccountRecoveries        => AccountRecoveries;
    IQueryable<ChangeLog>               IXploringMeContext.ChangeLogs               => ChangeLogs;
    IQueryable<Counter>                 IXploringMeContext.Counters                 => Counters;
    IQueryable<Enquiry>                 IXploringMeContext.Enquiries                => Enquiries;
    IQueryable<ResumeDownload>          IXploringMeContext.ResumeDownloads          => ResumeDownloads;
    IQueryable<UserLogin>               IXploringMeContext.UserLogins               => UserLogins;

    #region Finance
    IQueryable<Category>                IXploringMeContext.Categories               => Categories;
    IQueryable<TransactionAccount>      IXploringMeContext.TransactionAccounts      => TransactionAccounts;
    IQueryable<Transaction>             IXploringMeContext.Transactions             => Transactions;
    IQueryable<RefundHistory>           IXploringMeContext.RefundHistories          => RefundHistories;
    IQueryable<Budget>                  IXploringMeContext.Budgets                  => Budgets;
    IQueryable<RecurringBill>           IXploringMeContext.RecurringBills           => RecurringBills;
    IQueryable<TransactionTagging>      IXploringMeContext.Taggings                 => Taggings;
    #endregion


    #endregion

    public override void ConfigureMappings(ModelBuilder modelBuilder)
    {
        modelBuilder.AddMapping(new AccountRecoveryMap());
        modelBuilder.AddMapping(new ChangeLogMap());
        modelBuilder.AddMapping(new CounterMap());
        modelBuilder.AddMapping(new EnquiryMap());
        //modelBuilder.AddMapping(new MemberMap());
        //modelBuilder.AddMapping(new GotraMap());
        modelBuilder.AddMapping(new UserMap());
        modelBuilder.AddMapping(new UserLoginMap());

        //Financial Management
        modelBuilder.AddMapping(new CategoryMap());
        modelBuilder.AddMapping(new TransactionAccountMap());
        modelBuilder.AddMapping(new TransactionMap());
        modelBuilder.AddMapping(new RefundHistoryMap());
        modelBuilder.AddMapping(new BudgetMap());
        modelBuilder.AddMapping(new RecurringBillMap());
        modelBuilder.AddMapping(new TransactionTaggingMap());

        //modelBuilder.AddMapping(new DematAccountMap());
        //modelBuilder.AddMapping(new StockMap());
        //modelBuilder.AddMapping(new ScripMap());
        //modelBuilder.AddMapping(new MandateMap());
        //modelBuilder.AddMapping(new SchemeInvestmentMap());
        //modelBuilder.AddMapping(new SchemeMap());
        base.ConfigureMappings(modelBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}