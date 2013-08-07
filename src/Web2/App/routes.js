define(['app'], function (integerApp) {
    'use strict';

    return integerApp.config(function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(false).hashPrefix('!');

        $routeProvider.when('/', {
            templateUrl: '/app/views/calendar.html',
            controller: 'CalendarController'
        });
        $routeProvider.when('/eventos', {
            templateUrl: '/app/views/eventList.html',
            controller: 'EventListController'
        });
        $routeProvider.when('/config', {
            templateUrl: '/app/views/config.html',
            controller: 'ConfigController'
        });
        $routeProvider.when('/grupos', {
            templateUrl: '/app/views/admin/groups.html',
            controller: 'GroupController',
            resolve: {
                existingGroups: function (groupData, $q) {
                    var deferred = $q.defer();
                    groupData.groups().$then(function (result) {
                        deferred.resolve(result.data);
                    }, function (errorData) {
                        deferred.reject();
                    });
                    return deferred.promise;
                }
            }
        });
        $routeProvider.when('/locais', {
            templateUrl: '/app/views/admin/locals.html',
            controller: 'LocalController',
            resolve: {
                existingLocals: function (localData, $q) {
                    var deferred = $q.defer();
                    localData.locals().$then(function (result) {
                        deferred.resolve(result.data);
                    }, function (errorData) {
                        deferred.reject();
                    });
                    return deferred.promise;
                }
            }
        });
        $routeProvider.when('/tipos', {
            templateUrl: '/app/views/admin/types.html',
            controller: 'EventTypeController',
            resolve: {
                existingTypes: function (eventTypeData, $q) {
                    var deferred = $q.defer();
                    eventTypeData.types().$then(function (result) {
                        deferred.resolve(result.data);
                    }, function (errorData) {
                        deferred.reject();
                    });
                    return deferred.promise;
                }
            }
        });
        $routeProvider.when('/usuarios', {
            templateUrl: '/app/views/admin/users.html',
            controller: 'UserController',
            resolve: {
                existingUsers: function (userData, $q) {
                    var deferred = $q.defer();
                    userData.users().$then(function (result) {
                        deferred.resolve(result.data);
                    }, function (errorData) {
                        deferred.reject(); 
                    });
                    return deferred.promise;
                }
            }
        });
        $routeProvider.when('/evento', {
            templateUrl: '/app/views/event.html',
            controller: 'EventController',
            resolve: {
                currentEvent: function () {
                    return undefined;
                }
            }
        });
        $routeProvider.when('/evento/:eventId', {
            templateUrl: '/app/views/event.html',
            controller: 'EventController',
            resolve: {
                currentEvent: function ($route, eventData) {
                    var eventId = $route.current.params.eventId;
                    if (eventId)
                        return eventData.get(eventId);
                    else
                        return null;
                }
            }
        });
        $routeProvider.when('/esqueci', {
            templateUrl: '/app/views/user-passwordforgot.html',
            controller: 'UserForgotController',
            resolve: {
                willChange: function () {
                    return undefined;
                }
            }
        });
        $routeProvider.when('/trocar', {
            templateUrl: '/app/views/user-passwordchange.html',
            controller: 'UserForgotController',
            resolve: {
                willChange: function ($route, $q, passwordData) {
                    var deferred = $q.defer();
                    var reset = $route.current.params.reset;
                    if (reset === 'true') {
                        deferred.resolve(true);
                    }
                    else {
                        var id = $route.current.params.id;
                        var token = $route.current.params.token;
                        passwordData.cancel({ id: id, token: token }).then(function () {
                            deferred.resolve(false);
                        }, function () {
                            deferred.reject();
                        });
                    }
                    return deferred.promise;
                }
            }
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    });

});