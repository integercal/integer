define(['services'],
 	function () {  

 	    return angular.module('integerApp.controllers', ['integerApp.services', 'FacebookProvider', 'jm.i18next'])
		    .controller('HeaderController', ['$scope', '$location', '$rootScope', '$cookieStore', 'facebook', 'userData', function ($scope, $location, $rootScope, $cookieStore, facebook, userData) {
		        require(['controllers/headerController'], function (controller) {
		            angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$location': $location,
                            '$rootScope': $rootScope,
                            '$cookieStore': $cookieStore,
                            'facebook': facebook,
                            'userData': userData
                        }
                    );
		        });
		    }])
            .controller('UserForgotController', ['$scope', '$i18next', '$routeParams', '$location', 'passwordData', 'willChange', function ($scope, $i18next, $routeParams, $location, passwordData, willChange) {
                require(['controllers/userForgotController'], function (controller) {
                    angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            '$routeParams': $routeParams,
                            '$location': $location,
                            'passwordData': passwordData,
                            'willChange': willChange
                        }
                    );
                });
            }])
 	        .controller('CalendarController', ['$scope', 'eventData', 'userData', '$location', '$i18next',
 	        function ($scope, eventData, userData, $location, $i18next) {
 	                require(['controllers/calendarController'], function (controller) {
 	                    angular.injector(['ng']).invoke(
                            controller,
                            this,
                            {
                                '$scope': $scope,
                                'eventData': eventData,
                                'userData': userData,
                                '$location': $location,
                                '$i18next': $i18next
                            }
                        );
 	            });
 	        }])
            .controller('LocalController', ['$scope', '$i18next', 'localData', 'existingLocals',
 	        function ($scope, $i18next, localData, existingLocals) {
 	            require(['controllers/admin/localController'], function (controller) {
 	                angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            'localData': localData,
                            'existingLocals': existingLocals
                        }
                    );
 	            });
 	        }])
            .controller('GroupController', ['$scope', '$i18next', 'groupData', 'existingGroups',
 	        function ($scope, $i18next, groupData, existingGroups) {
 	            require(['controllers/admin/groupController'], function (controller) {
 	                angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            'groupData': groupData,
                            'existingGroups': existingGroups
                        }
                    );
 	            });
 	        }])
            .controller('UserController', ['$scope', '$i18next', 'userData', 'existingUsers',
 	        function ($scope, $i18next, userData, existingUsers) {
 	            require(['controllers/admin/userController'], function (controller) {
 	                angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            'userData': userData,
                            'existingUsers': existingUsers
                        }
                    );
 	            });
 	        }])
            .controller('UserAdminController', ['$scope', '$i18next', 'userData',
 	        function ($scope, $i18next, userData) {
 	            require(['controllers/admin/userAdminController'], function (controller) {
 	                angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            'userData': userData
                        }
                    );
 	            });
 	        }])
            .controller('EventTypeController', ['$scope', '$i18next', 'eventTypeData', 'existingTypes',
 	        function ($scope, $i18next, eventTypeData, existingTypes) {

 	            // TODO: because of uiSortable runs before require, we needed to put sortableOptions here
 	            $scope.sortableOptions = {
 	                stop: function (e, ui) {
 	                    $scope.updateOrder();
 	                },
 	                axis: 'y',
 	                cursor: 's-resize'
 	            };

 	            require(['controllers/admin/eventTypeController'], function (controller) {
 	                angular.injector(['ng']).invoke(
                        controller,
                        this,
                        {
                            '$scope': $scope,
                            '$i18next': $i18next,
                            'eventTypeData': eventTypeData,
                            'existingTypes': existingTypes
                        }
                    );
 	            });
 	        }])
 	        .controller('EventController', ['$scope', '$q', '$location', 'facebook', '$i18next', 'eventData', 'eventTypeData', 'groupData', 'localData', 'userData', 'currentEvent',
 	        function ($scope, $q, $location, facebook, $i18next, eventData, eventTypeData, groupData, localData, userData, currentEvent) {
 	                require(['controllers/eventController'], function (controller) {
 	                    angular.injector(['ng']).invoke(
                            controller,
                            this,
                            {
                                '$scope': $scope,
                                '$location': $location,
                                'facebook': facebook,
                                '$i18next': $i18next,
                                'eventData': eventData,
                                'eventTypeData': eventTypeData,
                                'groupData': groupData,
                                'localData': localData,
                                'userData': userData,
                                'currentEvent': currentEvent
                            }
                        );
 	            });
 	        }])
 	        .controller('EventListController', ['$scope', '$timeout', 'eventData',
 	            function ($scope, $timeout, eventData) {
 	                require(['controllers/eventListController'], function (controller) {
 	                    angular.injector(['ng']).invoke(
                            controller,
                            this,
                            {
                                '$scope': $scope,
                                '$timeout': $timeout,
                                'eventData': eventData
                            }
                        );
 	                });
 	            }]);
 	});