define(
    [
        'breeze',
        'services/dataservice.eventos'
    ],
    function (breeze, eventoDataService) {

        var self = this;

        breeze.NamingConvention.camelCase.setAsDefault();

        self.eventos = eventoDataService;

        return self;
    });