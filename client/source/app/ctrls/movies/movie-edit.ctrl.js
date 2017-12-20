(function() {
    'use strict';

    angular
        .module('hr')
        .controller('MovieEditCtrl', ['$http', '$stateParams', '$state', MovieEditCtrl]);

    function MovieEditCtrl($http, $stateParams, $state) {
        var self = this;

        console.log($stateParams.movieId)
        self.movieId = $stateParams.movieId

        $http({
            method: 'get',
            url: 'http://localhost:8080/api/Movie/' + self.movieId
        }).then(function(response) {
            console.log(response, 'res');
            self.movie = response.data;
        }, function(error) {
            console.log(error, 'can not get data.');
        });

        self.update = function(movie) {
            self.updatedMovie = {
                "id": movie.id,
                "rating": movie.rating,
                "releasedYear": movie.releasedYear,
                "sales": movie.sales,
                "title": movie.title
            }
            console.log(self.updatedMovie)
            $http({
                    method: 'PUT',
                    url: 'http://localhost:8080/api/Movie/' + self.movieId,
                    data: self.updatedMovie,
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                .then(function(response) {
                    console.log('succes');
                    $state.go('movies');
                }, function(error) {
                    console.log('can not put data.');
                });
        };

    };


}());