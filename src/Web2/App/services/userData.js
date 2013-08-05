define([], function () {
    "use strict";

    return ['$resource', 'REST_HOST', '$q', '$http', '$rootScope', function ($resource, REST_HOST, $q, $http, $rootScope) {
        
        var self = this;

        var User = $resource(REST_HOST + '/usuarios/:id', { id: '@id' }, {
            get: { method: 'GET', params: { token: '@token' } },
            loginAdmin: { method: 'POST', params: { email: '@email', senha: '@senha' } },
            loginFromFacebook: { method: 'PUT', params: { facebookData: '@facebookData' } }
        });
        self.current;

        self.isAuthenticated = function () {
            return !!self.current;
        };

        self.currentUser = function () {
            return self.current;
        };

        self.get = function (token) {
            return User.get({ token: token }, function (response) {
                self.current = response;
            });
        }

        self.loginAdmin = function (user) {
            var deferred = $q.defer();
            User.loginAdmin({ email: user.email, senha: user.password },
                function (response) {
                    self.getByToken(response.token, user.remember);
                    deferred.resolve(response);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }

        self.loginUserFromFacebook = function (fbData) {
            User.loginFromFacebook(fbData, function (resp) {
                self.getByToken(resp.token);
            });
        }

        self.getByToken = function (token, remember) {
            if (token) {
                $http.defaults.headers.common['Authorization'] = "Basic " + token;
                self.get(token);
                $rootScope.$broadcast('event:loginConfirmed', { token: token, remember: remember });
            }
        }

        self.logout = function () {
            self.current = null;
            $rootScope.$broadcast('event:logout');
        }

        return self;
    }]
});