/**
 * ---------------------------------------
 * This demo was created using amCharts 5.
 *
 * For more information visit:
 * https://www.amcharts.com/
 *
 * Documentation is available at:
 * https://www.amcharts.com/docs/v5/
 * ---------------------------------------
 */

// Create root element
// https://www.amcharts.com/docs/v5/getting-started/#Root_element
var root = am5.Root.new("chartdiv");


// Set themes
// https://www.amcharts.com/docs/v5/concepts/themes/
root.setThemes([
  am5themes_Animated.new(root)
]);


// Create chart
// https://www.amcharts.com/docs/v5/charts/xy-chart/
var chart = root.container.children.push(am5xy.XYChart.new(root, {
  panX: false,
  panY: false,
  wheelX: "panX",
  wheelY: "zoomX",
  layout: root.verticalLayout
}));


// Add legend
// https://www.amcharts.com/docs/v5/charts/xy-chart/legend-xy-series/
var legend = chart.children.push(
  am5.Legend.new(root, {
    centerX: am5.p50,
    x: am5.p50
  })
);

var data = [{
  year: "2021",
  europe: 2.5,
  namerica: 2.5,
  asia: 1.2,
  none: 0
}, {
  year: "2022",
  europe: 2.6,
  namerica: 2.7,
  asia: 2,
  none: 0
}, {
  year: "2023",
  europe: 2.8,
  namerica: 2.9,
  asia: 1.9,
  none: 0
}]


// Create axes
// https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
  categoryField: "year",
  renderer: am5xy.AxisRendererX.new(root, {
    cellStartLocation: 0.1,
    cellEndLocation: 0.9
  }),
  tooltip: am5.Tooltip.new(root, {})
}));

xAxis.data.setAll(data);

var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
  calculateTotals: true,
  min: 0,
  extraMax: 0.1,
  renderer: am5xy.AxisRendererY.new(root, {})
}));


// Add series
// https://www.amcharts.com/docs/v5/charts/xy-chart/series/
function makeSeries(name, fieldName, showTotal) {
  var series = chart.series.push(am5xy.ColumnSeries.new(root, {
    name: name,
    xAxis: xAxis,
    yAxis: yAxis,
    valueYField: fieldName,
    categoryXField: "year",
    stacked: true,
    maskBullets: false
  }));

  series.columns.template.setAll({
    tooltipText: "{name}, {categoryX}:{valueY}",
    width: am5.percent(90),
    tooltipY: 0
  });

  if (showTotal) {
    series.bullets.push(function () {
      return am5.Bullet.new(root, {
        locationY: 1,
        sprite: am5.Label.new(root, {
          text: "{valueYTotal}",
          fill: am5.color(0x000000),
          centerY: am5.p100,
          centerX: am5.p50,
          populateText: true
        })
      });
    });
  }

  series.data.setAll(data);
  series.appear();
  
  if (!showTotal) {
    legend.data.push(series);
  }
}

makeSeries("Europe", "europe");
makeSeries("North America", "namerica");
makeSeries("Asia", "asia");
makeSeries("", "none", true);


// Make stuff animate on load
// https://www.amcharts.com/docs/v5/concepts/animations/
chart.appear(1000, 100);
