(function() {
    'use strict';

    angular
        .module('hr')
        .controller('MovieAddCtrl', ['$http', '$state', MovieAddCtrl]);

    function MovieAddCtrl($http, $state, ) {
        var self = this;

        self.upload = function(movie) {
            // self.updatedMovie = {
            //     "id": movie.id,
            //     "rating": movie.rating,
            //     "releasedYear": movie.releasedYear,
            //     "sales": movie.sales,
            //     "title": movie.title
            // }
            console.log(movie)
            $http({
                    method: 'POST',
                    url: 'http://localhost:8080/api/movie',
                    data: movie,
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