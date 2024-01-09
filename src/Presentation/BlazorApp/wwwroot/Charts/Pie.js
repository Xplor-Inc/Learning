export const drowPieChart = (data, chartId) => {
    var root = am5.Root.new(chartId);

    var myTheme = am5.Theme.new(root);
    root._logo?.dispose();
    myTheme.rule("Label").setAll({
        fontSize: "10px",
            fill: am5.color('#FFFFFF')
   });

    root.setThemes([
        am5themes_Animated.new(root),
        myTheme
    ]);

    var chart = root.container.children.push(
        am5percent.PieChart.new(root, {
            radius: am5.percent(100),
            innerRadius: am5.percent(70),
            layout: root.verticalLayout
        })
    );

    var series = chart.series.push(
        am5percent.PieSeries.new(root, {
            valueField: "value",
            categoryField: "category",
            endAngle: 270
        })
    );

    series.set("colors", am5.ColorSet.new(root, {
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

    var gradient = am5.RadialGradient.new(root, {
        stops: [
            { color: am5.color(0x000000) },
            { color: am5.color(0x000000) },
            {}
        ]
    })

    series.slices.template.setAll({
        fillGradient: gradient,
        strokeWidth: 0,
        stroke: am5.color(0xffffff),
        cornerRadius: 5,
        shadowOpacity: 0.1,
        shadowOffsetX: 2,
        shadowOffsetY: 2,
        shadowColor: am5.color(0x000000),
        fillPattern: am5.GrainPattern.new(root, {
            maxOpacity: 0.2,
            density: 0.5,
            colors: [am5.color(0x000000)]
        })
    })

    series.slices.template.states.create("hover", {
        shadowOpacity: 1,
        shadowBlur: 5
    })

    series.data.setAll(data);

    // Disabling labels and ticks
     series.labels.template.set("visible", false);
     series.ticks.template.set("visible", false);

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
    legend.data.setAll(series.dataItems);

    var label = series.children.push(am5.Label.new(root, {
        text: "${valueSum.formatNumber('#,###.')}",
        fill: am5.color('#FFFFFF'),
        centerX: am5.percent(50),
        centerY: am5.percent(50),
        populateText: true,
        oversizedBehavior: "fit"
    }));

    series.onPrivate("width", function (width) {
        label.set("maxWidth", width * 0.7);
    });
    series.appear(1000, 100);
}