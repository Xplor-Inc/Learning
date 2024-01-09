namespace XploringMe.BlazorApp.Components.Pages.Transactions;

public class SearchDto
{
    public DateTimeOffset?      FromDate                    { get; set; }
    public DateTimeOffset?      ToDate                      { get; set; }
    public DateTimeOffset?      PaymentDate                 { get; set; }
    public TransactionType?     TransactionType             { get; set; }
    public long?                DebitAccountId              { get; set; }
    public long?                CreditAccountId             { get; set; }
    public long?                CategoryId                  { get; set; }
    public string?              SearchText                  { get; set; }
    public DebtStatus?          DebtStatus                  { get; set; }
    public IReadOnlyList<long>? Tags                        { get; set; }
    public string               SortOrder                   { get; set; } = "DESC";
    public string               SortBy                      { get; set; } = "TransactionDate";
    public int                  Skip                        { get; set; }
    public int                  PageSize                    { get; set; } = 2;
    public int                  Count                       { get; set; }
    public decimal              Income                      { get; set; }
    public decimal              Expense                     { get; set; }    
}
