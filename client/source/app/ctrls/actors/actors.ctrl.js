(function () {
    'use strict';

    angular
        .module('hr')
        .controller('ActorCtrl', [ActorCtrl]);

    function ActorCtrl () {
        var self = this;
        $http({
            method: 'get', 
            url: 'http://localhost:8080/api/actor'
        }).then(function (response) {
            console.log(response, 'res');
            self.movies = response.data;
        },function (error){
            console.log(error, 'can not get data.');
        });
    }

}());
