namespace XploringMe.BlazorApp.Components.Pages.Transactions;

public partial class RefundComponent
{
    [Parameter]
    public EventCallback<bool> ProcessRefundPopup { get; set; }
    [Parameter]
    public long TransactionAccountId { get; set; }

    [Parameter]
    public long TransactionId { get; set; }
    public string PopupClass { get; set; } = "popup";
    public string OverlayClass { get; set; } = "overlay";
    public List<TransactionAccountDto> Accounts { get; set; } = [];
    public RefundDto Model { get; set; } = new();
    private CustomValidation? CustomValidation;

    string? color;
    protected override void OnInitialized()
    {
        var accounts = AccountRepo.FindAll(e => e.DeletedOn == null)
                                         .ResultObject.OrderByDescending(o => o.IsActive).ThenBy(o => o.Name);
        Accounts = Mapper.Map<List<TransactionAccountDto>>(accounts);
        color = Accounts.Find(e => e.Id == TransactionAccountId)?.AccountType.GetAccountColor();
    }

    public async Task Submit()
    {
        if (Model.Amount.HasValue && Model.Amount == 0)
        {
            CustomValidation?.ClearErrors();
            var errors = new Dictionary<string, List<string>>
            {
                {
                    nameof(Model.Amount),
                    ["Invalid refund amount"]
                }
            };
            CustomValidation?.DisplayErrors(errors);
            return;
        }
        if (!Model.RefundDate.HasValue || !Model.Amount.HasValue) { return; }
        Expression<Func<Transaction, bool>> filter = e => e.DeletedOn == null
                                                       && e.Id == TransactionId;

        var expenseResult = ExpenseRepo.FindAll(filter, includeProperties: "TransactionAccount");
        if (expenseResult.HasErrors)
        {
            await NotificationService.Error(expenseResult.GetErrors());
            return;
        }

        var expense = expenseResult.ResultObject.FirstOrDefault();
        if (expense == null)
        {
            await NotificationService.Error("Parent transaction not fount");
            return;
        }

        var rft = RefundRepo.FindAll(e => e.TransactionId == expense.Id).ResultObject.Sum(e => e.Refund);
        var rfable = rft + Model.Amount.Value;

        if ((rfable > expense.Debit && expense.TransactionType == TransactionType.Expense)
            || (rfable > expense.Credit && (expense.TransactionType == TransactionType.Income || expense.TransactionType == TransactionType.Transfer)))            
        {
            await NotificationService.Error("Refund amount exceded limit");
            return;
        }
        if(expense.TransactionDate > Model.RefundDate)
        {
            await NotificationService.Error("Refund date can not be less than transaction date");
            return;
        }
        var total = expense.TransactionType == TransactionType.Income ? expense.Credit : expense.Debit;
        if(expense.TransactionType == TransactionType.Transfer) total = expense.Credit;
        total -= rft;
        RefundHistory refund = new()
        {
            Date            = Model.RefundDate.Value,
            Refund          = Model.Amount.Value,
            Balance         = total - Model.Amount.Value,
            Total           = total,
            Remark          = Model.Remark,
            TransactionId   = expense.Id
        };

        var bankAccountResult = AccountRepo.FindAll(e => e.Id == Model.RefundAccountId && e.DeletedOn == null);

        if (bankAccountResult.HasErrors)
        {
            await NotificationService.Error(bankAccountResult.GetErrors());
            return;
        }

        var bankAccount = bankAccountResult.ResultObject.FirstOrDefault();
        if (bankAccount == null)
        {
            await NotificationService.Error("Refund Account not found");
            return;
        }
        bankAccount.Balance += Model.Amount.Value;

        var bankAccountUpdateResult = AccountRepo.Update(bankAccount, 1);

        if (bankAccountUpdateResult.HasErrors)
        {
            await NotificationService.Error(bankAccountUpdateResult.GetErrors());
            return;
        }
        Transaction expenseRefund = new()
        {
            TransactionAccountId    = bankAccount.Id,
            CategoryId              = expense.CategoryId,
            Credit                  = expense.TransactionType == TransactionType.Income ? 0 : Model.Amount.Value,
            CurrentBalance          = bankAccount.Balance + Model.Amount.Value,
            Debit                   = expense.TransactionType == TransactionType.Income ? Model.Amount.Value : 0,
            TransactionType         = expense.TransactionType,
            ParentId                = expense.Id,
            Particular              = Model.Remark,
            PaymentDate             = Model.RefundDate.Value,
            PreviousBalance         = bankAccount.Balance,
            TransactionDate         = Model.RefundDate.Value,
            Source                  = TransactionSource.Refund,
        };
        var createResult = ExpenseRepo.Create(expenseRefund, 1);
        if (createResult.HasErrors)
        {
            await NotificationService.Error(createResult.GetErrors());
            return;
        }
        //  BackgroundJob.Enqueue<IReportConductor>(x => x.ValidateTransactions(expense.CreatedById, expense.TransactionAccountId, expense.TransactionDate));

        refund.RefundAccountId = bankAccount.Id;
        #region Refund History

        var historyResult = RefundRepo.Create(refund, 1);
        if (historyResult.HasErrors)
        {
            await NotificationService.Error(historyResult.GetErrors());
            return;
        }
        expense.IsRefunded = true;
        var updateResult = ExpenseRepo.Update(expense, 1);
        if (updateResult.HasErrors)
        {
            await NotificationService.Error(updateResult.GetErrors());
            return;
        }
        #endregion

        #region Update budget
        // BackgroundJob.Enqueue<IBudgetConductor>(x => x.ProcessBudget(expense.TransactionDate, expense.CreatedById));
        //  if (expense.TransactionAccount != null)
        //      BackgroundJob.Enqueue<IReportConductor>(x => x.ValidateTransactions(expense.CreatedById, expense.TransactionAccountId, expense.TransactionDate));
        #endregion
        await NotificationService.Success("Refund processed successfully.");
        await ProcessRefundPopup.InvokeAsync(false);
    }
    public void Close() { }
}

public class RefundDto
{
    [Required(ErrorMessage = "Refund amount is required")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "RefundAccountId is required")]
    public long? RefundAccountId { get; set; }

    [Required(ErrorMessage = "RefundDate is required")]
    public DateTimeOffset? RefundDate { get; set; }

    [Required(ErrorMessage = "Refund Remark is required")]
    public string Remark { get; set; } = default!;
}
