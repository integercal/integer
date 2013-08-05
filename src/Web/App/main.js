require.config({
    paths: { "text": "durandal/amd/text" }
});

var root = this;
define('jquery', [], function () { return root.jQuery; });
define('ko', [], function () { return root.ko; });
define('breeze', [], function () { return root.breeze; });

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'durandal/plugins/router', 'services/logger', 'ko.bindingHandlers'],
    function (app, viewLocator, system, router, logger) {

    // Enable debug message to show in the console 
    system.debug(true);

    app.start().then(function () {
        toastr.options.positionClass = 'toast-top-right';
        toastr.options.backgroundpositionClass = 'toast-top-right';

        router.handleInvalidRoute = function (route, params) {
            logger.logError('No Route Found', route, 'main', true);
        };

        // When finding a viewmodel module, replace the viewmodel string 
        // with view to find it partner view.
        router.useConvention();
        viewLocator.useConvention();

        // Adapt to touch devices
        app.adaptToDevice();
        //Show the app by setting the root view model for our application.
        app.setRoot('viewmodels/shell', 'entrance');
    });
});