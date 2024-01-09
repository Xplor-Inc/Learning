using Microsoft.AspNetCore.Components;

namespace XploringMe.BlazorApp.Components.Pages.Transactions;

public partial class ViewTransaction
{
    [Parameter]
    public long TransId { get; set; }
    public bool ProcessRefund { get; set; }
    public long? Balance { get; set; }
    private TransactionDto Model = new();
    private List<string> Remarks = [];
    private List<ChangeLog> Logs = [];

    public void ProcessRefundPopup(bool processRefund)
    {
        ProcessRefund = processRefund;
        GetLatestData();
    }
    protected override Task OnInitializedAsync()
    {
        GetLatestData();
        return base.OnInitializedAsync();
    }

    private void GetLatestData()
    {
        var mModel = ExpenseRepo.FindAll(e => e.DeletedOn == null && e.Id == TransId, orderBy: e => e.OrderBy("TransactionDate", "ASC"),
            includeProperties: "Category,TransactionAccount,RefundHistories,RefundHistories.RefundAccount")
                                .ResultObject.FirstOrDefault();
        Model = Mapper.Map<TransactionDto>(mModel);
                                

        Logs =  [.. LogRepo.FindAll(e => e.EntityName == nameof(Transaction) && e.PrimaryKey == TransId).ResultObject];

        var catIds = new List<long>();
        var actIds = new List<long>();
        var usrIds = new List<long>();

        catIds = [.. Logs.Where(e => !string.IsNullOrEmpty(e.OldValue) && long.TryParse(e.OldValue, out long _) && e.PropertyName == nameof(Transaction.CategoryId))
            .Select(s => long.Parse(s.OldValue ?? string.Empty)) ];
        catIds.AddRange([.. Logs.Where(e => !string.IsNullOrEmpty(e.NewValue) && long.TryParse(e.NewValue, out long _) && e.PropertyName == nameof(Transaction.CategoryId))
            .Select(s => long.Parse(s.NewValue ?? string.Empty))]);

        usrIds = [.. Logs.Where(e => !string.IsNullOrEmpty(e.OldValue) && long.TryParse(e.OldValue, out long _) && e.PropertyName == nameof(Transaction.UpdatedById))
            .Select(s => long.Parse(s.OldValue ?? string.Empty))];
        usrIds.AddRange([.. Logs.Where(e => !string.IsNullOrEmpty(e.NewValue) && long.TryParse(e.NewValue, out long _) && e.PropertyName == nameof(Transaction.UpdatedById))
            .Select(s => long.Parse(s.NewValue ?? string.Empty))]);

        actIds = [.. Logs.Where(e => !string.IsNullOrEmpty(e.OldValue) && long.TryParse(e.OldValue, out long _) && e.PropertyName == nameof(Transaction.TransactionAccountId))
            .Select(s => long.Parse(s.OldValue ?? string.Empty))];
        actIds.AddRange([.. Logs.Where(e => !string.IsNullOrEmpty(e.NewValue) && long.TryParse(e.NewValue, out long _) && e.PropertyName == nameof(Transaction.TransactionAccountId))
            .Select(s => long.Parse(s.NewValue ?? string.Empty))]);

        catIds = [.. catIds.Distinct()];
        usrIds = [.. usrIds.Distinct()];
        actIds = [.. actIds.Distinct()];


        if (catIds.Count > 0)
        {
            var cats = CatRepo.FindAll(e => catIds.Contains(e.Id)).ResultObject.ToList();
            foreach (var log in Logs.Where(e => e.PropertyName == nameof(Transaction.CategoryId)))
            {
                if (!string.IsNullOrEmpty(log.OldValue))
                    log.OldValue = cats.FirstOrDefault(e => e.Id == long.Parse(log.OldValue))?.Name;

                if (!string.IsNullOrEmpty(log.NewValue))
                    log.NewValue = cats.FirstOrDefault(e => e.Id == long.Parse(log.NewValue))?.Name;
            }
        }

        if (usrIds.Count > 0)
        {
            var users = UserRepo.FindAll(e => usrIds.Contains(e.Id)).ResultObject.ToList();
            foreach (var log in Logs.Where(e => e.PropertyName == nameof(Transaction.UpdatedById)))
            {
                    if (!string.IsNullOrEmpty(log.OldValue))
                        log.OldValue = users.FirstOrDefault(e => e.Id == long.Parse(log.OldValue))?.FirstName;

                    if (!string.IsNullOrEmpty(log.NewValue))
                        log.NewValue = users.FirstOrDefault(e => e.Id == long.Parse(log.NewValue))?.FirstName;
            }
        }
    
        if (catIds.Count > 0)
        {
            var acts = AccountRepo.FindAll(e => actIds.Contains(e.Id)).ResultObject.ToList();
            foreach (var log in Logs.Where(e => e.PropertyName == nameof(Transaction.TransactionAccountId)))
            {
                if (!string.IsNullOrEmpty(log.OldValue))
                    log.OldValue = acts.FirstOrDefault(e => e.Id == long.Parse(log.OldValue))?.Name;

                if (!string.IsNullOrEmpty(log.NewValue))
                    log.NewValue = acts.FirstOrDefault(e => e.Id == long.Parse(log.NewValue))?.Name;
            }
        }

    }

}
