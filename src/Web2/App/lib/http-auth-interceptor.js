(function () {
    'use strict';

    angular.module('http-auth-interceptor', ['http-auth-interceptor-buffer'])

    .factory('authService', ['$rootScope', 'httpBuffer', function ($rootScope, httpBuffer) {
        return {
            loginConfirmed: function (data) {
                $rootScope.$broadcast('event:auth-loginConfirmed', data);
                httpBuffer.retryAll();
            }
        };
    }])
            
    .config(['$httpProvider', function ($httpProvider) {

        var interceptor = ['$rootScope', '$q', 'httpBuffer', function ($rootScope, $q, httpBuffer) {
            function success(response) {
                return response;
            }

            function error(response) {
                if (response.status === 401 && !response.config.ignoreAuthModule) {
                    var deferred = $q.defer();
                    httpBuffer.append(response.config, deferred);
                    $rootScope.$broadcast('event:auth-loginRequired');
                    return deferred.promise;
                }
                // otherwise, default behaviour
                return $q.reject(response);
            }

            return function (promise) {
                return promise.then(success, error);
            };

        }];
        $httpProvider.responseInterceptors.push(interceptor);
    }]);

    
    angular.module('http-auth-interceptor-buffer', [])

    .factory('httpBuffer', ['$injector', function ($injector) {
        var buffer = [];

        /** Service initialized later because of circular dependency problem. */
        var $http;

        function retryHttpRequest(config, deferred) {
            function successCallback(response) {
                deferred.resolve(response);
            }
            function errorCallback(response) {
                deferred.reject(response);
            }
            $http = $http || $injector.get('$http');
            $http(config).then(successCallback, errorCallback);
        }

        return {            
            append: function (config, deferred) {
                buffer.push({
                    config: config,
                    deferred: deferred
                });
            },            
            retryAll: function () {
                for (var i = 0; i < buffer.length; ++i) {
                    retryHttpRequest(buffer[i].config, buffer[i].deferred);
                }
                buffer = [];
            }
        };
    }]);
})();
