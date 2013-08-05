define(function () {
    'use strict';

    return ['$scope', '$i18next', 'localData', 'existingLocals',
    function ($scope, $i18next, localData, existingLocals) {

            var blankLocal = {
                id: "",
                name: ""
            };

            var init = function () {
                $scope.newLocal = angular.copy(blankLocal);
                $scope.existingLocals = existingLocals;
                $scope.$apply();
            };

            $scope.edit = function (local) {
                local.editing = true;
            };

            $scope.cancelEdit = function (local) {
                local.editing = false;
            };

            $scope.canDelete = function (local) {
                local.canDelete = true;
            };

            $scope.delete = function ($index, local) {
                localData.delete(local.id).$then(
                    function () {
                        $scope.existingLocals.splice($index, 1);
                    }, callbackError);
            };

            $scope.insert = function (local) {
                var newLocal = angular.copy(local);

                localData.insert(newLocal)
                    .then(function (response) {
                        local = angular.copy(blankLocal);
                        newLocal.id = response.id;
                        $scope.existingLocals.push(newLocal);
                        saveSuccess();
                    },
                    callbackError);
            }

            $scope.update = function (local) {
                localData.update(local).then(function (response) {
                    local.editing = false;
                    saveSuccess();
                }, callbackError);
            }

            var saveSuccess = function () {
                toastr.success($i18next("localForm.saveSuccess"));
            }

            var callbackError = function (result) {
                toastr.error(result.data.error);
            }

            init();
        }
    ];
});