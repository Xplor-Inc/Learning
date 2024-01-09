using Microsoft.JSInterop;

namespace XploringMe.BlazorApp.Components.Pages.Home;

public partial class ChartWithTable : ComponentBase
{
    [Parameter]
    public ChartTableDto ChartTable { get; set; } = new ();
    private IJSObjectReference? JsModule { get; set; }

    private readonly string ChartId = new(Enumerable.Range(0, 10).Select(n => (char)(new Random().Next(32, 127))).ToArray());

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && ChartTable.Table.Count > 0)
        {
            JsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./../charts/pie.js");

            await JsModule.InvokeVoidAsync("drowPieChart", ChartId);
        }
    }
}
