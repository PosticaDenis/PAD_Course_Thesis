(function() {
    'use strict';

    angular
        .module('hr')
        .controller('ActorCtrl', ['$http', ActorCtrl]);

    function ActorCtrl($http) {
        var self = this;
        $http({
            method: 'get',
            url: 'http://localhost:8080/api/actor'
        }).then(function(response) {
            console.log(response, 'res');
            self.actors = response.data;
        }, function(error) {
            console.log(error, 'can not get data.');
        });
    }

}());