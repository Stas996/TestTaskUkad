function SiteMapController($scope, $filter, $location, SiteMapService, ChartService)
{
    $scope.diagnostic = [];
    $scope.sitemap;
    $scope.pageSize = 40;
    $scope.currentPage = 1;
    $scope.isDiagnose = false;

    var siteUrl = $location.search().siteUrl;

    $scope.get = function (siteUrl) {
        SiteMapService.get(siteUrl)
        .then(function (d) {
            $scope.sitemap = d.data;
            $scope.sitemap.map = $filter('orderBy')(d.data.map, "averageRequestTime", true);
            $scope.drawChart();
        }, function (error) {
            console.log("Get error " + error.status + " " + error.statusText);
        });
    };

    $scope.updateDiagnostic = function (siteUrl) {
        SiteMapService.update(siteUrl, $scope.diagnostic)
        .then(function (d) {
            $scope.get(siteUrl);
        }, function (error) {
            console.log("Update error " + error.status + " " + error.statusText);
        });
    };

    $scope.toDiagnose = function () {
        var h = 0;
        $scope.isDiagnose = true;
        for(var i = 0; i < $scope.sitemap.map.length; i++)
        {
            var currItem = $scope.sitemap.map[i];
            //if (currItem.requestsTimeLog.length !== $scope.sitemap.diagnosticOrder)
                //continue;

            setTimeout(function (i) {
                var currUrl = $scope.sitemap.map[i].url;
                $scope.process = i + 1;
                $scope.processUrl = currUrl;
                SiteMapService.getTime(currUrl)
                .then(function (d) {
                    $scope.sitemap.map[i].requestsTimeLog.push(d.data);
                }, function (error) {
                    console.log("Diagnostic error " + error.status + " " + error.statusText);
                });
            }, 500 * (h + 1), i);
            h++;
        }
        setTimeout($scope.stopDiagnose, 500 * (h + 4));
    }

    $scope.stopDiagnose = function () {
        
        SiteMapService.update(siteUrl, $scope.sitemap)
        .then(function (d) {
            location.reload();
        }, function (error) {
            console.log("Update error " + error.status + " " + error.statusText);
        });
    }


    $scope.chartAttrs = {};
    $scope.chartCategories = [];
    $scope.chartValues = [];
 
    $scope.drawChart = function () {
        data = $scope.sitemap.map;

        var start = ($scope.currentPage - 1) * $scope.pageSize;
        var end = $scope.currentPage * $scope.pageSize;

        $scope.chartAttrs = ChartService.getChartAttr();
        $scope.chartCategories = [{
            "category": ChartService.getChartCategories(data.slice(start, end), "url")
        }];

        $scope.chartValues = [{
            "seriesname": "MaxTime",
            "data": ChartService.getChartValues(data.slice(start, end), "maxRequestTime")
        }, {
            "seriesname": "MinTime",
            "data": ChartService.getChartValues(data.slice(start, end), "minRequestTime")
        }];
    }

    $scope.get(siteUrl);
}

app.controller('SiteMapController', SiteMapController);