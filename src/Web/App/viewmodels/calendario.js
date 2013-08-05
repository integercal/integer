define(
    ['durandal/app', 'durandal/plugins/router', 'services/logger', 'services/dataservice', './eventoModal'],
    function (app, router, logger, dataservice, EventoModal) {

        var self = this;

        self.currentDate = ko.observable(new Date());
        self.currentViewType = ko.observable('month');
        self.events = ko.observableArray();

        self.activate = function (context) {
            prepare(context);
            getAllEvents();

            return true;
        }

        self.showModalEvent = function () {
            app.showModal(new EventoModal());
        }

        self.dateChanged = function (e, args) {
            var referenceDate = args.selectedDates[0];
            self.currentDate(referenceDate);
            router.replaceLocation('/#/' + self.currentDate().getFullYear() + '/' + (self.currentDate().getMonth() + 1) + '/' + self.currentDate().getDate() + '/' + getViewType());
        }

        return self;

        function prepare(context) {
            if (context.splat) {
                params = context.splat[0].split("/");

                if (params[0] && params[1])
                    setDate(params[0], params[1] - 1, params[2]);

                if (params[3])
                    setViewType(params[3]);
            }
        }

        function setDate(year, month, day) {
            if (day == undefined)
                day = new Date().getDate();
            
            currentDate(new Date(year, month, day));
        }

        function setViewType(type) {
            if (type == 'semana')
                self.currentViewType('week');
            else if (type == 'dia')
                self.currentViewType('day');
            else
                self.currentViewType('month');
        }

        function getViewType() {
            if (self.currentViewType() == 'week')
                return 'semana';
            else if (self.currentViewType() == 'day')
                return 'dia';
            else
                return 'mes';
        }

        function getAllEvents() {
            dataservice.eventos.getAll(currentDate())
                .then(queryEventsSucceeded)
                .fail(queryEventsFailed);
        }

        function queryEventsSucceeded(data) {
            self.events(data.results);
            //if (data && data.results.length > 0) {
            //    for (var i = 0; i < data.results.length; i++) {
            //        //console.log(data.results[i]);
            //        self.events.push(data.results[i]);
            //        //self.events.push({
            //        //    id: "event1",
            //        //    subject: "Green event." + i,
            //        //    start: new Date(2013, 4, 18, 9),
            //        //    end: new Date(2013, 4, 18, 11),
            //        //    allday: true,
            //        //    description: "The green event.",
            //        //    color: "green"
            //        //});
            //    }
            //}
            
        }

        function queryEventsFailed(error) {
            logger.logError(error.message, "Erro ao obter eventos");
        }
    });