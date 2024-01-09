export const drawLineChart = (chartId) => {

    var root = am5.Root.new(chartId);

    var myTheme = am5.Theme.new(root);

    myTheme.rule("Label").setAll({
        fontSize: "10px",
           fill: am5.color('#FFFFFF')
 });

    root.setThemes([
        am5themes_Animated.new(root),
        myTheme
    ]);

    let chart = root.container.children.push(
        am5xy.XYChart.new(root, {
            layout: root.verticalLayout,
            pinchZoomX: true
        })
    );

    // The data
    let data = [
        {
            year: "1930",
            italy: 1,
            germany: 5,
            uk: 3
        },
        {
            year: "1934",
            italy: 1,
            germany: 2,
            uk: 6
        },
        {
            year: "1938",
            italy: 2,
            germany: 3,
            uk: 1
        },
        {
            year: "1950",
            italy: 3,
            germany: 4,
            uk: 1
        },
        {
            year: "1954",
            italy: 5,
            germany: 1,
            uk: 2
        },
        {
            year: "1958",
            italy: 3,
            germany: 2,
            uk: 1
        },
        {
            year: "1962",
            italy: 1,
            germany: 2,
            uk: 3
        },
        {
            year: "1966",
            italy: 2,
            germany: 1,
            uk: 5
        },
        {
            year: "1970",
            italy: 3,
            germany: 5,
            uk: 2
        },
        {
            year: "1974",
            italy: 4,
            germany: 3,
            uk: 6
        },
        {
            year: "1978",
            italy: 1,
            germany: 2,
            uk: 4
        }
    ];

    let xRenderer = am5xy.AxisRendererX.new(root, {});
    xRenderer.grid.template.set("location", 0.5);
    xRenderer.labels.template.setAll({
        location: 0.5,
        multiLocation: 0.5
    });

    let xAxis = chart.xAxes.push(
        am5xy.CategoryAxis.new(root, {
            categoryField: "year",
            renderer: xRenderer,
            tooltip: am5.Tooltip.new(root, {})
        })
    );

    xAxis.data.setAll(data);

    let yAxis = chart.yAxes.push(
        am5xy.ValueAxis.new(root, {
            maxPrecision: 0,
            renderer: am5xy.AxisRendererY.new(root, {
               // inversed: true
            })
        })
    );
    chart.set("colors", am5.ColorSet.new(root, {
        colors: [
            am5.color(0x73556E),
            am5.color(0x9FA1A6),
            am5.color(0xF2AA6B),
            am5.color(0xF28F6B),
            am5.color(0xA95A52),
            am5.color(0xE35B5D),
            am5.color(0xFFA446)
        ]
    }))

    // Add series
    // https://www.amcharts.com/docs/v5/charts/xy-chart/series/


    var legend = chart.children.push(am5.Legend.new(root, {
        centerX: am5.percent(50),
        x: am5.percent(50),
        //marginTop: 15,
        //marginBottom: 15,
        layout: am5.GridLayout.new(root, {
            maxColumns: 3,
            fixedWidthGrid: true
        })
    }));
    legend.markers.template.setAll({
        width: 15,
        height: 15
    });
    legend.markerRectangles.template.adapters.add("fillGradient", function () {
        return undefined;
    })
    legend.markerRectangles.template.setAll({
        cornerRadiusTL: 5,
        cornerRadiusTR: 5,
        cornerRadiusBL: 5,
        cornerRadiusBR: 5
    });
   
    function createSeries(name, field) {
        let series = chart.series.push(
            am5xy.LineSeries.new(root, {
                name: name,
                xAxis: xAxis,
                yAxis: yAxis,
                valueYField: field,
                categoryXField: "year",
                tooltip: am5.Tooltip.new(root, {
                    pointerOrientation: "horizontal",
                    labelText: "[bold]{name}[/]\n{categoryX}: {valueY}"
                })
            })
        );


        series.bullets.push(function () {
            return am5.Bullet.new(root, {
                sprite: am5.Circle.new(root, {
                    radius: 5,
                    fill: series.get("fill")
                })
            });
        });

        // create hover state for series and for mainContainer, so that when series is hovered,
        // the state would be passed down to the strokes which are in mainContainer.
        series.set("setStateOnChildren", true);
        series.states.create("hover", {});

        series.mainContainer.set("setStateOnChildren", true);
        series.mainContainer.states.create("hover", {});

        series.strokes.template.states.create("hover", {
            strokeWidth: 4
        });

        legend.data.setAll(chart.series.values);
        series.data.setAll(data);
        series.appear(1000);
    }

    createSeries("Italy", "italy");
    createSeries("Germany", "germany");
    createSeries("UK", "uk");

    chart.appear(1000, 100);

}