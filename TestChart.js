import React, { Component } from 'react';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Service } from '../Core/Utility';
import { ChartConfig } from './Configurations';

class ColumnBar extends Component<any, any> {
    height = '400px';
    constructor(props:any) {
        super(props);
        console.log('column bar')
        this.state = {
            hasError: false
        }
    }

    async componentDidMount() {
        var path = `/assets/data/chart/${this.props.path}.json`
        let response = await Service.Get(path);
        console.log(response);
        if (response.hasErrors) {
            this.setState({
                hasError: true
            })
        }
        else {
            this.createColumnChart(response.resultObject);
        }
    }
    createColumnChart = (response:any) => {
        am4core.useTheme(am4themes_animated);

        let chart = am4core.create(this.props.element, am4charts.XYChart);

        chart.data = response.data;

        // Create axes
        let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "Region";
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.renderer.minGridDistance = 30;
        categoryAxis.renderer.labels.template.horizontalCenter = "right";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.renderer.labels.template.rotation = 0;
       // categoryAxis.tooltip.disabled = true;
        categoryAxis.renderer.minHeight = 10;

       // SetTitles(chart, response.name, response.xTitle, response.yTitle, this.props.showLegend);
       ChartConfig.Title(chart, response.name);
       ChartConfig.XTitle(chart, response.xTitle);
       ChartConfig.YTitle(chart, response.yTitle);
       if (this.props.showLegend)
           ChartConfig.AddLegend(chart);
        // Create series
        let series = chart.series.push(new am4charts.ColumnSeries());
        series.sequencedInterpolation = true;
        series.dataFields.valueY = "Sales";
        series.dataFields.categoryX = "Region";
        series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
        series.columns.template.strokeWidth = 0;

        if(series.tooltip)
        series.tooltip.pointerOrientation = "vertical";

        series.columns.template.column.cornerRadiusTopLeft = 10;
        series.columns.template.column.cornerRadiusTopRight = 10;
        series.columns.template.column.fillOpacity = 0.8;

        var labelBullet = series.bullets.push(new am4charts.LabelBullet())
        labelBullet.interactionsEnabled = false
        labelBullet.dy = -10;
        labelBullet.label.text = '{valueY}'
        labelBullet.label.fill = am4core.color('#041C49')

        // on hover, make corner radiuses bigger
        let hoverState = series.columns.template.column.states.create("hover");
        hoverState.properties.cornerRadiusTopLeft = 0;
        hoverState.properties.cornerRadiusTopRight = 0;
        hoverState.properties.fillOpacity = 1;

        series.columns.template.adapter.add("fill", function (fill:any, target:any) {
            return chart.colors.getIndex(target.dataItem.index);
        });

        // Cursor
        chart.cursor = new am4charts.XYCursor();
    }
    render() {
        return (
            <div id={this.props.element} style={{ minHeight: this.height, width: '100%' }}>
                {this.state.hasError ? <div className='text-danger' style={{ textAlign: 'center', paddingTop: '190px' }}>
                    Data not available</div> : ''}
            </div>
        );
    }
}

export default ColumnBar;

**************************************************************************************************************************************************************
  import React, { Component } from 'react';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Service } from '../Core/Utility';
import { ChartConfig } from './Configurations';

class DefectAgeBar extends Component<any, any> {
    height = '400px';
    constructor(props:any) {
        super(props);
        this.state = {
            hasError: false
        }
    }

    async componentDidMount() {
        var path = `/assets/data/chart/${this.props.path}.json`
        let response = await Service.Get(path);
        if (response.hasErrors) {
            this.setState({
                hasError: true
            })
        }
        else {
            this.createColumnChart(response.resultObject);
        }
    }
    createColumnChart = (response:any) => {
        am4core.useTheme(am4themes_animated);
        // Themes end
        var chart = am4core.create(this.props.element, am4charts.XYChart)
        chart.colors.step = 2;
        chart.legend = new am4charts.Legend()
        chart.legend.position = 'top'
        chart.legend.paddingBottom = 20
        chart.legend.labels.template.maxWidth = 95

        var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
        xAxis.dataFields.category = 'Category'
        xAxis.renderer.cellStartLocation = 0.1
        xAxis.renderer.cellEndLocation = 0.9
        xAxis.renderer.grid.template.location = 0;

        var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
        yAxis.min = 0;

        function createSeries(value:any, name:any) {
            var series = chart.series.push(new am4charts.ColumnSeries())
            series.dataFields.valueY = value
            series.dataFields.categoryX = 'Category'
            series.name = name

            series.events.on("hidden", arrangeColumns);
            series.events.on("shown", arrangeColumns);
            
            var bullet1 = series.bullets.push(new am4charts.LabelBullet())
            bullet1.interactionsEnabled = false
            bullet1.dy = -10;
            bullet1.label.text = '{valueY}'
            bullet1.label.fill = am4core.color('#041C49')
            return series;
        }
        
        ChartConfig.Title(chart, response.name);
        ChartConfig.XTitle(chart, response.xTitle);
        ChartConfig.YTitle(chart, response.yTitle);
        if (this.props.showLegend)
            ChartConfig.AddLegend(chart);
        chart.data = response.data;
        
        createSeries('dMin', 'Min');
        createSeries('dMax', 'Max');
        createSeries('AvgAge', 'Avg Defect Age');

        function arrangeColumns() {


            var series = chart.series.getIndex(0);

            var w = 1 - xAxis.renderer.cellStartLocation - (1 - xAxis.renderer.cellEndLocation);
            if (series && series.dataItems.length > 1) {
                var x0 = xAxis.getX(series.dataItems.getIndex(0) as am4charts.XYSeriesDataItem, "categoryX");
                var x1 = xAxis.getX(series.dataItems.getIndex(1) as am4charts.XYSeriesDataItem, "categoryX");
                var delta = ((x1 - x0) / chart.series.length) * w;
                if (am4core.isNumber(delta)) {
                    var middle = chart.series.length / 2;

                    var newIndex = 0;
                    chart.series.each(function (series) {
                        if (!series.isHidden && !series.isHiding) {
                            series.dummyData = newIndex;
                            newIndex++;
                        }
                        else {
                            series.dummyData = chart.series.indexOf(series);
                        }
                    })
                    var visibleCount = newIndex;
                    var newMiddle = visibleCount / 2;

                    chart.series.each(function (series) {
                        var trueIndex = chart.series.indexOf(series);
                        var newIndex = series.dummyData;

                        var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                        series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                        series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                    })
                }
            }
        }
    }
    render() {
        return (
            <div id={this.props.element} style={{ minHeight: this.height, width: '100%' }}>
                {this.state.hasError ? <div className='text-danger' style={{ textAlign: 'center', paddingTop: '190px' }}>
                    Data not available</div> : ''}
            </div>
        );
    }
}

export default DefectAgeBar;

*****************************************************************************************************************************************************
  import React, { Component } from 'react';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated"
import { Service } from '../Core/Utility';
import { ChartConfig } from './Configurations';

interface legendData {
    name: string,
    fill: am4core.Color
}
class MaxStoryAge extends Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            height: '400px'
        }
    }

    async componentDidMount() {

        var oneBarHeight = 30;
        var path = `/assets/data/chart/${this.props.path}.json`
        let response = await Service.Get(path);
        if (response.hasErrors) {
            this.setState({
                hasError: true
            })
        }
        else {

            var height = (oneBarHeight * response.resultObject.data.length);

            if (height > 400) {
                this.setState({
                    height: `${height}px`
                })
            }
            var chart = am4core.create(this.props.element, am4charts.XYChart);

            chart.data = response.resultObject.data;
            // Create axes
            var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            yAxis.dataFields.category = "state";
            yAxis.renderer.grid.template.location = 0;
            yAxis.renderer.labels.template.fontSize = 12;
            yAxis.renderer.minGridDistance = 10;

            // var legend = {} as am4charts.Legend;
            ChartConfig.Title(chart, response.resultObject.name);
            ChartConfig.XTitle(chart, response.resultObject.xTitle);
            ChartConfig.YTitle(chart, response.resultObject.yTitle);
            var legend = new am4charts.Legend();
            if (this.props.showLegend) {
                legend = ChartConfig.AddLegend(chart)
            }
            chart.xAxes.push(new am4charts.ValueAxis());

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueX = "sales";
            series.dataFields.categoryY = "state";
            series.columns.template.tooltipText = "{categoryY}: [bold]{valueX}[/]";
            series.columns.template.strokeWidth = 0;

            var LabelBullet = series.bullets.push(new am4charts.LabelBullet())
            LabelBullet.interactionsEnabled = false
            LabelBullet.dx = 5;
            LabelBullet.label.truncate = false;
            LabelBullet.label.text = '{valueX}'
            LabelBullet.label.fill = am4core.color('#041C49')
            LabelBullet.label.horizontalCenter = "left";

            series.columns.template.adapter.add("fill", function (fill: any, target: any) {
                if (target.dataItem) {
                    switch (target.dataItem.dataContext.region) {
                        case "CIB":
                            return chart.colors.getIndex(0);
                        case "PB":
                            return chart.colors.getIndex(1);
                        case "Core":
                            return chart.colors.getIndex(2);
                        case "EF":
                            return chart.colors.getIndex(3);
                        default:
                            return chart.colors.getIndex(3);
                    }
                }
                return fill;
            });

            var axisBreaks = [] as am4charts.CategoryAxisBreak[];
            var legends = [] as legendData[];

            // Add ranges
            const addRange = (label: string, start: string, end: string, color: am4core.Color) => {
                var range = yAxis.axisRanges.create();
                range.category = start;
                range.endCategory = end;
                range.label.text = label;
                range.label.disabled = true;
                range.label.fill = color;
                range.label.location = 0;
                range.label.dx = -130;
                range.label.dy = 12;
                range.label.fontWeight = "bold";
                range.label.fontSize = 12;
                range.label.horizontalCenter = "left"
                range.label.inside = true;

                range.grid.stroke = am4core.color("#396478");
                range.grid.strokeOpacity = 1;
                range.tick.length = 200;
                range.tick.disabled = false;
                range.tick.strokeOpacity = 0.6;
                range.tick.stroke = am4core.color("#396478");
                range.tick.location = 0;

                range.locations.category = 1;
                var axisBreak = yAxis.axisBreaks.create();
                axisBreak.startCategory = start;
                axisBreak.endCategory = end;
                axisBreak.breakSize = 1;
                axisBreak.fillShape.disabled = true;
                axisBreak.startLine.disabled = true;
                axisBreak.endLine.disabled = true;
                axisBreaks.push(axisBreak);

                legends.push({ name: label, fill: color });
            }

            addRange("CIB", "CNT - Trade Finance", "FABePAY Enhancements", chart.colors.getIndex(0));
            addRange("Core", "ESIGN PHASE2", "ESIGN PHASE2", chart.colors.getIndex(2));
            //addRange("EF", "EF-Requirement Gap", "EF-Data Issue", chart.colors.getIndex(3));
            addRange("PB", "Acquisition Squad CASA & PL", "FAB_One_Squad2", chart.colors.getIndex(1));

            chart.cursor = new am4charts.XYCursor();
            if (legend) {
                legend.data = legends;

                legend.itemContainers.template.events.on("toggled", function (event: any) {
                    var name = event.target.dataItem.dataContext.name;
                    var axisBreak = axisBreaks[name];
                    if (event.target.isActive) {
                        axisBreak.animate({ property: "breakSize", to: 0 }, 1000, am4core.ease.cubicOut);
                        yAxis.dataItems.each(function (dataItem: any) {
                            if (dataItem.dataContext.region === name) {
                                dataItem.hide(1000, 500);
                            }
                        })
                        series.dataItems.each(function (dataItem: any) {
                            if (dataItem.dataContext.region === name) {
                                dataItem.hide(1000, 0, 0, ["valueX"]);
                            }
                        })
                    }
                    else {
                        axisBreak.animate({ property: "breakSize", to: 1 }, 1000, am4core.ease.cubicOut);
                        yAxis.dataItems.each(function (dataItem: any) {
                            if (dataItem.dataContext.region === name) {
                                dataItem.show(1000);
                            }
                        })

                        series.dataItems.each(function (dataItem: any) {
                            if (dataItem.dataContext.region === name) {
                                dataItem.show(1000, 0, ["valueX"]);
                            }
                        })
                    }
                })
            }
            am4core.useTheme(am4themes_animated);
        }
    }
    render() {

        return (

            React.createElement('div', { id: this.props.element, style: { minHeight: this.state.height, width: '100%' } })

        );
    }
}

export default MaxStoryAge;
*********************************************************************************************************************************************
  
  import { Component } from 'react';
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Service } from '../Core/Utility';
import { ChartConfig } from './Configurations';


class SimpleLine extends Component<any, any> {
    height = '400px';
    constructor(props: any) {
        super(props);
        this.state = {
            hasError: false
        }
    }

    async componentDidMount() {
        var path = `/assets/data/chart/${this.props.path}.json`
        let response = await Service.Get(path);
        if (!response.hasErrors) {
            this.drawLine(response.resultObject);
        }
        else {
            this.setState({
                hasError: true
            })
        }
    }
    drawLine = (response: any) => {
        am4core.useTheme(am4themes_animated);

        var chart = am4core.create(this.props.element, am4charts.XYChart);

        chart.colors.list = [
            am4core.color("red"),
            am4core.color("blue"),
            am4core.color("#FF6F91"),
            am4core.color("#FF9671"),
        ];
        var data = response.data;
        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        dateAxis.renderer.grid.template.location = 0;
        dateAxis.renderer.minGridDistance = 20;
        dateAxis.renderer.grid.template.disabled = true;
        dateAxis.renderer.fullWidthTooltip = true;

        chart.dateFormatter.dateFormat = "MMM";

        ChartConfig.Title(chart, response.name);
        ChartConfig.XTitle(chart, response.xTitle);
        ChartConfig.YTitle(chart, response.yTitle);
        if (this.props.showLegend)
            ChartConfig.AddLegend(chart);

        chart.yAxes.push(new am4charts.ValueAxis());
        this.createLineSeries(chart, 1, "CIB", data.CIB);
        this.createLineSeries(chart, 2, "PB", data.PB);
        this.createLineSeries(chart, 3, "Core", data.Core);
        this.createLineSeries(chart, 4, "EF", data.EF);

        var durationSeries = chart.series.push(new am4charts.LineSeries());
        durationSeries.dataFields.valueY = "value";
        durationSeries.dataFields.dateX = "date";
        durationSeries.name = "Months";
        durationSeries.strokeWidth = 2;
        durationSeries.propertyFields.strokeDasharray = "dashLength";
        durationSeries.tooltipText = "{valueY.formatDuration()}";
        durationSeries.showOnInit = true;

        chart.legend.markers.template.states.create("dimmed").properties.opacity = 0.3;
        chart.legend.labels.template.states.create("dimmed").properties.opacity = 0.3;

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.fullWidthLineX = true;
        chart.cursor.xAxis = dateAxis;
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineX.fill = am4core.color("#000");
        chart.cursor.lineX.fillOpacity = 0.1;
    }
    createLineSeries = (chart: am4charts.XYChart, s: number, name: string, dataObject: any) => {
        var series = chart.series.push(new am4charts.LineSeries());
        series.dataFields.valueY = "value" + s;
        series.dataFields.dateX = "date";
        series.name = name;

        var data = [];
        for (var i = 0; i < dataObject.length; i++) {
            var dataItem = {
                date: new Date(2021, parseInt(dataObject[i].month), 1).getTime(),
                ["value" + s]: dataObject[i].count
            };
            data.push(dataItem);

        }
        data.sort((a, b) => { return a.date - b.date });
        series.data = data;


        // Draw bullets on line...
        let bullet = series.bullets.push(new am4charts.Bullet());
        let square = bullet.createChild(am4core.Rectangle);
        square.width = 10;
        square.height = 10;
        square.horizontalCenter = "middle";
        square.verticalCenter = "middle";
        return series;
    }
    render() {
        return (
            <div id={this.props.element} style={{ minHeight: this.height, width: '100%' }}>
                {this.state.hasError ? <div className='text-danger' style={{ textAlign: 'center', paddingTop: '190px' }}>Data not available</div> : ''}
            </div>
        );
    }
}

export default SimpleLine;
