define(function () {
    'use strict';

    return ['$scope', '$i18next', 'passwordData', function ($scope, $i18next, passwordData) {
        
        $scope.userEmail = "";
        $scope.newPassword = "";
        $scope.newPassword2 = "";

        $scope.sendPassword = function (userForm) {
            $scope.formSubmitted = true;
            if (userForm.$valid) {
                $scope.isSending = true;
                passwordData.send($scope.userEmail).then(function (result) {
                    sendSuccess();
                }, sendError);
            }
        }

        var sendSuccess = function () {
            toastr.success($i18next("forgotPassword.sendSuccess"));
            $scope.isSending = false;
        }

        var sendError = function (result) {
            $scope.validationMessage = result.data.error;
            $scope.isSending = false;
        }

        $scope.changePassword = function (userForm) {
            $scope.formSubmitted = true;
            if (userForm.$valid) {
                $scope.isSending = true;
                passwordData.change($scope.userEmail).then(function (result) {
                    changeSuccess();
                }, sendError);
            }
        }

        var changeSuccess = function () {
            toastr.success($i18next("forgotPassword.changeSuccess"));
            $scope.isSending = false;
        }
    }]
});