namespace XploringMe.Core.Models.Investments.MFCrawling;

public class MutualFundModel
{
    public string SchemeCode { get; set; } = default!;
    public string SchemeName { get; set; } = default!;
}


public class PriceList
{
    public string Date { get; set; } = string.Empty;
    public double Nav { get; set; } = default!;
}

public class FundDetails
{
    public string FundHouse { get; set; } = default!;
    public string SchemeType { get; set; } = default!;
    public string SchemeCategory { get; set; } = default!;
    public int SchemeCode { get; set; }
    public string SchemeName { get; set; } = default!;
}

public class MFNavModel
{
    public FundDetails Meta { get; set; } = default!;
    public List<PriceList> Data { get; set; } = default!;
    public string Status { get; set; } = default!;
}
