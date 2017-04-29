function SiteMapController($scope, SiteMapService)
{
    $scope.links = [];

    $scope.getAll = function () {
        SiteMapService.getAll()
        .then(function (d) {
            $scope.links = d.data;
        }, function (error) {
            console.log(error.status + " " + error.statusText);
        });
    };

    $scope.get = function (id) {
        SiteMapService.get(id)
        .then(function (d) {
            $scope.sitemap = d.data;
        }, function (error) {
            console.log(error.status + " " + error.statusText);
        });
    };

    $scope.post = function (id) {
        SiteMapService.post(id)
        .then(function (d) {
            $scope.sitemap = d.data;
        }, function (error) {
            console.log(error.status + " " + error.statusText);
        });
    };

    $scope.delete = function (id) {
        SiteMapService.delete(id)
        .then(function (d) {
            $scope.getAll();
        }, function (error) {
            console.log(error.status + " " + error.statusText);
        })
    };

    //$scope.getAll();

}

app.controller('SiteMapController', SiteMapController);