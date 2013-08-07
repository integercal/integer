define([
    'services',
    'controllers',
    'directives',
    'services/FacebookProvider'
], function () {
    'use strict';
    
    var REST_HOST = '/api';

    var app = angular.module('integerApp', ['wijmo', 'jm.i18next', 'ngCookies', 'ui.directives', 'ngDragDrop', 'integerApp.controllers', 'integerApp.services', 'integerApp.directives', 'FacebookProvider']);

    app.constant('REST_HOST', REST_HOST)
        .config(function ($httpProvider, $i18nextProvider) {
            $httpProvider.defaults.headers.common = { 'Content-Type': 'application/json' };
            $httpProvider.defaults.headers.common = { 'Accept-Language': navigator.language };            

            var token = $.cookie('auth');
            if (token)
                $httpProvider.defaults.headers.common['Authorization'] = "Basic " + token;
        })
        .config(function ($i18nextProvider) {
            $i18nextProvider.options = {
                lng: navigator.language || navigator.userLanguage,
                useCookie: false,
                useLocalStorage: false,
                fallbackLng: 'pt-BR',
                resGetPath: '/app/locales/__lng__/__ns__.js'
            };
        })
        .run(function ($rootScope, $http, userData, facebook) {
            initFacebookSDK($rootScope);

            $rootScope.$on('event:loginConfirmed', function (event, login) {
                if (login.remember) 
                    $.cookie('auth', login.token, { expires: 10 * 365 });
                else 
                    $.cookie('auth', login.token);
            });

            $rootScope.$on('event:logout', function () {
                $.cookie('auth', '');
            });

            $rootScope.$on('fb_connected', function (event, args) {
                facebook.getUserData(function (fbData) {
                    $rootScope.$apply(function () {
                        userData.loginUserFromFacebook({
                            facebookId: fbData.id,
                            name: fbData.name,
                            email: fbData.email,
                            gender: fbData.gender,
                            userName: fbData.username
                        });
                    });
                });
            });

            $rootScope.$on('fb_statusChange', function (event, args) {
                var status = args.status; 
                if (status == "not_authorized") { //TODO: if want auto login, check is status is 'connected' too
                    userData.logout();
                }
            });

            var token = $.cookie('auth', token);
            userData.getByToken(token);
        });

    function initFacebookSDK($rootScope) {
        window.fbAsyncInit = function () {
            FB.init({
                appId: '498071616908752',                        
                status: true,
                cookie: true,
                xfbml: true                                  
            });
            //TODO: if want auto login, check is status is 'connected'
            //FB.Event.subscribe('auth.statusChange', function (response) {
            //    $rootScope.$broadcast("fb_statusChange", { 'status': response.status });
            //});
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    }

    return app;
});