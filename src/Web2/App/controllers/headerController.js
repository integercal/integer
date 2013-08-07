define(function () {
    'use strict';

    return ['$scope', '$location', '$cookieStore', 'facebook', 'userData', function ($scope, $location, $cookieStore, facebook, userData) {
        
        $scope.user = {};
        $scope.currentUser = {};
        $scope.isLoggingIn = false;
        $scope.loginAsAdmin = false;
        $scope.loginError = false;
        $scope.loginMessage = '';

        $scope.isFacebookUser = false;

        $scope.toggleLoginAsAdmin = function () {
            $scope.loginAsAdmin = !$scope.loginAsAdmin;
        }

        $scope.doLoginFB = function () {
            facebook.login();
        }

        $scope.doLogin = function (userForm) {
            $scope.formSubmitted = true;
            if (userForm.$valid) {
                $scope.isLoggingIn = true;
                userData.loginAdmin($scope.user, loginSuccess, loginError).then(loginSuccess, loginError);
            }
        }

        $scope.resetForm = function () {
            $scope.user = {};
            $scope.loginError = false;
            $scope.toggleLoginAsAdmin();
        }

        function loginSuccess(result) {
            $scope.resetForm();
            $scope.isLoggingIn = false;
        }

        function loginError(result) {
            $scope.isLoggingIn = false;
            $scope.loginError = true;
            $scope.loginMessage = result.data.error;
        }

        $scope.doLogout = function () {
            userData.logout();
        }

        $scope.$watch(userData.currentUser, function (currentUser) {
            $scope.currentUser = currentUser;
            $scope.isAuthenticated = !!currentUser;
            $scope.isFacebookUser = currentUser && !!currentUser.username;
        });

        $scope.forgotPassword = function () {
            $scope.resetForm();
            $location.path("/esqueci");
        }
    }]
});