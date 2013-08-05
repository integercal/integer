define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', function ($resource, REST_HOST, $q) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/locais/:localId', { localId: '@localId' }, {
            insert: { method: 'POST' },
            update: { method: 'PUT' }
        });

        self.locals = function () {
            return resource.query();
        }

        self.insert = function (local) {
            var deferred = $q.defer();
            resource.insert(local,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.update = function (local) {
            var deferred = $q.defer();
            resource.update(local,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.delete = function (id) {
            return resource.delete({ localId: id });
        };

        return self;
    }];
});