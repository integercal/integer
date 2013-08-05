define(
    ['durandal/app', 'services/logger'],
    function (app, logger) {

        var EventoModal = function() {
            activate: activate;
            close: close;
        };

        EventoModal.close = function () {
            console.log('close');
            //this.modal.close(false);
        }

        return EventoModal;

        function activate() {
            return true;
        }        
    });