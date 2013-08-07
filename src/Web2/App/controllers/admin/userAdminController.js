define(function () {
    'use strict';

    return ['$scope', '$i18next', 'userData',
    function ($scope, $i18next, userData) {
        
        var blankUser = {
            id: "",
            name: "",
            email: ""
        };

        var init = function () {
            $scope.currentEditing = angular.copy(blankUser);
            $scope.$apply();
        };

        $scope.addAdmin = function () {
            $scope.isEditing = true;
        }

        $scope.cancelEditAdmin = function () {
            $scope.currentEditing = angular.copy(blankUser);
            $scope.isEditing = false;
            $scope.closeAlert();

        }

        $scope.save = function (userForm) {
            $scope.formSubmitted = true;
            if (userForm.$valid) {
                $scope.isSaving = true;

                var userToSave = angular.copy($scope.currentEditing);
                if (userToSave.id == "") {
                    userData.insert(userToSave).then(function (result) {
                        console.log("inserted");
                        console.log(result);
                        $scope.existingUsers.push(result);
                        saveSuccess();
                    }, saveError);
                }
                else
                    userData.update(userToSave).then(saveSuccess, saveError);
            }
        }

        var saveSuccess = function () {
            toastr.success($i18next("userAddModal.saveSuccess"));
            $scope.isSaving = false;
            $scope.cancelEditAdmin();
        }

        var saveError = function (result) {
            $scope.validationMessage = result.data.error;
            $scope.isSaving = false;
        }

        $scope.closeAlert = function () {
            $scope.validationMessage = "";
        }

        init();
    }];
});