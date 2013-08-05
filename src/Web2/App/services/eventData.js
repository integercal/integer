define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', function ($resource, REST_HOST, $q) {
        
        var self = this;

        var eventResource = $resource(REST_HOST + '/eventos/:id', { id: '@id' }, {
            insert: { method: 'POST'},
            update: { method: 'PUT'}
        });

        self.events = function (date, viewType) {
            return eventResource.query({ date: date.toUTCString(), viewType: viewType });
        }

        self.get = function (id) {
            var deferred = $q.defer();
            eventResource.get({ id: id },
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }

        self.insert = function (event) {
            var deferred = $q.defer();
            eventResource.insert(event,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.update = function (event) {
            var deferred = $q.defer();
            eventResource.update(event,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        var rsvpResource = $resource(REST_HOST + '/eventos/:parentId/rsvp/:id', { parentId: '@parentId', id: '@id' }, {
            insert: { method: 'POST' }
        });

        self.rsvp = function (event, idUser) {
            var deferred = $q.defer();
            rsvpResource.insert({ parentId: event.id, idUser: idUser },
                function (success) {
                    deferred.resolve(success);
                },
                function (fail) {
                    deferred.reject(fail.data);
                });
            return deferred.promise;

        };

        self.desRsvp = function (event, idUser) {
            var deferred = $q.defer();
            rsvpResource.delete({ parentId: event.id, idUser: idUser },
                function (success) {
                    deferred.resolve(success);
                },
                function (fail) {
                    deferred.reject(fail.data);
                });
            return deferred.promise;
        };

        return self;
    }];
});