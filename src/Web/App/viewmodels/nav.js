define(['services/logger'], function (logger) {
    var self = this;

    self.loginAsAdmin = ko.observable(false);

    self.activate = function () {
        return true;
    }

    self.toggleLoginAsAdmin = function () {
        self.loginAsAdmin(!self.loginAsAdmin());
    }
    
    return self;
});