(function() {
    'use strict';

    angular
        .module('hr')
        .controller('ActorAddCtrl', ['$http', '$state', ActorAddCtrl]);

    function ActorAddCtrl($http, $state, ) {
        var self = this;

        self.upload = function(actor) {
            console.log(actor)
            $http({
                    method: 'POST',
                    url: 'http://localhost:8080/api/actor',
                    data: actor,
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                .then(function(response) {
                    console.log('succes');
                    $state.go('actors');
                }, function(error) {
                    console.log('can not put data.');
                });
        };

    };


}());