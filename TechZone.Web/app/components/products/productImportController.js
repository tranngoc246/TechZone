(function (app) {
    app.controller('productImportController', productImportController);

    productImportController.$inject = ['apiService', '$http', 'authenticationService', '$scope', 'notificationService', '$state', 'commonService'];

    function productImportController(apiService, $http, authenticationService, $scope, notificationService, $state, commonService) {

        $scope.files = [];
        $scope.categoryId = 0;
        $scope.ImportProduct = ImportProduct;
        $scope.flatFolders = [];
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                $scope.files.push(args.file);
            });
        });

        function ImportProduct() {
            authenticationService.setHeader();
            $http({
                method: 'POST',
                url: "/api/product/import",
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("categoryId", angular.toJson(data.categoryId));
                    for (var i = 0; i < data.files.length; i++) {
                        formData.append('file' + i, data.files[i]);
                    }
                    return formData;
                },
                data: { categoryId: $scope.categoryId, files: $scope.files }
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                $state.go('products');
            },
                function (data, status, headers, config) {
                    notificationService.displayError(data);
                });
        }
        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = commonService.getTree(result.data, "ID", "ParentID");
                $scope.parentCategories.forEach(function (item) {
                    recur(item, 0, $scope.flatFolders);
                });
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        function times(n, str) {
            var result = '';
            for (var i = 0; i < n; i++) {
                result += str;
            }
            return result;
        };
        function recur(item, level, arr) {
            arr.push({
                Name: times(level, '–') + ' ' + item.Name,
                ID: item.ID,
                Level: level,
                Indent: times(level, '–')
            });
            if (item.children) {
                item.children.forEach(function (item) {
                    recur(item, level + 1, arr);
                });
            }
        };
        loadProductCategory();
    }

})(angular.module('techzone.products'));