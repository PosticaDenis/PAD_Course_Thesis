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