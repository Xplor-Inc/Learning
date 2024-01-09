namespace XploringMe.BlazorApp.Models.Dtos.Counter;

public class CounterPostDto
{
    public string Page   { get; set; } = string.Empty;
    public string Search { get; set; } = string.Empty;
}

public class CounterDto : AuditableDto
{
    public string           Browser         { get; set; } = string.Empty;
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
