define(function () {
    'use strict';

    return ['$scope', '$q', '$location', '$routeParams', 'facebook', '$i18next', 'eventData', 'eventTypeData', 'groupData', 'localData', 'userData', 'currentEvent',
        function ($scope, $q, $location, $routeParams, facebook, $i18next, eventData, eventTypeData, groupData, localData, userData, currentEvent) {

            $scope.validationMessage = "";
            var blankEvent = {
                id: "",
                group: "",
                subject: "",
                description: "",
                eventType: "",
                start: "",
                end: "",            
                locals: [],
                rsvp: []
            };

            $scope.selectedStart;
            $scope.selectedEnd;

            $scope.sameDayEvent = function () {
                return $scope.currentEvent && (new Date($scope.currentEvent.start).getDate() == new Date($scope.currentEvent.end).getDate());
            };

            var isNew = function () {
                return $scope.currentEvent.id == blankEvent.id;
            };

            $scope.shouldInviteFriends = false;
            $scope.isInvitingFriends = false;

            var init = function () {
                if (currentEvent) 
                    $scope.currentEvent = currentEvent;
                else
                    $scope.currentEvent = angular.copy(blankEvent);

                $scope.$apply();
            };

            $scope.backToCalendar = function () {
                $location.path("/");
            };

            $scope.closeAlert = function () {
                $scope.validationMessage = "";
            }

            $scope.saveEvent = function (eventForm) {
                $scope.formSubmitted = true;
                if (eventForm.$valid && $scope.currentEvent.locals.length > 0) {
                    $scope.isSaving = true;

                    var eventToSave = angular.copy($scope.currentEvent);
                    eventToSave.start = $scope.selectedStart;
                    eventToSave.end = $scope.selectedEnd;

                    if (eventToSave.id == "")
                        eventData.insert(eventToSave).then(saveSuccess, saveError);
                    else
                        eventData.update(eventToSave).then(saveSuccess, saveError);
                }
            };

            var saveSuccess = function () {
                toastr.success($i18next("eventForm.saveSuccess"));
                $scope.isSaving = false;
                $location.path("/");
            }

            var saveError = function (result) {
                $scope.validationMessage = result.data.error;
                $scope.isSaving = false;
            }

            var selectGroupForCurrentUser = function () {
                $scope.groups.$then(function (result) {
                    $scope.currentUser.$then(function (result) {
                        for (var i = 0; i < $scope.groups.length; i++) {
                            if (isNew() && $scope.groups[i].id == $scope.currentUser.groupId) {
                                $scope.currentEvent.group = $scope.groups[i];
                            }
                        }
                    });
                });
            }

            $scope.selectLocal = function (local) {
                local.selected = !local.selected;
                if (local.selected) {
                    $scope.currentEvent.locals.push(local);
                }
                else {
                    for (var i = 0; i < $scope.currentEvent.locals.length; i++) {
                        if ($scope.currentEvent.locals[i].id == local.id)
                            $scope.currentEvent.locals.splice(i, 1);
                    }
                }
            }

            var selectLocalsForCurrentEvent = function () {
                $scope.locals.$then(function (result) {
                    for (var i = 0; i < $scope.currentEvent.locals.length; i++) {
                        existing:
                            for (var j = 0; j < $scope.locals.length; j++) {
                                if ($scope.currentEvent.locals[i].id == $scope.locals[j].id) {
                                    $scope.locals[j].selected = true;
                                    break existing;
                                }
                            }
                    }
                });
            }

            $scope.rsvpMe = function () {
                eventData.rsvp($scope.currentEvent, $scope.currentUser.id).then(
                    function () {
                        $scope.currentEvent.rsvp.push({ name: $scope.currentUser.name, username: $scope.currentUser.username });
                        $scope.currentUserWillGo = true;
                        $scope.shouldInviteFriends = true;
                    },
                    function (response) {
                        toastr.error(response.error);
                    }
                );
            };
            
            $scope.desRsvpMe = function (index) {
                eventData.desRsvp($scope.currentEvent, $scope.currentUser.id).then(
                    function () {
                        $scope.currentUserWillGo = false;
                        $scope.currentEvent.rsvp.splice(index, 1);
                        toastr.success($i18next("eventReadOnly.notGoingSuccess"));
                    },
                    function (response) {
                        toastr.error(response.error);
                    }
                );
            };

            $scope.$watch(userData.currentUser, function (currentUser) {
                $scope.currentUser = currentUser;
                $scope.isAuthenticated = !!currentUser;
                $scope.currentUserWillGo = false;

                $scope.canEdit = currentUser && (currentUser.role == "admin"
                    || (isNew() === true || currentUser.groupId == $scope.currentEvent.group.id));
                
                if (currentUser) {
                    currentUser.$then(function () {  
                        if ($scope.canEdit) {
                            if (!isNew()) {
                                $scope.selectedStart = moment($scope.currentEvent.start).format('L H:mm');
                                $scope.selectedEnd = moment($scope.currentEvent.end).format('L H:mm');
                            }
                            $scope.locals = localData.locals();
                            selectLocalsForCurrentEvent();
                            eventTypeData.types().$then(function (result) {
                                $scope.eventTypes = result.data;
                                for (var i = 0; i < $scope.eventTypes.length; i++) {
                                    if (result.data[i].id == $scope.currentEvent.eventType.id)
                                        $scope.currentEvent.eventType = $scope.eventTypes[i];
                                }
                            });
                            groupData.groups().$then(function (result) {
                                $scope.groups = result.data;
                                for (var i = 0; i < $scope.groups.length; i++) {
                                    if (result.data[i].id == $scope.currentEvent.group.id)
                                        $scope.currentEvent.group = $scope.groups[i];
                                }
                            });
                            selectGroupForCurrentUser();
                        }
                        for (var i = 0; i < $scope.currentEvent.rsvp.length; i++) {
                            if (currentUser.username == $scope.currentEvent.rsvp[i].username) {
                                $scope.currentUserWillGo = true;
                                break;
                            }
                        }
                    });
                }
            });

            $scope.inviteFriends = function () {
                $scope.isInvitingFriends = true;
                facebook.share($location.absUrl(), currentEvent.subject, $i18next("eventReadOnly.imGoingFacebookMessage"), $scope.confirmInvitedFriends);
            };

            $scope.confirmInvitedFriends = function () {
                $scope.shouldInviteFriends = false;
                $scope.isInvitingFriends = false;
                if(!$scope.$$phase)
                    $scope.$apply();
            };

            init();
        }
    ];
});