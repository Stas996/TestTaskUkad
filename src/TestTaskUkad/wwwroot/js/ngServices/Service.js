app.service("SiteMapService", function ($http) {

    this.URL = "api/Sitemap/";

    this.getAll = function () {
        return $http.get(this.URL);
    }

    this.get = function (id) {
        return $http(
        {
            method: 'get',
            data: id,
            url: this.URL + "?url=" + id,
        });
    }

    this.post = function (id) {
        return $http(
        {
            method: 'post',
            data: id,
            url: this.URL + "?url=" + id
        });
    }

    this.delete = function (id) {
        return $http(
        {
            method: 'delete',
            data: id,
            url: this.URL + "?url=" + id
        });
    }

});