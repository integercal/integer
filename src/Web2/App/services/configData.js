define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', '$http', '$rootScope', function ($resource, REST_HOST, $q, $http, $rootScope) {
        
        var self = this;

        var resource = $resource(REST_HOST + '/configuracoes');

        self.get = function () {
            return resource.get();
        }

        return self;
    }]
});