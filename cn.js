import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";

const setTitle = (chart: am4charts.XYChart, title: string) => {
    var chartTitle = chart.titles.create();
    chartTitle.text = title;
    chartTitle.fontSize = 18;
    chartTitle.marginBottom = 30;
}

const setXTitle = (chart: am4charts.XYChart, title: string) => {
    let label = chart.chartContainer.createChild(am4core.Label);
    label.text = title;
    label.align = "center";
    label.fontSize = "0.75rem"
    //label.fontWeight = "bold"
}

const setYTitle = (chart: am4charts.XYChart, title: string) => {
    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.extraMax = .1;
    valueAxis.title.text = title;
    valueAxis.title.fontWeight = "bold";
}

const setLegend = (chart: am4charts.XYChart) => {
    var legend = new am4charts.Legend();
    legend.position = 'right';
    legend.scrollable = true;
    legend.valign = 'top';
    legend.reverseOrder = true;

    chart.legend = legend;
    return chart.legend;
}

const chartColors = () =>{
    return [
        am4core.color("red"),
        am4core.color("blue"),
        am4core.color("#FF6F91"),
        am4core.color("#FF9671"),
    ];
}
export const ChartConfig = {
    Title: setTitle,
    XTitle: setXTitle,
    YTitle: setYTitle,
    AddLegend: setLegend,
    ColorList : chartColors
}
