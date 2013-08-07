define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', '$http', '$rootScope', function ($resource, REST_HOST, $q, $http, $rootScope) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/UsuariosSenha', {}, {
            send: { method: 'POST', params: { email: '@email' } },
            change: { method: 'PUT', params: { id: '@id', token: '@token', newPassword: '@newPassword' } }
        });

        self.send = function (email) {
            var deferred = $q.defer();
            resource.send({ email: email },
                function (response) {
                    deferred.resolve(response);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }

        self.change = function (request) {
            var deferred = $q.defer();
            resource.change(request,
                function (response) {
                    deferred.resolve(response);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }

        self.cancel = function (request) {
            var deferred = $q.defer();
            resource.delete(request).$then(
                function (response) {
                    deferred.resolve(response);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }

        return self;
    }]
});