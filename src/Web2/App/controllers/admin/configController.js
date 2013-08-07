define(function () {
    'use strict';

    return ['$scope', '$i18next', 'configData', 'currentParish', function ($scope, $i18next, configData, currentParish) {
        
        var init = function () {
            $scope.currentParish = currentParish;
            $scope.$apply();
        }

        $scope.clickTab = function (tabId) {
            $scope.activeTab = tabId;
        }

        init();
    }]
});