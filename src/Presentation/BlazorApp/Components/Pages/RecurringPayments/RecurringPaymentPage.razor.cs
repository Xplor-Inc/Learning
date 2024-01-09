namespace XploringMe.BlazorApp.Components.Pages.RecurringPayments;

public partial class RecurringPaymentPage
{
    public RecurringBillDto             Model       { get; set; } = default!;
    public bool                         EditingForm { get; set; }
    public List<TransactionAccountDto>  Accounts    { get; set; } = [];
    public List<RecurringBillDto>       Bills       { get; set; } = [];
    private Validations                 CustomValidation = new();
    protected override async Task OnInitializedAsync()
    {
        var accounts = AccountRepo.FindAll(e => e.DeletedOn == null)
                                     .ResultObject.OrderByDescending(o => o.IsActive).ThenBy(o => o.Name);
        Accounts = Mapper.Map<List<TransactionAccountDto>>(accounts);
        GetDate();
        await base.OnInitializedAsync();
    }

    protected void GetDate()
    {
        var bills = RPRepo.FindAll(e => e.DeletedOn == null, orderBy: e => e.OrderBy("BillName", "ASC")).ResultObject.ToList();
        Bills = Mapper.Map<List<RecurringBillDto>>(bills);
        Model = new RecurringBillDto();

    }
    protected async Task Submit()
    {
        if(! await CustomValidation.ValidateAll())
        {
            return;
        }
        var errors = new Dictionary<string, List<string>>();

        if (Model.Frequency == RecurringFrequency.Days && Model.NextPaymentDays < 1)
        {
            errors.Add(nameof(Model.NextPaymentDays),
                [ "For recurring type 'Days' " +
            "'NextPaymentDays' is required." ]);
        }

        if (errors.Count != 0)
        {
           // CustomValidation?.DisplayErrors(errors);
            return;
        }
        var ifExists = RPRepo.FindAll(e => e.BillName == Model.BillName && e.AccountNo == Model.AccountNo).ResultObject.Any();
        if (ifExists)
            {
                await NotificationService.Error($"Recurring Bill {Model.BillName} already exists");
            }

        var bill= Mapper.Map<RecurringBill>(Model);
        var result = RPRepo.Create(bill, 1);
        if (result.HasErrors)
        {
            await NotificationService.Error($"Error : {result.GetErrors()}");
            return;
        }
        await NotificationService.Success("Recurring Bill added successfully");
        GetDate();
        return;
    }

    protected async Task Update()
    {
        if (!await CustomValidation.ValidateAll())
        {
            return;
        }
        var ifExists = RPRepo.FindAll(e => e.BillName == Model.BillName && e.AccountNo == Model.AccountNo && e.Id != Model.Id)
                                .ResultObject.Any();
        if (ifExists)
        {
            await NotificationService.Error($"Recurring Bill {Model.BillName} already exists");
            return;
        }
        var bill = RPRepo.FindAll(e => e.Id == Model.Id).ResultObject.FirstOrDefault();
        if (bill is null)
        {
            await NotificationService.Error($"Recurring Bill {Model.BillName} not exists");
            return;
        }
        bill.BillName           = Model.BillName;
        bill.StartDate          = Model.StartDate;
        bill.DebitAccountId     = Model.AutoDebit ? Model.DebitAccountId : null;
        bill.Amount             = Model.Amount;
        bill.AutoDebit          = Model.AutoDebit;
        bill.Frequency          = Model.Frequency;
        bill.Paid               = Model.Paid;
        bill.NextPaymentDays    = Model.GetNextPaymentDays();
        var result = RPRepo.Update(bill, 1);
        if (result.HasErrors)
        {
            await NotificationService.Error($"Error : {result.GetErrors()}");
            return;
        }
        await NotificationService.Success("Recurring Bill updated successfully");
        GetDate();
        EditingForm = false;
    }

    protected async Task Delete(long id)
    {
        if (id > 0)
        {
            var bill = RPRepo.FindAll(e => e.Id == id).ResultObject.FirstOrDefault();
            if (bill == null)
            {
                await NotificationService.Error($"Recurring Bill {Model.BillName} not exists");
                return;
            }
            var result = RPRepo.Delete(bill, 1);
            if (result.HasErrors)
            {
                await NotificationService.Error($"Error : {result.GetErrors()}");
                return;
            }
            await NotificationService.Success("Recurring Bill deleted successfully");
            GetDate();
        }
    }

    public void Schedular()
    {
        var bills = RPRepo.FindAll(e => e.DeletedOn == null && e.StartDate.ToIst().Date == DateTimeOffset.Now.ToIst().Date).ResultObject.ToList();
        foreach ( var bill in bills )
        {
            if (bill.AutoDebit)
            {

            }
        }
    }
}
