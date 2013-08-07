define(function () {
    'use strict';

    return ['$scope', '$i18next', 'userData', 'existingUsers',
    function ($scope, $i18next, userData, existingUsers) {

        var defaultTab = "agente";

        var blankUser = {
            id: "",
            name: ""
        };

        var init = function () {
            $scope.activeTab = defaultTab;
            $scope.newUser = angular.copy(blankUser);
            $scope.existingUsers = existingUsers;
            $scope.$apply();
        };

        $scope.edit = function (user) {
            user.editing = true;
        };

        $scope.cancelEdit = function (user) {
            user.editing = false;
        };

        $scope.canDelete = function (user) {
            user.canDelete = true;
        };

        $scope.delete = function ($index, user) {
            //TODO inactivate user
            //userData.delete(user.id).$then(
            //    function () {
            //        $scope.existingLocals.splice($index, 1);
            //    }, callbackError);
        };

        $scope.insert = function (user) {
            var newUser = angular.copy(user);

            userData.insert(newUser)
                .then(function (response) {
                    user = angular.copy(blankUser);
                    newUser.id = response.id;
                    $scope.existingUsers.push(newUser);
                    saveSuccess();
                },
                callbackError);
        }

        $scope.update = function (user) {
            userData.update(user).then(function (response) {
                user.editing = false;
                saveSuccess();
            }, callbackError);
        }

        var saveSuccess = function () {
            toastr.success($i18next("userForm.saveSuccess"));
        }

        var callbackError = function (result) {
            toastr.error(result.data.error);
        }

        $scope.clickTab = function (tabId) {
            $scope.activeTab = tabId;
        }

        $scope.queryUsers = function (item) {
            if (item.role == $scope.activeTab)
                return true;

            return false;
        }

        init();
    }];
});