app.service("ChartService", function () {

    this.getChartAttr = function () {
        return {
            "caption": "Sitemap diagnostic",
            "plotgradientcolor": "",
            "bgcolor": "FFFFFF",
            "showalternatehgridcolor": "0",
            "divlinecolor": "CCCCCC",
            "showvalues": "0",
            "showcanvasborder": "0",
            "canvasborderalpha": "0",
            "canvasbordercolor": "CCCCCC",
            "canvasborderthickness": "1",
            "yaxismaxvalue": "5000",
            "captionpadding": "30",
            "linethickness": "3",
            "yaxisvaluespadding": "15",
            "legendshadow": "0",
            "legendborderalpha": "0",
            "showborder": "0"
        };
    }

    this.getChartCategories = function (data, categoryName) {
        var categories = [];
        for (var i = 0; i < data.length; i++)
            if (data[i])
                categories.push({ label: data[i][categoryName] });
        return categories;
    }

    this.getChartValues = function (data, valueName) {
        var values = [];
        for (var i = 0; i < data.length; i++)
            if (data[i])
                values.push({ value: data[i][valueName] });
        return values;
    }

});