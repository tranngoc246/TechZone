/// <reference path="../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('techzone',
        ['techzone.products',
            'techzone.product_categories',
            'techzone.common'])
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        $urlRouterProvider.otherwise('/admin');
    }
})();