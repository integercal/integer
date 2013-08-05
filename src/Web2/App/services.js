define([
    'services/userData',
    'services/eventData',
    'services/localData',
    'services/eventTypeData',
    'services/groupData'
], function (userData, eventData, localData, eventTypes, groupData) {

    return angular.module('integerApp.services', ['ngResource'])
        .factory('eventData', eventData)
        .factory('userData', userData)
        .factory('localData', localData)
        .factory('eventTypeData', eventTypes)
        .factory('groupData', groupData);
 	});