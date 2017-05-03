app.service("SiteMapService", function ($http) {

    this.URL = "/api/Sitemap/";

    this.get = function (siteUrl) {
        return $http(
        {
            method: 'get',
            data: siteUrl,
            url: this.URL + "?siteUrl=" + siteUrl
        });
    }

    this.getTime = function (siteUrl) {
        return $http(
        {
            method: 'get',
            data: siteUrl,
            url: this.URL + "Time/" + "?siteUrl=" + siteUrl
        });
    }

    this.update = function (siteUrl, data) {
        return $http(
        {
            method: 'put',
            data: data,
            url: this.URL + "?siteUrl=" + siteUrl
        });
    }

});