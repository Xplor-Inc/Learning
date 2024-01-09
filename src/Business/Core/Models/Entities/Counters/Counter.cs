namespace XploringMe.Core.Models.Entities.Counters;

public class Counter : Entity
{
    public string           Browser         { get; set; } = string.Empty;
    public new long?        CreatedById     { get; set; }
    public string           Device          { get; set; } = string.Empty;
    public string           IPAddress       { get; set; } = string.Empty;
    public DateTimeOffset?  LastVisit       { get; set; }
    public string           OperatingSystem { get; set; } = string.Empty;
    public string           Page            { get; set; } = string.Empty;
    public string           Search          { get; set; } = string.Empty;
    public string           UserAgent       { get; set; } = string.Empty;
    public string           VisitorId       { get; set; } = string.Empty;

    #region Analysis Data
    public int              ResumeDownloads { get; set; }
    public int              EnquirySent     { get; set; }

    #endregion
}

