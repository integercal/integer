define(['breeze', 'services/logger', 'config', 'services/jsonResultsAdapter', 'models/m.evento'],
    function (breeze, logger, config, jsonResultsAdapter, eventoModel) {
        
        var self = this;

        var serviceName = config.calendarioEndPoint;
        var askForMetadata = false;

        var dataService = new breeze.DataService({
            serviceName: serviceName,
            hasServerMetadata: askForMetadata,
            jsonResultsAdapter: jsonResultsAdapter
        });
        self.manager = new breeze.EntityManager({ dataService: dataService });

        eventoModel.configFor(manager.metadataStore);
        
        self.getAll = function () {
            var query = new breeze.EntityQuery("Eventos");
            return manager.executeQuery(query);
        }

        return self;
    });