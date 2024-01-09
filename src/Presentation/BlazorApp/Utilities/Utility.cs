namespace XploringMe.BlazorApp.Utilities;

public static class Utility
{
    public static string GetTransactionColor(this TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Transfer => "#ECDA29", // Light Yellow
            TransactionType.Income => "#008000", //Green
            TransactionType.Expense => "#FF0000", //Red
            _ => "gray",
        };
    }
    public static string GetAccountColor(this AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Loan => "#ECDA29", // Light Yellow
            AccountType.Saving => "#008000", // Green
            AccountType.CreditCard => "#FF0000", // Red
            AccountType.Cash => "#0000FF", // Blue
            _ => "gray",
        };
    }

    public static string HexToRGBA(this string hex, decimal alpha = 0.2M)
    {
        if(hex.Contains('#')) { hex = hex.Replace("#", ""); }
        if(hex.Length != 6) return hex;
        var x = Convert.FromHexString(hex);
        return $"rgba({x[0]}, {x[1]}, {x[2]}, {alpha})";
    }
}
