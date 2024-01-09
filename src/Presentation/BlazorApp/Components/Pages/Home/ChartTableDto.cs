namespace XploringMe.BlazorApp.Components.Pages.Home;

public class ChartTableDto
{
    public string Title { get; set; } = default!;
    public List<PieChartDto> Table { get; set; } = new();

}
