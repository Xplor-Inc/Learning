namespace XploringMe.BlazorApp.Components.Pages.Home;

public partial class Dashboard
{
    private List<Data> Datas { get; set; } = [];

    public async Task Get()
    {
        var currentMonth = DateTimeOffset.Now;
        var data = ExpenseRepo.FindAll(e => e.DeletedOn == null);
        if(data.HasErrors)
        {
            var errors = data.GetErrors();
            await NotificationService.Error(errors);
        }
        var exp = await data.ResultObject.ToListAsync();

    }
    //void FilterMonthly (List<Transaction> transactions)
    //{
    //    var income = transactions.Where(x => x.TransactionType == TransactionType.Income).Sum(s => s.Credit);
    //    var exp = transactions.Where(x => x.TransactionType == TransactionType.Expense).Sum(s => s.Debit);
    //    var interestPaid = transactions.Where(x => x.TransactionType == TransactionType.Expense && x.CategoryId == 2).Sum(s => s.Debit);
    //   // var BalanceAtEOM = transactions.Where(x => x.TransactionType == TransactionType.Expense).Sum(s => s.Debit);
    //    var AmountReceivable = transactions.Where(x => x.TransactionAccount.AccountType == AccountType.Loan).Sum(s => s.Debit-s.Credit);
    //    var AmountPayable = transactions.Where(x => x.TransactionAccount.AccountType == AccountType.Loan).Sum(s => s.Credit - s.Debit);

    //    var Investments = transactions.Where(x => x.CategoryId == 1).Sum(s => s.Debit);
    //    var SpendByCredit = transactions.Where(x => x.TransactionAccount.AccountType == AccountType.CreditCard).Sum(s => s.Debit);
    //    var SpiritualSpends = transactions.Where(x => x.CategoryId == 2).Sum(s => s.Debit);

    //    var topExps = transactions.Where(x=>x.TransactionType == TransactionType.Expense).OrderByDescending(o=>o.Debit).Take(10).ToList();
    //}

    //void FilterMonthly(List<TransactionAccount> accounts)
    //{
    //    var BalanceAtEOM = accounts.Where(x => x.AccountType == AccountType.Saving || x.AccountType == AccountType.Cash).Sum(s => s.Balance);
    //    var CreditOutstanding = accounts.Where(x => x.AccountType == AccountType.CreditCard).Sum(s => s.Balance);
    //    //var interestPaid = accounts.Where(x => x.TransactionType == TransactionType.Expense && x.CategoryId == 2).Sum(s => s.Debit);
    //    //// var BalanceAtEOM = transactions.Where(x => x.TransactionType == TransactionType.Expense).Sum(s => s.Debit);
    //    //var AmountReceivable = accounts.Where(x => x.TransactionAccount.AccountType == AccountType.Loan).Sum(s => s.Debit - s.Credit);
    //    //var AmountPayable = accounts.Where(x => x.TransactionAccount.AccountType == AccountType.Loan).Sum(s => s.Credit - s.Debit);

    //    //var Investments = accounts.Where(x => x.CategoryId == 1).Sum(s => s.Debit);
    //    //var SpendByCredit = accounts.Where(x => x.TransactionAccount.AccountType == AccountType.CreditCard).Sum(s => s.Debit);
    //    //var SpiritualSpends = accounts.Where(x => x.CategoryId == 2).Sum(s => s.Debit);

    //    //var topExps = accounts.Where(x => x.TransactionType == TransactionType.Expense).OrderByDescending(o => o.Debit).Take(10).ToList();
    //}

    protected override void OnInitialized()
    {

        Datas.Add(new Data
        {
            Change = 4,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Total Income",
            Type = ChangeType.Positive,
            Value = 123456
        });
        Datas.Add(new Data
        {
            Change = 4,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Total Expenses",
            Type = ChangeType.Negative,
            Value = 12345
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = false,
            Title = "Interest Paid",
            Type = ChangeType.Positive,
            Value = 8764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Balance at EOM",
            Type = ChangeType.Positive,
            Value = 88764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Amount Receivable",
            Type = ChangeType.Positive,
            Value = 88764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Amount Payable",
            Type = ChangeType.Positive,
            Value = 88764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Credit Outstanding",
            Type = ChangeType.Positive,
            Value = 88764
        });
        Datas.Add(new Data
        {
            Change = 4,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Loan Outstanding",
            Type = ChangeType.Negative,
            Value = 123456
        });
        Datas.Add(new Data
        {
            Change = 4,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Fixed Liabilities",
            Type = ChangeType.Negative,
            Value = 12345
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = false,
            Title = "Total Investment",
            Type = ChangeType.Negative,
            Value = 8764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Spend by Credit",
            Type = ChangeType.Positive,
            Value = 88764
        });
        Datas.Add(new Data
        {
            Change = 20,
            SubText = "vs Prev Month",
            Success = true,
            Title = "Market Returns",
            Type = ChangeType.Positive,
            Value = 88764
        });

        base.OnInitialized();
    }
    //private IJSObjectReference? jsModule;

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    //if (firstRender)
    //    //{
    //    //    jsModule = await JS.InvokeAsync<IJSObjectReference>(
    //    //        "import", "./charts/pie.js");

    //    //    await jsModule.InvokeVoidAsync("drowPieChart", "chartdiv1");
    //    //    await jsModule.InvokeVoidAsync("drowPieChart", "chartdiv2");
    //    //    await jsModule.InvokeVoidAsync("drowPieChart", "chartdiv3");
    //    //    await jsModule.InvokeVoidAsync("drowPieChart", "chartdiv4");
    //    //    //  await jsModule.InvokeVoidAsync("drowPieChart", "chartdiv11");

    //    //    jsModule = await JS.InvokeAsync<IJSObjectReference>(
    //    //        "import", "./charts/LineChart.js");

    //    //    await jsModule.InvokeVoidAsync("drawLineChart", "chartdiv5");
    //    //}
    //}
}
