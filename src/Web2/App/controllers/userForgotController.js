define(function () {
    'use strict';

    return ['$scope', '$i18next', '$routeParams', '$location', 'passwordData', 'willChange', function ($scope, $i18next, $routeParams, $location, passwordData, willChange) {
        
        $scope.userEmail = "";

        $scope.requestChange = {
            id: "",
            token: "",
            reset: "",
            newPassword: "",
            newPassword2: ""
        };

        var init = function () {
            if (willChange == false)
                $location.path("/");

            $scope.requestChange.id = $routeParams.id;
            $scope.requestChange.token = $routeParams.token;
            $scope.requestChange.reset = $routeParams.reset;
            $scope.$apply();
        }

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
            $location.path("/");
        }

        var sendError = function (result) {
            $scope.validationMessage = result.data.error;
            $scope.isSending = false;
        }

        $scope.changePassword = function (userForm) {
            $scope.formSubmitted = true;
            if (userForm.$valid) {
                $scope.isSending = true;
                passwordData.change($scope.requestChange).then(function (result) {
                    changeSuccess();
                }, sendError);
            }
        }

        var changeSuccess = function () {
            toastr.success($i18next("forgotPassword.changeSuccess"));
            $scope.isSending = false;
            $location.path("/");
        }

        init();
    }]
});