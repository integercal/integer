define(function () {
    'use strict';

    return ['$scope', '$timeout', 'eventData',
        function ($scope, $timeout, eventData) {

            var init = function () {
                //clock();
                eventData.events(new Date(), 'day').$then(function (result) {
                    $scope.events = result.data;
                    for (var i = 0; i < $scope.events.length; i++) {
                        var date = new Date($scope.events[i].start);
                        console.log(date);
                        var formattedTime = (date.getHours() + 3) + ':' + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes();
                        $scope.events[i].start = formattedTime;
                    }
                });
                
            };

            $scope.onTimeout = function () {
                $scope.timenow = moment().format('hh:mm:ss');
                mytimeout = $timeout($scope.onTimeout, 1000);
            }
            var mytimeout = $timeout($scope.onTimeout, 1000)

            init();
        }
    ];
});