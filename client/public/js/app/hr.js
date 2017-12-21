angular.module('hr', [
    'ui.router'
]);
(function() {
    'use strict';

    angular
        .module('hr')
        .config(['$stateProvider', '$urlRouterProvider', routes]);

    function routes($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('movies', {
                url: '/movies',
                templateUrl: '/public/templates/movies/movies.tpl.html',
                controller: 'MovieCtrl',
                controllerAs: 'movie'
            })
            .state('edit', {
                url: '/movies/edit/:movieId',
                templateUrl: '/public/templates/movies/movies-edit.tpl.html',
                controller: 'MovieEditCtrl',
                controllerAs: 'm'
            })
            .state('upload', {
                url: '/movies/upload',
                templateUrl: '/public/templates/movies/movies-add.tpl.html',
                controller: 'MovieAddCtrl',
                controllerAs: 'adm'
            })
            .state('acupload', {
                url: '/actors/upload',
                templateUrl: '/public/templates/actors/actors-add.tpl.html',
                controller: 'ActorAddCtrl',
                controllerAs: 'adc'
            })
            .state('acedit', {
                url: '/actors/edit/:actorId',
                templateUrl: '/public/templates/actors/actors-edit.tpl.html',
                controller: 'ActorEditCtrl',
                controllerAs: 'a'
            })
            .state('actors', {
                url: '/actors',
                templateUrl: '/public/templates/actors/actors.tpl.html',
                controller: 'ActorCtrl',
                controllerAs: 'actor'
            });

        $urlRouterProvider.otherwise('/movies');
    }

}());


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
(function() {
    'use strict';

    angular
        .module('hr')
        .controller('MovieCtrl', ['$http', MovieCtrl]);

    function MovieCtrl($http) {
        var self = this;

        $http({
            method: 'get',
            url: 'http://localhost:8080/api/movie'
        }).then(function(response) {
            console.log(response, 'res');
            self.movies = response.data;
        }, function(error) {
            console.log(error, 'can not get data.');
        });

    };


}());