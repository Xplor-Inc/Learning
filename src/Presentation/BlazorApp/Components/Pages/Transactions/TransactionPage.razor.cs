namespace XploringMe.BlazorApp.Components.Pages.Transactions;

public partial class TransactionPage
{
    private TransactionDto? Edit;
    private bool            OpenPopup;
    private bool            Loading     = true;
    private readonly SearchDto       SearchModel = new();
    private List<string>    Remarks = [];
    private List<TransactionAccountDto> Accounts    = [];
    private List<CategoryDto>           Categories  = [];
    private List<TransactionDto>        Expenses    = [];
    protected override async Task OnInitializedAsync()
    {
        var accounts = AccountRepo.FindAll(e => e.DeletedOn == null)
                                  .ResultObject.OrderByDescending(o => o.IsActive).ThenBy(o => o.Name);
        Accounts = Mapper.Map<List<TransactionAccountDto>>(accounts);

        var cats = CatRepo.FindAll(e => e.DeletedOn == null)
                          .ResultObject.OrderBy(o => o.Name);

        Categories = Mapper.Map<List<CategoryDto>>(cats);
        Remarks = [.. ExpenseRepo.FindAll(e => e.DeletedOn == null).ResultObject.Select(e => e.Particular).Distinct()];
        await SearchData();
        await base.OnInitializedAsync();
    }
    protected async Task CloseForm(bool refreshData)
    {
        OpenPopup = false;
        if (refreshData) await SearchData();
    }
    protected void OpenForm()
    {
        Edit       = null;
        OpenPopup   = true;
    }
    protected void OpenForm(TransactionDto dto)
    {
        Edit = dto;
        OpenPopup = true;
    }
    public async Task SearchData()
    {
        Expression<Func<Transaction, bool>> filter = e => e.DeletedOn == null && e.CreatedById == 1;
        if (SearchModel.Tags != null && SearchModel.Tags.Count > 0)
        {
            filter = filter.AndAlso(e => e.Tagging != null && e.Tagging.Any(e => SearchModel.Tags.Contains(e.TagId)));
        }
        if (SearchModel.DebtStatus.HasValue)
        {
            filter = filter.AndAlso(e => e.DebtStatus == SearchModel.DebtStatus);
        }
        if (SearchModel.CategoryId.HasValue)
        {
            filter = filter.AndAlso(e => e.Category != null && e.Category.Id == SearchModel.CategoryId);            
        }
        if (SearchModel.CreditAccountId.HasValue)
        {
            filter = filter.AndAlso(e => e.TransactionAccount != null && e.TransactionAccount.Id == SearchModel.CreditAccountId);
        }
        if (!string.IsNullOrEmpty(SearchModel.SearchText))
        {
            if (long.TryParse(SearchModel.SearchText, out long tId))
            {
                filter = filter.AndAlso(e => e.Id == tId || e.Particular.Contains(SearchModel.SearchText) || SearchModel.SearchText.Contains(e.Particular));
            }
            else
            {
                filter = filter.AndAlso(e => e.Particular.Contains(SearchModel.SearchText) || SearchModel.SearchText.Contains(e.Particular));
            }
        }        
        if (SearchModel.FromDate.HasValue)
        {
            filter = filter.AndAlso(e => e.TransactionDate >= SearchModel.FromDate.Value);
        }
        if (SearchModel.ToDate.HasValue)
        {
            filter = filter.AndAlso(e => e.TransactionDate <= SearchModel.ToDate.Value);
        }
        if (SearchModel.PaymentDate.HasValue)
        {
            filter = filter.AndAlso(e => e.PaymentDate >= SearchModel.PaymentDate.Value.AddDays(-1) && e.PaymentDate < SearchModel.PaymentDate.Value.AddDays(1));
        }
        var incomeExpenseFilter = filter;
        if (SearchModel.TransactionType.HasValue)
        {
            filter = filter.AndAlso(e => e.TransactionType == SearchModel.TransactionType);
        }

        var expensesResult = ExpenseRepo.FindAll(filter            : filter, 
                                                 orderBy           : e => e.OrderBy(SearchModel.SortBy, SearchModel.SortOrder)
                                                                           .ThenBy("CreatedOn", SearchModel.SortOrder),
                                                 asNoTracking      : true, 
                                                 includeProperties : GetIncludeProperties(false, false).Join(","), 
                                                 take              : SearchModel.PageSize,
                                                 skip              : SearchModel.Skip);
        if (expensesResult.HasErrors)
        {
            var error = expensesResult.GetErrors();
            await NotificationService.Error(error);
            Loading = false;
            return;
        }

        var expenses = await expensesResult.ResultObject.ToListAsync();
        expenses.ForEach(e =>
        {
            e.Tagging?.RemoveAll(e => e.DeletedOn.HasValue);
        });
        var rowsCount = ExpenseRepo.FindAll(filter).ResultObject.Count();
        SearchModel.Count = rowsCount;
        Expenses = Mapper.Map<List<TransactionDto>>(expenses);
        Loading = false;

        var expenseIncomeResult = ExpenseRepo.FindAll(incomeExpenseFilter)
                                                             .ResultObject
                                                             .Select(e => new
                                                             {
                                                                 e.Debit,
                                                                 e.Credit,
                                                                 e.TransactionType
                                                             })
                                                             .ToList();

        SearchModel.Expense = expenseIncomeResult.Where(e => e.TransactionType == TransactionType.Expense).Sum(a => a.Debit - a.Credit);
        SearchModel.Income  = expenseIncomeResult.Where(e => e.TransactionType == TransactionType.Income).Sum(a => a.Credit - a.Debit);
    }
    protected async Task NextPage()
    {
        Loading = true;
        SearchModel.Skip += SearchModel.PageSize;
        await SearchData();
    }
    protected async Task PrevPage()
    {
        Loading = true;
        SearchModel.Skip -= SearchModel.PageSize;
       await SearchData();
    }
    private static string[] GetIncludeProperties(bool? includeChilds = false, bool? includeRefunds = false)
    {
        var includeProperties = new List<string>
        {
            nameof(Transaction.Category),
            nameof(Transaction.TransactionAccount),
            nameof(Transaction.Tagging),
            "Tagging.Tag"
        };
        if (includeChilds.HasValue && includeChilds.Value)
        {
            includeProperties.Add("ChildTransactions.TransactionAccount");
        }
        if (includeRefunds.HasValue && includeRefunds.Value)
        {
            includeProperties.Add("RefundHistories.RefundAccount");
        }
        return [.. includeProperties];
    }

}
