using XploringMe.Core.Enumerations.Finance;

namespace XploringMe.Core.Models.Entities.Finance.SummaryReport;

public class DashboardBox : Auditable
{
    public DateTimeOffset       Date        { get; set; }
    public string               Title       { get; set; } = string.Empty;
    public double               Value       { get; set; }
    public double               Change      { get; set; }
    public string               CompareBy   { get; set; } = string.Empty;
    public bool                 Success     { get; set; }
    public RecurringFrequency   Frequency   { get; set; }
    public ChangeType           Type        { get; set; }
}
