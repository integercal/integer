define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', function ($resource, REST_HOST, $q) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/tiposdeevento/:typeId', { typeId: '@typeId' }, {
            insert: { method: 'POST' },
            update: { method: 'PUT', isArray: true }
        });

        self.types = function () {
            return resource.query();
        }
        
        self.insert = function (type) {
            var deferred = $q.defer();
            resource.insert(type,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.update = function (type) {
            var deferred = $q.defer();
            resource.update([type],
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.updateOrder = function (types) {
            var deferred = $q.defer();
            resource.update(types,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.delete = function (id) {
            return resource.delete({ typeId: id });
        };

        return self;
    }];
});