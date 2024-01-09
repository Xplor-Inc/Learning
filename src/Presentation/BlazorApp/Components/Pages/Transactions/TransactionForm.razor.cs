namespace XploringMe.BlazorApp.Components.Pages.Transactions;

public partial class TransactionForm
{
    [Parameter]
    public long? TransactionAccountId { get; set; }

    [Parameter]
    public List<TransactionAccountDto> Accounts { get; set; } = [];

    [Parameter]
    public List<CategoryDto> Categories { get; set; } = [];

    [Parameter]
    public EventCallback<bool> CloseForm { get; set; }

    [Parameter]
    public TransactionDto? Model { get; set; } = default!;
    private List<string> Remarks { get; set; } = [];
    private Validations CustomValidation = new();
    private bool EditingForm { get; set; }


    protected override Task OnInitializedAsync()
    {
        if (Model == null)
        {
            Model = new TransactionDto ();
            if(TransactionAccountId.HasValue)
                Model.TransactionAccountId = TransactionAccountId.Value;
        }
        else EditingForm = true;
        Remarks = [.. ExpenseRepo.FindAll(e => e.DeletedOn == null).ResultObject.Select(e => e.Particular).Distinct()];
        return base.OnInitializedAsync();
    }

    protected async Task Submit(bool closeForm)
    {
        if (Model == null) return;
        if (!await CustomValidation.ValidateAll())
        {
            return;
        }
        var errors = new Dictionary<string, List<string>>();
        CustomValidation?.ClearAll();
        if (Model.Debit < 1 && Model.TransactionType != TransactionType.Income)
        {
            errors.Add(nameof(Model.Debit),
                            ["Enter a valid amount"]);
            // CustomValidation?.DisplayErrors(errors);
            return;
        }

        var account = AccountRepo.FindAll(e => e.Id == Model.TransactionAccountId).ResultObject.FirstOrDefault();
        if (account == null)
        {
            errors.Add(nameof(Model.TransactionAccountId),
                            ["Selected account is not available."]);
            // CustomValidation?.DisplayErrors(errors);
            return;

        }
        if (account.Balance < Model.Debit && Model.TransactionType != TransactionType.Income)
        {
            errors.Add(nameof(Model.Debit),
                            ["Selected account doesn't have sufficiant balance"]);
            // CustomValidation?.DisplayErrors(errors);
            return;
        }

        if (Model.TransactionType == TransactionType.Transfer && !Model.CreditAccountId.HasValue)
        {
            errors.Add(nameof(Model.Debit),
                            ["Provide a credit account to transfer balance"]);
            //CustomValidation?.DisplayErrors(errors);
            return;
        }
        var expense = Mapper.Map<Transaction>(Model);

        expense.PreviousBalance = account.Balance;
        expense.CurrentBalance  = account.Balance - Model.Debit + Model.Credit;
        expense.Source          = TransactionSource.Transaction;

        var result = ExpenseRepo.Create(expense, 1);
        if (result.HasErrors)
        {
            var error = result.GetErrors();
            await NotificationService.Error(error);
            return;
        }
        account.Balance += expense.Credit - expense.Debit;

        var accountUpdate = AccountRepo.Update(account, 1);
        if (accountUpdate.HasErrors)
        {
            var error = accountUpdate.GetErrors();
            await NotificationService.Error(error);
            return;
        }
        Accounts.First(e => e.Id == account.Id).Balance = account.Balance;

        if (Model.TransactionType != TransactionType.Transfer)
        {
            await NotificationService.Success($"{Model.TransactionType} : added successfully");
        }
        if (Model.TransactionType == TransactionType.Transfer && Model.CreditAccountId.HasValue)
        {
            var creditAccount = AccountRepo.FindAll(e => e.Id == Model.CreditAccountId).ResultObject.FirstOrDefault();
            if (creditAccount == null)
            {
                errors.Add(nameof(Model.CreditAccountId),
                                ["Selected credit account is not available."]);
                // CustomValidation?.DisplayErrors(errors);
                return;
            }

            var creditTrans = new Transaction
            {
                ParentId                = expense.Id,
                CreditAccountId         = null,
                DebitAccountId          = Model.TransactionAccountId,
                TransactionAccountId    = Model.CreditAccountId.Value,
                CategoryId              = Model.CategoryId,
                Credit                  = Model.Debit,
                Particular              = $"From {account.Name} for {Model.Particular}",
                PaymentDate             = Model.PaymentDate,
                TransactionDate         = Model.TransactionDate,
                TransactionType         = Model.TransactionType,
                PreviousBalance         = creditAccount.Balance,
                CurrentBalance          = creditAccount.Balance + Model.Debit,
                Source                  = TransactionSource.Transaction
            };
            var creditResult = ExpenseRepo.Create(creditTrans, 1);
            if (creditResult.HasErrors)
            {
                var error = creditResult.GetErrors();
                await NotificationService.Error(error);
                return;
            }
            creditAccount.Balance += expense.Debit;
            var creditAccountUpdate = AccountRepo.Update(creditAccount, 1);
            if (creditAccountUpdate.HasErrors)
            {
                var error = creditAccountUpdate.GetErrors();
                await NotificationService.Error(error);
                return;
            }

            await NotificationService.Success($"Balance transfered successfully to {creditAccount.Name} from {account.Name}");
            expense.ParentId = creditTrans.Id;
            expense.Particular = $"To {creditAccount.Name} for {Model.Particular}";
            var updateExResult = ExpenseRepo.Update(expense, 1);
            if (updateExResult.HasErrors)
            {
                var error = updateExResult.GetErrors();
                await NotificationService.Error(error);
                return;
            }
            Accounts.First(e => e.Id == creditAccount.Id).Balance = creditAccount.Balance;
        }

        if (Model.TagIds.Count > 0)
            await CreateTags(Model.TagIds, expense.Id);

        Model = new TransactionDto();
        if (TransactionAccountId.HasValue)
            Model.TransactionAccountId = TransactionAccountId.Value;
        if (!Remarks.Any(e => string.Equals(Model.Particular, e, StringComparison.CurrentCultureIgnoreCase)))
        {
            Remarks.Add(Model.Particular);
            Remarks.Sort();
        }
        if (closeForm)
            await CloseForm.InvokeAsync(closeForm);
    }

    public async Task Update()
    {
        if (Model == null) return;
        var transaction = ExpenseRepo.FindAll(e => e.DeletedOn == null && e.Id == Model.Id).ResultObject.FirstOrDefault();
        if (transaction == null)
        {
            await NotificationService.Error("Transaction not found"); return;
        }
        var errors = new Dictionary<string, List<string>>();
        // CustomValidation?.ClearErrors();
        if (Model.Debit < 1 && Model.Credit < 1)
        {
            errors.Add(nameof(Model.Debit),
                            ["Enter a valid amount"]);
            // CustomValidation?.DisplayErrors(errors);
            return;
        }

        var account = AccountRepo.FindAll(e => e.Id == Model.TransactionAccountId).ResultObject.FirstOrDefault();
        if (account == null)
        {
            errors.Add(nameof(Model.TransactionAccountId),
                            ["Selected account is not available."]);
            //  CustomValidation?.DisplayErrors(errors);
            return;
        }
        if ((Model.Debit - transaction.Debit) > account.Balance && Model.TransactionType == TransactionType.Expense)
        {
            errors.Add(nameof(Model.Debit),
                            ["Selected account doesn't have sufficiant balance"]);
            //  CustomValidation?.DisplayErrors(errors);
            return;
        }
        account.Balance += transaction.Debit + transaction.Credit - Model.Debit;

        transaction.TransactionAccountId    = Model.TransactionAccountId;
        transaction.CategoryId              = Model.CategoryId;
        transaction.Debit = Model.Debit;
        transaction.Credit = Model.Credit;
        transaction.Particular = Model.Particular;
        transaction.PaymentDate = Model.PaymentDate;
        transaction.TransactionDate = Model.TransactionDate;
        transaction.TransactionType = Model.TransactionType;
        transaction.PreviousBalance = account.Balance + Model.Debit;
        transaction.CurrentBalance = account.Balance;

        if (transaction.TransactionType == TransactionType.Income)
        {
            transaction.Credit = transaction.Debit;
            transaction.Debit = 0;
            transaction.CurrentBalance = account.Balance + Model.Debit;
        }
        var result = ExpenseRepo.Update(transaction, 1);
        if (result.HasErrors)
        {
            var error = result.GetErrors();
            await NotificationService.Error(error);
            return;
        }

        var accountUpdate = AccountRepo.Update(account, 1);
        if (accountUpdate.HasErrors)
        {
            var error = accountUpdate.GetErrors();
            await NotificationService.Error(error);
            return;
        }
        Accounts.First(e => e.Id == account.Id).Balance = account.Balance;

        if (Model.TagIds.Count > 0)
            await CreateTags(Model.TagIds, transaction.Id);

        if (Model.TransactionType == TransactionType.Transfer && transaction.ParentId.HasValue)
        {
            var crTransaction = ExpenseRepo.FindAll(e => e.DeletedOn == null && e.Id == transaction.ParentId).ResultObject.FirstOrDefault();
            if (crTransaction == null)
            {
                await NotificationService.Error("Transaction not found"); return;
            }
            var creditAccount = AccountRepo.FindAll(e => e.Id == crTransaction.TransactionAccountId).ResultObject.FirstOrDefault();
            if (creditAccount == null)
            {
                errors.Add(nameof(Model.CreditAccountId),
                                ["Selected credit account is not available."]);
                // CustomValidation?.DisplayErrors(errors);
                return;
            }
            creditAccount.Balance += crTransaction.Debit + crTransaction.Credit - Model.Debit;

            crTransaction.CategoryId = Model.CategoryId;
            crTransaction.Credit = Model.Debit;
            crTransaction.Debit = Model.Credit;
            // crTransaction.Particular = Model.Particular;
            crTransaction.PaymentDate = Model.PaymentDate;
            crTransaction.TransactionDate = Model.TransactionDate;
            crTransaction.TransactionType = Model.TransactionType;
            crTransaction.PreviousBalance = creditAccount.Balance;
            crTransaction.CurrentBalance = creditAccount.Balance + Model.Debit;

            var creditResult = ExpenseRepo.Update(crTransaction, 1);
            if (creditResult.HasErrors)
            {
                var error = creditResult.GetErrors();
                await NotificationService.Error(error);
                return;
            }
            var creditAccountUpdate = AccountRepo.Update(creditAccount, 1);
            if (creditAccountUpdate.HasErrors)
            {
                var error = creditAccountUpdate.GetErrors();
                await NotificationService.Error(error);
                return;
            }

            Accounts.First(e => e.Id == creditAccount.Id).Balance = creditAccount.Balance;
        }
        Model = new TransactionDto();
        if (TransactionAccountId.HasValue)
            Model.TransactionAccountId = TransactionAccountId.Value;

        await NotificationService.Success($"Transaction update successfully");
        EditingForm = false;
        await CloseForm.InvokeAsync(true);
    }
    public async Task CreateTags(List<long> tags, long expenseId)
    {
        var dbTags = TaggingRepo.FindAll(e => e.TransactionId == expenseId).ResultObject.ToList();
        foreach (var dbTag in dbTags)
        {
            if (!tags.Contains(dbTag.TagId))
            {
                TaggingRepo.Delete(dbTag.Id, 1, false);
            }
        }
        var newTags = tags.FindAll(e => !dbTags.Select(s => s.TagId).Contains(e));

        var lidtTags = new List<TransactionTagging>();
        foreach (var tag in newTags)
        {
            lidtTags.Add(new TransactionTagging { CreatedById = 1, CreatedOn = DateTimeOffset.Now, TagId = tag, TransactionId = expenseId });
        }
        var createTagResult = TaggingRepo.Create(lidtTags, 1);
        if (createTagResult.HasErrors)
        {
            await NotificationService.Error(createTagResult.GetErrors());
        }
    }
}
