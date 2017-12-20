(function() {
    'use strict';

    angular
        .module('hr')
        .controller('ActorEditCtrl', ['$http', '$stateParams', '$state', ActorEditCtrl]);

    function ActorEditCtrl($http, $stateParams, $state) {
        var self = this;

        console.log($stateParams.actorId)
        self.actorId = $stateParams.actorId

        $http({
            method: 'get',
            url: 'http://localhost:8080/api/Actor/' + self.actorId
        }).then(function(response) {
            console.log(response, 'res');
            self.actor = response.data;
        }, function(error) {
            console.log(error, 'can not get data.');
        });

        self.update = function(actor) {
            self.updatedActor = {
                // "id": actor.id,
                "firstName": actor.firstName,
                "lastName": actor.lastName,
            }
            console.log(self.updatedActor)
            $http({
                    method: 'PUT',
                    url: 'http://localhost:8080/api/Actor/' + self.actorId,
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