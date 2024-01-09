namespace XploringMe.BlazorApp.Components.Pages.Accounts;

public partial class Create 
{
    public AccountType? SelectedAccountType { get; set; }
    public List<TransactionAccountDto> SelectedAccounts = [];
    private long? TransactionAaccountId;

    protected TransactionAccountDto Model { get; set; } = new();
    public List<TransactionAccountDto> Accounts = [];
    private List<CategoryDto> Categories = [];
    private List<TransactionDto> Expenses = [];

    private bool AddAccount;
    private bool EditAccount;
    private bool CreateTransaction;
    private bool LoadingPage = true;

    private void CreateAccountPopup(bool adding)
    {
        AddAccount = adding;
        Model = new TransactionAccountDto();
    }
    protected async Task FilterAccounts(AccountType type)
    {
        SelectedAccounts = Accounts.FindAll(x => x.AccountType == type);
        SelectedAccountType = type;
        await SearchData(SelectedAccountType);
    }
    protected override async Task OnInitializedAsync()
    {
        var cats = CatRepo.FindAll(e => e.DeletedOn == null)
                  .ResultObject.OrderBy(o => o.Name);

        Categories = Mapper.Map<List<CategoryDto>>(cats);
        await GetAccounts();
        await SearchData(SelectedAccountType);
        LoadingPage = false;
        await base.OnInitializedAsync();
    }
    protected async Task CloseTransactionPopup(bool refreshData)
    {
        if(refreshData)
            await SearchData(SelectedAccountType);
        CreateTransaction = false; 
        TransactionAaccountId = null;
    }
    protected async Task GetAccounts()
    {
        var accounts = await AccountRepo.FindAll(e => e.DeletedOn == null, orderBy:e=>e.OrderBy("IsActive").ThenBy("Name"))
                                .ResultObject.ToListAsync();
        Accounts = SelectedAccounts = Mapper.Map<List<TransactionAccountDto>>(accounts);
        if(SelectedAccountType.HasValue)
            SelectedAccounts = Accounts.FindAll(x => x.AccountType == SelectedAccountType);
    }
    private async void EditAccountPopup(bool editing, TransactionAccountDto? dto)
    {
        Model = dto ?? new TransactionAccountDto();
        EditAccount = editing;
        if (!editing)
           await GetAccounts();
    }
    private void OpenPopup()
    {
        CreateTransaction = true;
        Model = new TransactionAccountDto();
    }
    public async Task SearchData(AccountType? accountType)
    {
        DateTimeOffset today = DateTimeOffset.Now;
        Expression<Func<Transaction, bool>> filter = e => e.DeletedOn == null && e.CreatedById == 1;
        if (accountType.HasValue)
        {
            filter = filter.AndAlso(e => e.TransactionAccount.AccountType == accountType);
        }

        filter = filter.AndAlso(e => e.TransactionDate >= today.AddDays(-today.Day));
        filter = filter.AndAlso(e => e.TransactionDate <= today);

        var expensesResult = ExpenseRepo.FindAll(filter: filter,
                                                 orderBy: e => e.OrderBy("CreatedOn", "DESC"),
                                                 asNoTracking: true,
                                                 includeProperties: GetIncludeProperties(false, false).Join(","),
                                                 take: 100);
        if (expensesResult.HasErrors)
        {
            var error = expensesResult.GetErrors();
            await NotificationService.Error(error);
            LoadingPage = false;
            return;
        }

        var expenses = await expensesResult.ResultObject.ToListAsync();
        Expenses = Mapper.Map<List<TransactionDto>>(expenses);
    }
    protected async Task Submit()
    {
        if (Model != null)
        {
            var ifExists = AccountRepo.FindAll(e => e.Name == Model.Name && e.AccountNumber == Model.AccountNumber).ResultObject.Any();
            if (ifExists)
            {
                await NotificationService.Error($"Account {Model.Name} already exists");
            }
            var account = Mapper.Map<TransactionAccount>(Model);
            account.Balance = account.OpeningBalance;
            var result = AccountRepo.Create(account, 1);
            if (result.HasErrors)
            {
                await NotificationService.Error($"Error : {result.GetErrors()}");
                return;
            }
            await NotificationService.Success("Account created successfully");
            await GetAccounts();
            CreateAccountPopup(false);
            return;
        }
    }
    protected async Task Delete(long id)
    {
        if (id > 0)
        {
            var account = AccountRepo.FindAll(e => e.Id == id).ResultObject.FirstOrDefault();
            if (account == null)
            {
                await NotificationService.Error($"Account {Model.Name} not exists");
                return;
            }
            var result = AccountRepo.Delete(account, 1);
            if (result.HasErrors)
            {
                await NotificationService.Error($"Error : {result.GetErrors()}");
                return;
            }
            await NotificationService.Success("Account deleted successfully");
            await GetAccounts();
        }
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
