(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.products = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProducts = getProducts;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteProduct = deleteProduct;

        $scope.selectAll = selectAll;

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.products, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.products, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedProducts: JSON.stringify(listId)
                }
            }
            apiService.del('api/product/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + $scope.selected.length + ' bản ghi.');
                getProducts(checkSearch);
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        $scope.$watch("products", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProduct(id) {
            $ngBootbox.confirm({
                size: 'small',
                title: '<b>Xác nhận xóa</b>',
                message: 'Bạn có chắc chắn muốn xóa không?',
                buttons: {
                    confirm: {
                        label: 'Xóa',
                        className: 'btn-danger btn-sm'
                    },
                    cancel: {
                        label: 'Hủy',
                        className: 'btn-secondary btn-sm'
                    }
                }
            }).then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/product/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công!');
                    $scope.getProducts(checkSearch);
                }, function () {
                    notificationService.displayError('Xóa không thành công!');
                });
            });
        }

        var checkSearch = false;

        function search() {
            checkSearch = true;
            getProducts(checkSearch);
        }
        function getProducts(page, check) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/product/getall', config, function (result) {
                if (check) {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                    }
                    else {
                        notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + ' bản ghi.');
                    }
                }
                $scope.products = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load product failed.');
            });
        }

        $scope.getProducts(checkSearch);
    }
})(angular.module('techzone.products'));