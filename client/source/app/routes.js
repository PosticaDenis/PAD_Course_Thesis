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
