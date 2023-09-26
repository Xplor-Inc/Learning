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
root.setThemes([am5themes_Animated.new(root)]);

// Create chart
// https://www.amcharts.com/docs/v5/charts/xy-chart/
var chart = root.container.children.push(
  am5xy.XYChart.new(root, {
    panX: false,
    panY: false,
    wheelX: "panX",
    wheelY: "zoomX",
    layout: root.verticalLayout
  })
);

// Add scrollbar
// https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
chart.set(
  "scrollbarX",
  am5.Scrollbar.new(root, {
    orientation: "horizontal"
  })
);

var data = [
  {
    year: "2016",
    income: 23.5,
    expenses: 23.1
  },
  {
    year: "2017",
    income: 26.2,
    expenses: 26.5
  },
  {
    year: "2018",
    income: 30.1,
    expenses: 34.9
  },
  {
    year: "2019",
    income: 29.5,
    expenses: 29.5
  },
  {
    year: "2020",
    income: 29,
    expenses: 25
  }
];

chart.get("colors").set("step", 5);

// Create axes
// https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
var xRenderer = am5xy.AxisRendererX.new(root, {});
var xAxis = chart.xAxes.push(
  am5xy.CategoryAxis.new(root, {
    categoryField: "year",
    renderer: xRenderer,
    tooltip: am5.Tooltip.new(root, {})
  })
);
xRenderer.grid.template.setAll({
  location: 1
})

xAxis.data.setAll(data);

var yAxis = chart.yAxes.push(
  am5xy.ValueAxis.new(root, {
    min: 0,
    extraMax: 0.1,
    renderer: am5xy.AxisRendererY.new(root, {
      strokeOpacity: 0.1
    })
  })
);

var labelTemplate = am5.Template.new({});
labelTemplate.events.on("positionchanged", function() {
  // first arrange using centerY
  var i = 0;
  am5.array.each(series1.dataItems, function(s1DataItem) {
    var s2DataItem = series2.dataItems[i];

    if (s2DataItem) {
      var s1Bullets = s1DataItem.bullets;
      if (s1Bullets) {
        var s1Bullet = s1Bullets[0];
      }
      var s2Bullets = s2DataItem.bullets;
      if (s2Bullets) {
        var s2Bullet = s2Bullets[1];
      }
      if (s1Bullet && s2Bullet) {
        var s1Sprite = s1Bullet.get("sprite");
        var s2Sprite = s2Bullet.get("sprite");

        if (am5.math.boundsOverlap(s1Sprite.bounds(), s2Sprite.bounds())) {

          var s1Value = s1DataItem.get("valueYWorking");
          var s2Value = s2DataItem.get("valueYWorking");

          if (s1Value > s2Value) {
            s1Sprite.set("centerY", am5.p100);
            s2Sprite.set("centerY", 0);
          }
          else {
            s1Sprite.set("centerY", 0);
            s2Sprite.set("centerY", am5.p100);
          }
        }
      }
    }

    i++;
  })
})


// Add series
// https://www.amcharts.com/docs/v5/charts/xy-chart/series/

var series1 = chart.series.push(
  am5xy.ColumnSeries.new(root, {
    name: "Income",
    xAxis: xAxis,
    yAxis: yAxis,
    valueYField: "income",
    categoryXField: "year",
    tooltip: am5.Tooltip.new(root, {
      pointerOrientation: "horizontal",
      labelText: "{name} in {categoryX}: {valueY} {info}"
    })
  })
);

series1.columns.template.setAll({
  tooltipY: am5.percent(10),
  fillOpacity: 0.5,
  strokeOpacity: 0
});

series1.bullets.push(function() {
  return am5.Bullet.new(root, {
    locationX: 0.5,
    locationY: 1,
    sprite: am5.Label.new(root, {
      centerY: am5.p100,
      centerX: am5.p50,
      text: "{valueY}",
      fill: series1.get("fill"),
      populateText: true
    }, labelTemplate)
  });
});

series1.data.setAll(data);

var series2 = chart.series.push(
  am5xy.LineSeries.new(root, {
    name: "Expenses",
    xAxis: xAxis,
    yAxis: yAxis,
    valueYField: "expenses",
    categoryXField: "year",
    tooltip: am5.Tooltip.new(root, {
      pointerOrientation: "horizontal",
      labelText: "{name} in {categoryX}: {valueY} {info}"
    })
  })
);


series2.data.setAll(data);

series2.bullets.push(function() {
  return am5.Bullet.new(root, {
    sprite: am5.Circle.new(root, {
      strokeWidth: 3,
      stroke: series2.get("stroke"),
      radius: 5,
      fill: root.interfaceColors.get("background")
    })
  });
});

series2.bullets.push(function() {
  return am5.Bullet.new(root, {
    locationX: 0.5,
    locationY: 1,
    sprite: am5.Label.new(root, {
      centerY: am5.p100,
      centerX: am5.p50,
      text: "{valueY}",
      fill: series2.get("fill"),
      populateText: true
    }, labelTemplate)
  });
});


chart.set("cursor", am5xy.XYCursor.new(root, {}));

// Add legend
// https://www.amcharts.com/docs/v5/charts/xy-chart/legend-xy-series/
var legend = chart.children.push(
  am5.Legend.new(root, {
    centerX: am5.p50,
    x: am5.p50
  })
);
legend.data.setAll(chart.series.values);

// Make stuff animate on load
// https://www.amcharts.com/docs/v5/concepts/animations/
chart.appear(1000, 100);
series1.appear();
