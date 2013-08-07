'use strict';

angular.module('integerApp.directives', [])
    .directive('dateTime', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                $(element).parent().datetimepicker({
                    language: navigator.language,
                    format: global.getBootstrapDatetimepickerFormat(),
                    autoclose: true,
                    minuteStep: 10,
                    pickerPosition: "bottom-left",
                    startDate: new Date()
                });

                if (!ngModel) {
                    console.log('no model, returning');
                    return;
                }

                element.bind('blur keyup change', function () {
                    scope.$apply(read);
                });

                read();

                function read() {
                    ngModel.$setViewValue(element.val());
                }
            }
        }
    })

    .directive('colorPicker', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {

                element.colorPicker({
                    colors: ["ff4242", "ffb380", "ffff5c", "00e600", "7070ff", "f500f5", "575757", "d9d9d9", "00FFFF"]
                });

                ngModel.$render = function () {
                    element.val(ngModel.$viewValue);
                    element.change();
                };

                element.bind('change', function (event) {
                    ngModel.$setViewValue(element.val().replace("#",""));
                });
            }
        }
    })

    .directive("repeatPassword", function () {
        return {
            require: "ngModel",
            link: function (scope, elem, attrs, ctrl) {
                var otherInput = elem.inheritedData("$formController")[attrs.repeatPassword];

                ctrl.$parsers.push(function (value) {
                    if (value === otherInput.$viewValue) {
                        ctrl.$setValidity("repeat", true);
                        return value;
                    }
                    ctrl.$setValidity("repeat", false);
                });

                otherInput.$parsers.push(function (value) {
                    ctrl.$setValidity("repeat", value === ctrl.$viewValue);
                    return value;
                });
            }
        };
    });;
