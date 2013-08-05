define(function () {
    'use strict';

    return ['$scope', 'eventData', 'userData', '$location', '$i18next', function ($scope, eventData, userData, $location, $i18next) {

        $scope.events = [];
        $scope.currentViewType = "month";
        $scope.currentDate = new Date();
        $scope.culture = navigator.language;
        $scope.localizedTexts = {
            buttonToday: $i18next("calendarForm.buttonToday"),
            buttonDayView: $i18next("calendarForm.buttonDayView"),
            buttonWeekView: $i18next("calendarForm.buttonWeekView"),
            buttonMonthView: $i18next("calendarForm.buttonMonthView"),
            buttonListView: $i18next("calendarForm.buttonListView"),
            labelToday: $i18next("calendarForm.labelToday"),
            labelAllDay: $i18next("calendarForm.labelAllDay")
        };
        $scope.dayViewHeaderTexts = {
            day: $i18next("calendarForm.dayViewHeaderDay"),
            week: $i18next("calendarForm.dayViewHeaderWeek"),
            list: $i18next("calendarForm.dayViewHeaderList")
        };

        var init = function () {
            loadEvents($scope.currentDate);
        };

        $scope.scheduleNew = function () {
            $location.path("/evento");
        };

        $scope.editEvent = function (e, args) {
            var event = args.data;
            $scope.$apply(function () {
                $location.path("/evento/" + event.id);
            });
            e.preventDefault();
        }

        $scope.selectedDateChanged = function (e, args) {
            var date = args.selectedDates[0];
            loadEvents(date);
        }

        $scope.$watch(userData.currentUser, function (currentUser) {
            $scope.currentUser = currentUser;
            $scope.isAuthenticated = !!currentUser;
            $scope.canSchedule = currentUser && (currentUser.role == "admin" || currentUser.role == "agente");
        });

        var loadEvents = function (date) {
            $scope.events = eventData.events(date, $scope.currentViewType);
            $scope.events.$then(function () {
                formatDateTimeForWijevcal();
            });
            $scope.$apply();
        }

        var formatDateTimeForWijevcal = function () {
            for (var i = 0; i < $scope.events.length; i++) {
                $scope.events[i].start = moment($scope.events[i].start).toDate();
                $scope.events[i].end = moment($scope.events[i].end).toDate();
            }
        }

        init();
    }];
});