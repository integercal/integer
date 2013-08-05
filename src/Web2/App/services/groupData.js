define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', function ($resource, REST_HOST, $q) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/grupos/:groupId', { groupId: '@groupId' }, {
            insert: { method: 'POST' },
            update: { method: 'PUT' }
        });

        self.groups = function () {
            return resource.query();
        }

        self.insert = function (theResource) {
            var deferred = $q.defer();
            resource.insert(theResource,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.update = function (theResource) {
            var deferred = $q.defer();
            resource.update(theResource,
                function (key) {
                    deferred.resolve(key);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }

        self.delete = function (id) {
            return resource.delete({ groupId: id });
        };

        return self;
    }];
});