var app;
(function () {
    app = angular.module("SiteMapModule", []);
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false,
            rewriteLinks: false
        });
    }]);
    app.filter('startFrom', function(){
        return function(data, start){
            return data.slice(start);
        }
    })
})();