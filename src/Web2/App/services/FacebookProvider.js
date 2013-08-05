define([], function () {
    var fb = angular.module('FacebookProvider', []);
    fb.factory('facebook', function ($rootScope, $q) {
        return {
            getLoginStatus: function () {
                FB.getLoginStatus(function (response) {
                    $rootScope.$broadcast("fb_statusChange", { 'status': response.status });
                }, true);
            },
            getUserData: function (callbak) {
                FB.api('/me', callbak);
            },
            share: function (url, subject, description, callback) {
                FB.ui({
                    method: 'feed',
                    link: url,
                    //picture: '',
                    name: subject,
                    description: description
                }, function (response) {
                    callback();
                });
            },
            login: function () {
                FB.getLoginStatus(function (response) {
                    switch (response.status) {
                        case 'connected':
                            $rootScope.$broadcast('fb_connected', { facebookId: response.authResponse.userID });
                            break;
                        case 'not_authorized':
                        case 'unknown':
                            FB.login(function (response) {
                                if (response.authResponse) {
                                    $rootScope.$broadcast('fb_connected', {
                                        facebookId: response.authResponse.userID,
                                        userNotAuthorized: true
                                    });
                                } else {
                                    $rootScope.$broadcast('fb_login_failed');
                                }
                            }, { scope: 'email' });
                            break;
                        default:
                            FB.login(function (response) {
                                if (response.authResponse) {
                                    $rootScope.$broadcast('fb_connected', { facebookId: response.authResponse.userID });
                                    $rootScope.$broadcast('fb_get_login_status');
                                } else {
                                    $rootScope.$broadcast('fb_login_failed');
                                }
                            });
                            break;
                    }
                }, true);
            },
            logout: function () {
                FB.logout(function (response) {
                    if (response) {
                        $rootScope.$broadcast('fb_logout_succeded');
                    } else {
                        $rootScope.$broadcast('fb_logout_failed');
                    }
                });
            },
            unsubscribe: function () {
                FB.api("/me/permissions", "DELETE", function (response) {
                    $rootScope.$broadcast('fb_get_login_status');
                });
            }
        };
    });
    return fb;
});