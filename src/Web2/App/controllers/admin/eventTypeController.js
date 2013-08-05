define(function () {
    'use strict';

    return ['$scope', '$i18next', 'eventTypeData', 'existingTypes',
        function ($scope, $i18next, eventTypeData, existingTypes) {        

            var blankType = {
                id: "",
                name: "",
                priority: null
            };

            var init = function () {
                $scope.newType = angular.copy(blankType);
                $scope.existingTypes = existingTypes;
                //$scope.$apply();
            };

            $scope.edit = function (type) {
                type.editing = true;
            };

            $scope.cancelEdit = function (type) {
                type.editing = false;
            };

            $scope.canDelete = function (type) {
                type.canDelete = true;
            };

            $scope.delete = function ($index, type) {
                eventTypeData.delete(type.id).$then(
                    function (response) {
                        $scope.existingTypes.splice($index, 1);
                        refresh();
                    }, callbackError);
            };

            $scope.insert = function (type) {
                var newType = angular.copy(type);

                eventTypeData.insert(newType)
                    .then(function (response) {
                        type = angular.copy(blankType);
                        newType = angular.copy(response);
                        $scope.existingTypes.push(newType);
                        saveSuccess();
                    },
                    callbackError);
            }

            $scope.update = function (type) {
                eventTypeData.update(type).then(function (response) {
                    type.editing = false;
                    saveSuccess();
                }, callbackError);
            }

            var saveSuccess = function () {
                toastr.success($i18next("eventTypeForm.saveSuccess"));
            }

            var callbackError = function (result) {
                toastr.error(result.data.error);
            }

            $scope.updateOrder = function () {
                for (var i = 0; i < $scope.existingTypes.length; i++) {
                    var priority = i + 1;
                    $scope.existingTypes[i].priority = priority;
                }

                eventTypeData.updateOrder($scope.existingTypes).then(function (response) {
                    saveSuccess();
                }, callbackError);
            }

            var refresh = function () {
                eventTypeData.types().$then(function (result) {
                    $scope.existingTypes = result.data;
                });
            };

            init();
        }
    ];
});