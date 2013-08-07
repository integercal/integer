define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', '$http', '$rootScope', function ($resource, REST_HOST, $q, $http, $rootScope) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/UsuariosSenha', {}, {
            send: { method: 'POST', params: { email: '@email' } },
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

        return self;
    }]
});