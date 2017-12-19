angular.module('hr', [
    'ui.router'
]);
(function () {
    'use strict';

    angular
        .module('hr')
        .config(['$stateProvider', '$urlRouterProvider', routes]);

    function routes ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('login', {
                url: '/movies',
                templateUrl: '/public/templates/movies/movies.tpl.html',
                controller: 'MovieCtrl',
                controllerAs: 'movie'
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
