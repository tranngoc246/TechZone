/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('techzone.product_categories', ['techzone.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('product_categories', {
            url: "/product_categories",
            parent: 'base',
            templateUrl: "/app/components/product_categories/productCategoryListView.html",
            controller: "productCategoryListController"
        }).state('add_product_category', {
            url: "/add_product_category",
            parent: 'base',
            templateUrl: "/app/components/product_categories/productCategoryAddView.html",
            controller: "productCategoryAddController"
        }).state('edit_product_category', {
            url: "/edit_product_category/:id",
            parent: 'base',
            templateUrl: "/app/components/product_categories/productCategoryEditView.html",
            controller: "productCategoryEditController"
        }).state('manufacturer', {
            url: "/manufacturer/:id",
            parent: 'base',
            templateUrl: "/app/components/manufacturer/manufacturerListView.html",
            controller: "manufacturerListController"
        }).state('add_manufacturer', {
            url: "/add_manufacturer/:id",
            parent: 'base',
            templateUrl: "/app/components/manufacturer/manufacturerAddView.html",
            controller: "manufacturerAddController"
        }).state('edit_manufacturer', {
            url: "/edit_manufacturer/:idCategory/:idManufacturer",
            parent: 'base',
            templateUrl: "/app/components/manufacturer/manufacturerEditView.html",
            controller: "manufacturerEditController"
        });
    }
})();