define(['breeze', 'services/logger', 'config'],
    function (breeze, logger, config) {

        return new breeze.JsonResultsAdapter({
            name: "integer",

            extractResults: function (data) {
                var results = data.results;
                if (!results) throw new Error("Unable to resolve 'results' property");

                return results;
            },

            visitNode: function (node, queryContext, nodeContext) {
                // Evento parser
                if (node && node.Id && node.Subject) {
                    var entityTypeName = 'Evento';
                    var entityType = entityTypeName && queryContext.entityManager.metadataStore.getEntityType(entityTypeName, true);
                    var propertyName = nodeContext.propertyName;
                    var ignore = propertyName && propertyName.substr(0, 1) === "$";

                    return {
                        entityType: entityType,
                        //nodeId: node.id,
                        ignore: ignore
                    };
                }
            }

        });
    });
    