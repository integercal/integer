var root = this;
define('jquery', [], function () { return root.jQuery; });

require.config({
    baseUrl: '/app'
});
    
require([
	'jquery',
	'app',
	'routes'
], function ($, app, routes) {
    'use strict';

    $(document).ready(function () {
        // TODO use this if while not fixes https://github.com/timrwood/moment/issues/422
        var lang = navigator.language.toLowerCase();
        if (lang.substring(0, 2) == 'en')
            lang = lang.substring(0, 2);
        console.log("moment.lang " + lang);
        moment.lang(lang);

        toastr.options.positionClass = 'toast-bottom-right';
        toastr.options.backgroundpositionClass = 'toast-bottom-right';

        var $html = $('html');
        angular.bootstrap($html, ['integerApp']);
        $html.addClass('ng-app');
    });

});

var global = {
    getBootstrapDatetimepickerFormat: function () {
        var lang = navigator.language;
        if (lang == "en-US")
            return "mm/dd/yyyy hh:ii";
        else if (lang == "pt-BR")
            return "dd/mm/yyyy hh:ii";
    }
}