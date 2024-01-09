namespace XploringMe.BlazorApp.Components.Pages.Accounts;

public partial class Edit
{

    [Parameter]
    public EventCallback CloseForm { get; set; }

    [Parameter]
    public TransactionAccountDto Model { get; set; } = new();
    protected string EditingAccountName = string.Empty;
    protected Validations CustomValidation = new();

    protected string GetCardType()
    {
        if (Model.AccountType == AccountType.Saving)
            return "Debit";
        if (Model.AccountType == AccountType.CreditCard)
            return "Credit";
        return "";
    }
    protected override Task OnParametersSetAsync()
    {
        EditingAccountName = Model.Name;
        return base.OnParametersSetAsync();
    }
    protected async Task Update()
    {
        if (!await CustomValidation.ValidateAll()) { return; }
        var ifExists = await AccountRepo.FindAll(e => e.Name == Model.Name && e.AccountNumber == Model.AccountNumber && e.Id != Model.Id)
                                .ResultObject.AnyAsync();
        if (ifExists)
        {
            await NotificationService.Error($"Account {Model.Name} already exists");
            return;
        }
        var account = AccountRepo.FindAll(e => e.Id == Model.Id).ResultObject.FirstOrDefault();
        if (account == null)
        {
            await NotificationService.Error($"Account {Model.Name} not exists");
            return;
        }
        account.Name = Model.Name;
        account.IsActive = Model.IsActive;
        account.OpeningBalance = Model.OpeningBalance;
        account.AccountNumber = Model.AccountNumber;
        account.AccountType = Model.AccountType;
        account.Balance = Model.Balance;
        account.StatementDay = Model.StatementDay;
        account.DebitCardCVV = Model.DebitCardCVV;
        account.DebitCardExpireDate = Model.DebitCardExpireDate;
        account.DebitCardNo = Model.DebitCardNo;
        account.MobileBankingPIN = Model.MobileBankingPIN;
        account.DebitCardPIN = Model.DebitCardPIN;
        account.NetBankingPassword = Model.NetBankingPassword;
        account.NetBankingTransPassword = Model.NetBankingTransPassword;
        account.NetBankingURL = Model.NetBankingURL;
        account.NetBankingUserId = Model.NetBankingUserId;
        account.UPIPIN = Model.UPIPIN;
        var result = AccountRepo.Update(account, 1);
        if (result.HasErrors)
        {
            await NotificationService.Error($"Error : {result.GetErrors()}");
            return;
        }
        await NotificationService.Success("Account updated successfully");
        await CloseForm.InvokeAsync(true);
    }

}
