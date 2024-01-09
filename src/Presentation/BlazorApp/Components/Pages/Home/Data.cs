namespace XploringMe.BlazorApp.Components.Pages.Home;

public class Data
{
    public string Title { get; set; } = string.Empty;
    public double Value { get; set; }
    public double Change { get; set; }
    public string SubText { get; set; } = string.Empty;
    public bool Success { get; set; }
    public ChangeType Type { get; set; }
}
