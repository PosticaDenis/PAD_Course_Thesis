(function () {
    'use strict';

    angular
        .module('hr')
        .controller('MovieCtrl', ['$http', MovieCtrl]);

    function MovieCtrl ($http) {
        var self = this;

        $http({
            method: 'get', 
            url: 'http://localhost:8080/api/movie'
        }).then(function (response) {
            console.log(response, 'res');
            self.movies = response.data;
        },function (error){
            console.log(error, 'can not get data.');
        });

    }

}());
