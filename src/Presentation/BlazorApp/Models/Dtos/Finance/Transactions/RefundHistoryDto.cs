namespace XploringMe.BlazorApp.Models.Dtos.Finance.Transactions;

public class RefundHistoryDto : AuditableDto
{
    public decimal          Total           { get; set; }
    public decimal          Balance         { get; set; }
    public DateTimeOffset   Date            { get; set; }
    public decimal          Refund          { get; set; }
    public string           Remark          { get; set; } = string.Empty;
    
    public TransactionAccountDto? RefundAccount { get; set; }
}