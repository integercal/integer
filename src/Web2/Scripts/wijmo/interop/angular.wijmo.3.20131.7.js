/*
 *
 * Wijmo Library 3.20131.7
 * http://wijmo.com/
 *
 * Copyright(c) GrapeCity, Inc.  All rights reserved.
 * 
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * licensing@wijmo.com
 * http://wijmo.com/widgets/license/
 *
 */
var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var wijmo;
(function (wijmo) {
    (function (ng) {
        // declare the Node variable explicitly because IE8 and older versions do not expose it.
        var Node;
        (function (Node) {
            Node._map = [];
            Node.ELEMENT_NODE = 1;
            Node.ATTRIBUTE_NODE = 2;
            Node.TEXT_NODE = 3;
            Node.CDATA_SECTION_NODE = 4;
            Node.ENTITY_REFERENCE_NODE = 5;
            Node.ENTITY_NODE = 6;
            Node.PROCESSING_INSTRUCTION_NODE = 7;
            Node.COMMENT_NODE = 8;
            Node.DOCUMENT_NODE = 9;
            Node.DOCUMENT_TYPE_NODE = 10;
            Node.DOCUMENT_FRAGMENT_NODE = 11;
            Node.NOTATION_NODE = 12;
        })(Node || (Node = {}));
        function getTypeDefFromExample(value) {
            if(value == null) {
                return {
                };
            }
            var meta = {
                type: angular.isArray(value) ? "array" : typeof value
            };
            switch(meta.type) {
                case "object":
                    meta.properties = {
                    };
                    if(value) {
                        $.each(value, function (key, propValue) {
                            meta.properties[key] = getTypeDefFromExample(propValue);
                        });
                    }
                    break;
                case "array":
                    meta.elementType = getTypeDefFromExample(value[0]);
                    break;
            }
            return meta;
        }
        var propPath;
        (function (propPath) {
            function partition(path) {
                if(typeof path !== "string") {
                    return path;
                }
                var parts = path.split(/[\.\[\]]/g);
                for(var i = parts.length - 1; i >= 0; i--) {
                    if(!parts[i]) {
                        parts.splice(i, 1);
                    }
                }
                return parts;
            }
            propPath.partition = partition;
            function get(obj, path) {
                var parts = partition(path);
                for(var i = 0; obj && i < parts.length; i++) {
                    obj = obj[parts[i]];
                }
                return obj;
            }
            propPath.get = get;
            function set(obj, path, value) {
                var parts = partition(path);
                var last = parts.pop();
                obj = get(obj, parts);
                if(obj) {
                    obj[last] = value;
                }
            }
            propPath.set = set;
        })(propPath || (propPath = {}));
        function derive(proto, newProperties) {
            var derived;
            if(Object.create) {
                try  {
                    derived = Object.create(proto);
                } catch (err) {
                }
            }
            if(!derived) {
                function Clazz() {
                }
                Clazz.prototype = proto;
                derived = new Clazz();
            }
            if(newProperties) {
                $.extend(derived, newProperties);
            }
            return derived;
        }
        function safeApply(scope, data) {
            var phase = scope.$root.$$phase;
            if(phase !== '$apply' && phase !== '$digest') {
                scope.$apply(data);
            }
        }
        var Markup = (function () {
            function Markup(node, typeDef, selector, services) {
                this.selector = selector;
                this.services = services;
                this.innerMarkupTemplate = "<div/>";
                this.bindings = [];
                this.subElements = [];
                this.parseOptions($(node), typeDef);
            }
            Markup.prototype.moveContents = function (from, to) {
                function moveAttr(name) {
                    var value = from.attr(name);
                    if(value) {
                        to.attr(name, value);
                        from.removeAttr(name);
                    }
                }
                from.children().each(function (_, child) {
                    return to.append(child);
                });
                if(!to.children().length) {
                    to.text(from.text());
                }
                moveAttr("id");
                moveAttr("style");
                moveAttr("class");
                return to;
            };
            Markup.prototype.prepareSubelement = function (element) {
                var clone = element.clone();
                var converted = this.moveContents(element, $(this.innerMarkupTemplate));
                return {
                    element: clone,
                    link: this.services.$compile(converted)
                };
            };
            Markup.prototype.extractSubelements = function (element) {
                if(!this.selector) {
                    return;
                }
                var e = $(element);
                if(e.is(this.selector)) {
                    this.subElements.push(this.prepareSubelement(e));
                    e.empty();
                }
            };
            Markup.prototype.apply = function (scope, parentElement) {
                angular.forEach(this.subElements, function (se) {
                    se.link(scope.$parent.$new(false), function (clonedElement) {
                        return parentElement.append(clonedElement);
                    });
                });
            };
            Markup.prototype.getNameMap = // get camelCase name -> lowercase property name mapping
            function (obj) {
                var map = {
                }, key;
                for(key in obj) {
                    map[key.toLowerCase()] = key;
                }
                return map;
            };
            Markup.prototype.parseOptions = function ($node, typeDef) {
                this.options = this.parse($node, typeDef, "");
            };
            Markup.prototype.parse = function ($node, typeDef, path) {
                var _this = this;
                var readNode = function (node) {
                    var $node = $(node), value, match, name, propPath;
                    switch(node.nodeType) {
                        case Node.ATTRIBUTE_NODE:
                            value = $node.val();
                            break;
                        case Node.ELEMENT_NODE:
                            value = $node.text();
                            break;
                        default:
                            return;
                    }
                    // restore the original property name casing if possible
                    name = node.nodeName || node.name;
                    name = name.toLowerCase();
                    if(map[name]) {
                        name = map[name];
                    } else if(name.match(/-/)) {
                        var parts = name.split("-");
                        name = parts.shift();
                        angular.forEach(parts, function (p) {
                            return name += p.charAt(0).toUpperCase() + p.substring(1);
                        });
                    }
                    match = value && /^{{(.+)}}$/.exec(value);
                    if(match) {
                        toRemove.push(node);
                        _this.bindings.push({
                            path: (path && path + ".") + name,
                            expression: match[1]
                        });
                        return;
                    }
                    if(node.nodeType === Node.ELEMENT_NODE && array) {
                        // then push the sub-element
                        array.push(_this.parse($node, typeDef && typeDef.elementType, path + "[" + array.length + "]"));
                    } else {
                        obj[name] = _this.parse($node, properties && properties[name], (path && path + ".") + name);
                    }
                };
                var node = $node[0], text = node.nodeType === Node.TEXT_NODE ? (node).wholeText : (node).value, isArray = typeDef && typeDef.type === "array", properties = typeDef && typeDef.properties, map = // we need this lowercase name map because HTML IS NOT CASE-SENSITIVE! Chris said that.
                properties && this.getNameMap(properties) || {
                }, toRemove = [], obj, array, primitiveTypeRequested;
                if(node.nodeType === Node.ELEMENT_NODE) {
                    this.extractSubelements($node);
                }
                // if the type is a number or boolean, then parse it.
                // if it is not an object or the node is not an element, return the text
                switch(typeDef && typeDef.type) {
                    case "boolean":
                        return text.toLowerCase() === "true";
                    case "number":
                        return parseFloat(text);
                    default:
                        primitiveTypeRequested = typeDef && typeDef.type && typeDef.type !== "object" && typeDef.type !== "array";
                        if(primitiveTypeRequested || node.nodeType !== Node.ELEMENT_NODE) {
                            return text;
                        }
                }
                // parse a DOM element to an object/array
                if(isArray) {
                    array = [];
                } else {
                    obj = {
                    };
                }
                // read attributes
                angular.forEach(node.attributes, readNode);
                angular.forEach(node.childNodes, readNode);
                $.each(toRemove, function (_, node) {
                    if(node.nodeType === Node.ATTRIBUTE_NODE) {
                        $(node.ownerElement).removeAttr(node.name);
                    } else {
                        $(node).remove();
                    }
                });
                return obj || array;
            };
            return Markup;
        })();        
        var definitions;
        (function (definitions) {
            var DirectiveBase = (function () {
                function DirectiveBase(widgetName, namespace, clazz, services) {
                    this.widgetName = widgetName;
                    this.namespace = namespace;
                    this.services = services;
                    this.expectedTemplate = "<div/>";
                    this.restrict = 'E';
                    // require mapping to a DOM element
                    // create a new scope (not sharing)
                    this.scope = {
                    };
                    this.internalEventPrefix = "wijmo-angular";
                    this.innerMarkupSelector = null;
                    this.replace = true;
                    this.scopeWatchers = {
                    };
                    this.fullWidgetName = namespace + widgetName.charAt(0).toUpperCase() + widgetName.substr(1);
                    this.wijMetadata = DirectiveBase.mergeMetadata(widgetName, clazz.prototype.options);
                    this.eventPrefix = clazz.prototype.widgetEventPrefix || widgetName;
                    this.registerEvents();
                }
                DirectiveBase.mergeMetadata = function mergeMetadata(widgetName, options) {
                    var fromOptions = {
                        properties: getTypeDefFromExample(options).properties
                    }, result = $.extend({
                    }, fromOptions, widgetMetadata["base"]), inheritanceStack = [], parentName = widgetName;
                    do {
                        inheritanceStack.unshift(parentName);
                        parentName = widgetMetadata[parentName] && widgetMetadata[parentName].inherits;
                    }while(parentName);
                    angular.forEach(inheritanceStack, function (name) {
                        return $.extend(true, result, widgetMetadata[name]);
                    });
                    return result;
                };
                DirectiveBase.prototype.bindToWidget = function (name, handler) {
                    var fullName = this.eventPrefix + name.toLowerCase() + "." + this.internalEventPrefix;
                    this.element.bind(fullName, handler);
                };
                DirectiveBase.prototype.registerEvents = function () {
                    var _this = this;
                    // TODO: optimize this. No need to watch for all events if handlers are not specified
                    if(!this.wijMetadata.events) {
                        return;
                    }
                    $.each(this.wijMetadata.events, function (name) {
                        _this.scope[name] = "=" + name.toLowerCase();
                        _this.scopeWatchers[name] = function (handler) {
                            this.bindToWidget(name, handler);
                        };
                    });
                };
                DirectiveBase.prototype.createMarkup = function (elem, typeDef) {
                    return new Markup(elem[0], typeDef, this.innerMarkupSelector, this.services);
                };
                DirectiveBase.prototype.parseMarkup = function (elem) {
                    var markup = this.createMarkup(elem, {
                        type: "object",
                        properties: this.wijMetadata.properties
                    });
                    markup.options.dataSource = [];
                    return markup;
                };
                DirectiveBase.prototype.compile = function (tElem, tAttrs, $compile) {
                    var newThis = derive(this, {
                        markup: this.parseMarkup(tElem)
                    });
                    return $.proxy(newThis.link, newThis);
                };
                DirectiveBase.prototype.createInstanceCore = function (scope, newElem, options) {
                    return newElem[this.widgetName](options);
                };
                DirectiveBase.prototype.createInstance = function (scope, elem, attrs) {
                    // create a widget instance
                    var newElem = $(this.expectedTemplate).replaceAll(elem);
                    this.markup.apply(scope, newElem);
                    // move style and class to the new element
                    newElem.attr({
                        style: attrs.style,
                        id: attrs.id,
                        "class": attrs["class"]
                    });
                    return this.createInstanceCore(scope, newElem, this.markup.options);
                };
                DirectiveBase.prototype.link = function (scope, elem, attrs) {
                    var newThis = derive(this, {
                        $scope: scope,
                        element: this.createInstance(scope, elem, attrs)
                    });
                    newThis.widget = newThis.element.data(this.widgetName) || newThis.element.data(this.fullWidgetName);
                    newThis.wireData();
                };
                DirectiveBase.prototype.wireData = function () {
                    var _this = this;
                    var parentScope = this.$scope.$parent, applyingOptions = {
                    };
                    // setup directive scope watches
                    $.each(this.scopeWatchers, function (name, handler) {
                        return _this.$scope.$watch(name, $.proxy(handler, _this), true);
                    });
                    // establish two-way data binding between widget options and a view model (parent scope)
                    $.each(this.markup.bindings, function (_, binding) {
                        // listen to changes in the view model
                        var didAssign = false;
                        parentScope.$watch(binding.expression, function (value) {
                            if(applyingOptions[binding.path] && _this.widget.option(binding.path) === value) {
                                return;
                            }
                            _this.setOption(binding.path, value);
                            didAssign = true;
                        }, true);
                        if(!didAssign) {
                            _this.setOption(binding.path, parentScope.$eval(binding.expression));
                        }
                        // listen to changes in the widget options
                        var meta = _this.wijMetadata.properties[binding.path];
                        var changeEvent = meta && meta.changeEvent;
                        if(!changeEvent) {
                            changeEvent = binding.path + "Changed";
                            if(!(changeEvent in _this.widget.options)) {
                                changeEvent = null;
                            }
                        }
                        if(changeEvent) {
                            _this.bindToWidget(changeEvent, function () {
                                applyingOptions[binding.path] = true;
                                try  {
                                    propPath.set(parentScope, binding.expression, _this.widget.option(binding.path));
                                    safeApply(parentScope, binding.expression);
                                }finally {
                                    applyingOptions[binding.path] = false;
                                }
                            });
                        }
                    });
                };
                DirectiveBase.prototype.setOption = function (path, value) {
                    var parts = propPath.partition(path);
                    if(parts.length == 1) {
                        this.widget.option(path, value);
                    } else {
                        var optionName = parts.shift();
                        var optionValue = this.widget.option(optionName);
                        propPath.set(optionValue, parts, value);
                        this.widget.option(optionName, optionValue);
                    }
                };
                return DirectiveBase;
            })();
            definitions.DirectiveBase = DirectiveBase;            
            var GridMarkup = (function (_super) {
                __extends(GridMarkup, _super);
                function GridMarkup() {
                    _super.apply(this, arguments);

                }
                GridMarkup.prototype.extactCellTemplate = function ($col, name) {
                    var templateContainer = $col.children(name);
                    if(templateContainer.length === 0) {
                        return null;
                    }
                    var template = templateContainer.children().clone();
                    if(template.length === 0) {
                        return null;
                    }
                    templateContainer.remove();
                    return {
                        element: template,
                        link: this.services.$compile(template)
                    };
                };
                GridMarkup.prototype.extractCellTemplates = function (node) {
                    var _this = this;
                    this.cellTemplates = this.cellTemplates || [];
                    $(node).children("columns").children().each(function (index, col) {
                        var $col = $(col);
                        var cellTemplate = _this.extactCellTemplate($col, "cellTemplate");
                        var editorTemplate = _this.extactCellTemplate($col, "editorTemplate");
                        if(cellTemplate || editorTemplate) {
                            _this.cellTemplates[index] = {
                                view: cellTemplate,
                                edit: editorTemplate
                            };
                        }
                    });
                };
                GridMarkup.prototype.parseOptions = function ($node, typeDef) {
                    this.extractCellTemplates($node);
                    _super.prototype.parseOptions.call(this, $node, typeDef);
                    this.options.data = [];
                };
                return GridMarkup;
            })(Markup);            
            var wijgrid = (function (_super) {
                __extends(wijgrid, _super);
                function wijgrid() {
                    _super.apply(this, arguments);

                    this.expectedTemplate = "<table/>";
                }
                wijgrid.prototype.createMarkup = function (elem, typeDef) {
                    return new GridMarkup(elem[0], typeDef, this.innerMarkupSelector, this.services);
                };
                wijgrid.prototype.dataOptionExression = function () {
                    var expr = null;
                    $.each(this.markup.bindings, function (_, b) {
                        if(b.path === "data") {
                            expr = b.expression;
                            return false;
                        }
                    });
                    return expr;
                };
                wijgrid.prototype.applyCellTemplates = function (scope, options) {
                    var _this = this;
                    function applyCellTemplate(index, container, template) {
                        if(index < 0) {
                            return false;
                        }
                        var items = scope.$parent.$eval(dataExpr);
                        if(!items) {
                            return false;
                        }
                        var rowScope = scope.$new();
                        rowScope.rowData = items[index];
                        template.link(rowScope, function (el) {
                            container.empty().append(el);
                        });
                        container.children().data(ngKey, true);
                        return true;
                    }
                    var columns = options.columns, dataExpr = this.dataOptionExression(), ngKey = "wijmoNg";
                    if(!dataExpr) {
                        return;
                    }
                    var hasEditTemplates = false;
                    $.each(this.markup.cellTemplates, function (index, template) {
                        if(!template) {
                            return;
                        }
                        var column = (columns[index] = columns[index] || {
                        });
                        if(template.view) {
                            var origFormatter = column.cellFormatter;
                            column.cellFormatter = function (args) {
                                return $.isFunction(origFormatter) && origFormatter(args) || applyCellTemplate(args.row.dataItemIndex, args.$container, template.view);
                            };
                        }
                        if(template.edit) {
                            hasEditTemplates = true;
                        }
                    });
                    if(hasEditTemplates) {
                        var origBeforeCellEdit = options.beforeCellEdit;
                        var origAfterCellEdit = options.afterCellEdit;
                        options.beforeCellEdit = function (e, args) {
                            if($.isFunction(origBeforeCellEdit)) {
                                origBeforeCellEdit(args);
                                if(args.handled) {
                                    return;
                                }
                            }
                            var col = args.cell.column();
                            if(!col || col.dataIndex < 0 || col.dataIndex >= _this.markup.cellTemplates.length) {
                                return;
                            }
                            var row = args.cell.row();
                            if(!row || row.dataItemIndex < 0) {
                                return;
                            }
                            var container = args.cell.container();
                            if(!container || container.length == 0) {
                                return;
                            }
                            var template = _this.markup.cellTemplates[col.dataIndex];
                            if(!template) {
                                return;
                            }
                            if(applyCellTemplate(row.dataItemIndex, container, template.edit)) {
                                args.handled = true;
                            }
                        };
                        options.afterCellEdit = function (e, args) {
                            if($.isFunction(origAfterCellEdit)) {
                                origAfterCellEdit(args);
                                if(args.handled) {
                                    return;
                                }
                            }
                            var container = args.cell.container();
                            if(container && container.children().data(ngKey)) {
                                container.empty();
                            }
                        };
                    }
                };
                wijgrid.prototype.createInstanceCore = function (scope, newElem, options) {
                    // apply column templates
                    options = $.extend(true, {
                    }, options);
                    this.applyCellTemplates(scope, options);
                    var grid = _super.prototype.createInstanceCore.call(this, scope, newElem, options);
                    return grid;
                };
                return wijgrid;
            })(DirectiveBase);
            definitions.wijgrid = wijgrid;            
            var wijinputcore = (function (_super) {
                __extends(wijinputcore, _super);
                function wijinputcore() {
                    _super.apply(this, arguments);

                    this.expectedTemplate = "<input/>";
                }
                return wijinputcore;
            })(DirectiveBase);
            definitions.wijinputcore = wijinputcore;            
            var wijinputdate = (function (_super) {
                __extends(wijinputdate, _super);
                function wijinputdate() {
                    _super.apply(this, arguments);

                }
                return wijinputdate;
            })(wijinputcore);
            definitions.wijinputdate = wijinputdate;            
            var wijinputmask = (function (_super) {
                __extends(wijinputmask, _super);
                function wijinputmask() {
                    _super.apply(this, arguments);

                }
                return wijinputmask;
            })(wijinputcore);
            definitions.wijinputmask = wijinputmask;            
            var wijinputnumber = (function (_super) {
                __extends(wijinputnumber, _super);
                function wijinputnumber() {
                    _super.apply(this, arguments);

                }
                return wijinputnumber;
            })(wijinputcore);
            definitions.wijinputnumber = wijinputnumber;            
            var wijcheckbox = (function (_super) {
                __extends(wijcheckbox, _super);
                function wijcheckbox() {
                    _super.apply(this, arguments);

                    this.expectedTemplate = "<input type='checkbox'/>";
                }
                return wijcheckbox;
            })(DirectiveBase);
            definitions.wijcheckbox = wijcheckbox;            
            var wijsplitter = (function (_super) {
                __extends(wijsplitter, _super);
                function wijsplitter() {
                    _super.apply(this, arguments);

                    this.innerMarkupSelector = "panel1, panel2";
                }
                return wijsplitter;
            })(DirectiveBase);
            definitions.wijsplitter = wijsplitter;            
            var wijexpander = (function (_super) {
                __extends(wijexpander, _super);
                function wijexpander() {
                    _super.apply(this, arguments);

                    this.innerMarkupSelector = "h1, div";
                }
                return wijexpander;
            })(DirectiveBase);
            definitions.wijexpander = wijexpander;            
            var TabsMarkup = (function (_super) {
                __extends(TabsMarkup, _super);
                function TabsMarkup(node, typeDef, services) {
                                _super.call(this, node, typeDef, "tab", services);
                    this.services = services;
                }
                TabsMarkup.prototype.apply = function (scope, parentElement) {
                    _super.prototype.apply.call(this, scope, parentElement);
                    var ul = $("<ul/>");
                    angular.forEach(this.subElements, function (se) {
                        var id = se.element.attr("id"), anchor = $("<a/>").text(se.element.attr("title"));
                        if(id) {
                            anchor.attr("href", "#" + id);
                        }
                        $("<li/>").append(anchor).appendTo(ul);
                    });
                    ul.prependTo(parentElement);
                };
                return TabsMarkup;
            })(Markup);            
            var wijtabs = (function (_super) {
                __extends(wijtabs, _super);
                function wijtabs() {
                    _super.apply(this, arguments);

                }
                wijtabs.prototype.createMarkup = function (element, typeDef) {
                    return new TabsMarkup(element, typeDef, this.services);
                };
                return wijtabs;
            })(DirectiveBase);
            definitions.wijtabs = wijtabs;            
            var gcSpread = (function (_super) {
                __extends(gcSpread, _super);
                function gcSpread() {
                    _super.apply(this, arguments);

                }
                gcSpread.prototype.setOption = function (path, value) {
                    if(path === "dataSource") {
                        this.widget.sheets[0].setDataSource(value);
                    } else {
                        _super.prototype.setOption.call(this, path, value);
                    }
                };
                return gcSpread;
            })(DirectiveBase);
            definitions.gcSpread = gcSpread;            
            function findDirectiveClass(widgetName) {
                var metadata = widgetMetadata[widgetName], parentMetadata;
                return definitions[widgetName] || metadata && metadata.inherits && findDirectiveClass(metadata.inherits);
            }
            definitions.findDirectiveClass = findDirectiveClass;
        })(definitions || (definitions = {}));
        // define the wijmo module
        var wijModule = angular["module"]('wijmo', []);
        function registerDirectiveIfNeeded(widgetName, namespace, clazz, directiveName) {
            var directiveClass = definitions.findDirectiveClass(widgetName) || definitions.DirectiveBase;
            var meta = widgetMetadata[widgetName];
            if(meta && meta.isDecorator) {
                return;
            }
            wijModule.directive(directiveName || widgetName.toLowerCase(), [
                "$compile", 
                function ($compile) {
                    return new directiveClass(widgetName, namespace, clazz, {
                        $compile: $compile
                    });
                }            ]);
        }
        var widgetMetadata = {
            "base": {
                events: {
                    "create": {
                    },
                    "change": {
                    }
                }
            },
            "wijtooltip": {
                "events": {
                    "showing": {
                    },
                    "shown": {
                    },
                    "hiding": {
                    },
                    "hidden": {
                    }
                },
                "properties": {
                    "group": {
                    },
                    "ajaxCallback": {
                    }
                }
            },
            "wijslider": {
                "events": {
                    "buttonMouseOver": {
                    },
                    "buttonMouseOut": {
                    },
                    "buttonMouseDown": {
                    },
                    "buttonMouseUp": {
                    },
                    "buttonClick": {
                    },
                    "start": {
                    },
                    "stop": {
                    }
                },
                "properties": {
                    "value": {
                        changeEvent: "change"
                    },
                    "values": {
                    }
                }
            },
            "wijsplitter": {
                "events": {
                    "sized": {
                    },
                    "load": {
                    },
                    "sizing": {
                    }
                },
                "properties": {
                    "expand": {
                    },
                    "collapse": {
                    },
                    "expanded": {
                    },
                    "collapsed": {
                    }
                }
            },
            "wijprogressbar": {
                "properties": {
                    "progressChanging": {
                    },
                    "beforeProgressChanging": {
                    },
                    "progressChanged": {
                    }
                }
            },
            "wijdialog": {
                "events": {
                    "blur": {
                    },
                    "buttonCreating": {
                    },
                    "resize": {
                    },
                    "stateChanged": {
                    },
                    "focus": {
                    },
                    "resizeStart": {
                    },
                    "resizeStop": {
                    }
                },
                "properties": {
                    "hide": {
                    },
                    "show": {
                    },
                    "collapsingAnimation": {
                    },
                    "expandingAnimation": {
                    }
                }
            },
            "wijaccordion": {
                "events": {
                    "beforeSelectedIndexChanged": {
                    },
                    "selectedIndexChanged": {
                    }
                },
                "properties": {
                    "duration": {
                    }
                }
            },
            "wijpopup": {
                "events": {
                    "showing": {
                    },
                    "shown": {
                    },
                    "hiding": {
                    },
                    "hidden": {
                    },
                    "posChanged": {
                    }
                }
            },
            "wijsuperpanel": {
                "events": {
                    "dragStop": {
                    },
                    "painted": {
                    },
                    "scroll": {
                    },
                    "scrolling": {
                    },
                    "scrolled": {
                    },
                    "resized": {
                    }
                },
                "properties": {
                    "hScrollerActivating": {
                    },
                    "vScrollerActivating": {
                    }
                }
            },
            "wijcheckbox": {
                isDecorator: true,
                "properties": {
                    "checked": {
                        type: "bool",
                        changeEvent: "changed"
                    }
                }
            },
            "wijradio": {
                isDecorator: true,
                "properties": {
                    "checked": {
                    }
                }
            },
            "wijlist": {
                "events": {
                    "focusing": {
                    },
                    "focus": {
                    },
                    "blur": {
                    },
                    "selected": {
                    },
                    "listRendered": {
                    },
                    "itemRendering": {
                    },
                    "itemRendered": {
                    }
                },
                "properties": {
                    "superPanelOptions": {
                    }
                }
            },
            "wijcalendar": {
                "events": {
                    "beforeSlide": {
                    },
                    "beforeSelect": {
                    },
                    "selectedDatesChanged": {
                    },
                    "afterSelect": {
                    },
                    "afterSlide": {
                    }
                },
                "properties": {
                    "customizeDate": {
                    },
                    "title": {
                    },
                    selectedDates: {
                        type: "array",
                        elementType: "date",
                        changeEvent: "selecteddateschanged"
                    }
                }
            },
            wijdropdown: {
                isDecorator: true
            },
            "wijexpander": {
                "events": {
                    "beforeCollapse": {
                    },
                    "afterCollapse": {
                    },
                    "beforeExpand": {
                    },
                    "afterExpand": {
                    }
                }
            },
            "wijmenu": {
                "events": {
                    "focus": {
                    },
                    "blur": {
                    },
                    "select": {
                    },
                    "showing": {
                    },
                    "shown": {
                    },
                    "hidding": {
                    },
                    "hidden": {
                    }
                },
                "properties": {
                    "superPanelOptions": {
                    }
                }
            },
            "wijmenuitem": {
                "events": {
                    "hidding": {
                    },
                    "hidden": {
                    },
                    "showing": {
                    },
                    "shown": {
                    }
                }
            },
            "wijtabs": {
                "properties": {
                    "ajaxOptions": {
                    },
                    "cookie": {
                    },
                    "hideOption": {
                    },
                    "showOption": {
                    },
                    "add": {
                    },
                    "remove": {
                    },
                    "select": {
                    },
                    "beforeShow": {
                    },
                    "show": {
                    },
                    "load": {
                    },
                    "disable": {
                    },
                    "enable": {
                    }
                }
            },
            wijtextbox: {
                isDecorator: true
            },
            "wijpager": {
                "events": {
                    "pageIndexChanging": {
                    },
                    "pageIndexChanged": {
                    }
                }
            },
            "wijcombobox": {
                "events": {
                    "select": {
                    },
                    "search": {
                    },
                    "open": {
                    },
                    "close": {
                    }
                },
                "properties": {
                    "data": {
                    },
                    "labelText": {
                    },
                    "showingAnimation": {
                    },
                    "hidingAnimation": {
                    },
                    "selectedValue": {
                    },
                    "text": {
                    },
                    "listOptions": {
                    }
                }
            },
            "wijinputcore": {
                "events": {
                    "initializing": {
                    },
                    "initialized": {
                    },
                    "triggerMouseDown": {
                    },
                    "triggerMouseUp": {
                    },
                    "initialized": {
                    },
                    "textChanged": {
                    },
                    "invalidInput": {
                    }
                }
            },
            "wijinputdate": {
                inherits: "wijinputcore",
                "events": {
                    "dateChanged": {
                    }
                },
                "properties": {
                    "date": {
                    },
                    "minDate": {
                    },
                    "maxDate": {
                    }
                }
            },
            "wijinputmask": {
                inherits: "wijinputcore",
                "properties": {
                    "text": {
                        type: "string"
                    }
                }
            },
            "wijinputnumber": {
                inherits: "wijinputcore",
                "events": {
                    "valueChanged": {
                    },
                    "valueBoundsExceeded": {
                    }
                },
                "properties": {
                    "value": {
                    }
                }
            },
            "wijgrid": {
                "properties": {
                    data: {
                        changeEvent: "afterCellEdit"
                    },
                    "columns": {
                        type: "array",
                        elementType: {
                            type: "object",
                            properties: {
                                readOnly: {
                                    type: "bool"
                                },
                                "dataKey": {
                                    type: "string"
                                },
                                "dataType": {
                                    type: "string"
                                },
                                "headerText": {
                                    type: "string"
                                }
                            }
                        }
                    }
                },
                "events": {
                    "ajaxError": {
                    },
                    "dataLoading": {
                    },
                    "dataLoaded": {
                    },
                    "loading": {
                    },
                    "loaded": {
                    },
                    "columnDropping": {
                    },
                    "columnDropped": {
                    },
                    "columnGrouping": {
                    },
                    "columnGrouped": {
                    },
                    "columnUngrouping": {
                    },
                    "columnUngrouped": {
                    },
                    "filtering": {
                    },
                    "filtered": {
                    },
                    "sorting": {
                    },
                    "sorted": {
                    },
                    "currentCellChanged": {
                    },
                    "pageIndexChanging": {
                    },
                    "pageIndexChanged": {
                    },
                    "rendering": {
                    },
                    "rendered": {
                    },
                    "columnResizing": {
                    },
                    "columnResized": {
                    },
                    "currentCellChanging": {
                    },
                    "afterCellEdit": {
                    },
                    "afterCellUpdate": {
                    },
                    "beforeCellEdit": {
                    },
                    "beforeCellUpdate": {
                    },
                    "columnDragging": {
                    },
                    "columnDragged": {
                    },
                    "filterOperatorsListShowing": {
                    },
                    "groupAggregate": {
                    },
                    "groupText": {
                    },
                    "invalidCellValue": {
                    },
                    "selectionChanged": {
                    }
                }
            },
            "wijchartcore": {
                "events": {
                    "beforeSeriesChange": {
                    },
                    "afterSeriesChange": {
                    },
                    "seriesChanged": {
                    },
                    "beforePaint": {
                    },
                    "painted": {
                    },
                    "mouseDown": {
                    },
                    "mouseUp": {
                    },
                    "mouseOver": {
                    },
                    "mouseOut": {
                    },
                    "mouseMove": {
                    },
                    "click": {
                    }
                },
                "properties": {
                    "width": {
                        type: "number"
                    },
                    "height": {
                        type: "number"
                    }
                }
            },
            "wijcompositechart": {
                inherits: "wijchartcore"
            },
            "wijbarchart": {
                inherits: "wijchartcore"
            },
            "wijlinechart": {
                inherits: "wijchartcore",
                "properties": {
                    "hole": {
                    }
                }
            },
            "wijscatterchart": {
                inherits: "wijchartcore"
            },
            "wijbubblechart": {
                inherits: "wijchartcore"
            },
            "wijpiechart": {
                inherits: "wijchartcore",
                "properties": {
                    "radius": {
                        type: "number"
                    }
                }
            },
            "wijtree": {
                "events": {
                    "nodeBeforeDropped": {
                    },
                    "nodeDropped": {
                    },
                    "nodeBlur": {
                    },
                    "nodeFocus": {
                    },
                    "nodeClick": {
                    },
                    "nodeCheckChanged": {
                    },
                    "nodeCollapsed": {
                    },
                    "nodeExpanded": {
                    },
                    "nodeDragging": {
                    },
                    "nodeDragStarted": {
                    },
                    "nodeMouseOver": {
                    },
                    "nodeMouseOut": {
                    },
                    "nodeTextChanged": {
                    },
                    "selectedNodeChanged": {
                    },
                    "nodeExpanding": {
                    },
                    "nodeCollapsing": {
                    }
                }
            },
            "wijtreenode": {
                "events": {
                    "nodeTextChanged": {
                    },
                    "nodeDragStarted": {
                    },
                    "nodeDragging": {
                    },
                    "nodeCheckChanged": {
                    },
                    "nodeFocus": {
                    },
                    "nodeBlur": {
                    },
                    "nodeClick": {
                    },
                    "selectedNodeChanged": {
                    },
                    "nodeMouseOver": {
                    },
                    "nodeMouseOut": {
                    }
                }
            },
            "wijupload": {
                "events": {
                    "cancel": {
                    },
                    "totalComplete": {
                    },
                    "progress": {
                    },
                    "complete": {
                    },
                    "totalProgress": {
                    },
                    "upload": {
                    },
                    "totalUpload": {
                    }
                }
            },
            "wijwizard": {
                "events": {
                    "show": {
                    },
                    "add": {
                    },
                    "remove": {
                    },
                    "activeIndexChanged": {
                    },
                    "validating": {
                    },
                    "load": {
                    }
                },
                "properties": {
                    "ajaxOptions": {
                    },
                    "cookie": {
                    }
                }
            },
            "wijribbon": {
                "events": {
                    "click": {
                    }
                }
            },
            "wijeditor": {
                "events": {
                    "commandButtonClick": {
                    },
                    "textChanged": {
                    }
                },
                "properties": {
                    "simpleModeCommands": {
                    },
                    "text": {
                    },
                    "localization": {
                    }
                }
            },
            "wijrating": {
                "events": {
                    "hover": {
                    },
                    "rating": {
                    },
                    "rated": {
                    },
                    "reset": {
                    }
                },
                "properties": {
                    "min": {
                    },
                    "max": {
                    },
                    "animation": {
                    }
                }
            },
            "wijcarousel": {
                "events": {
                    "loadCallback": {
                    },
                    "itemClick": {
                    },
                    "beforeScroll": {
                    },
                    "afterScroll": {
                    },
                    "create": {
                    }
                }
            },
            "wijgallery": {
                "events": {
                    "loadCallback": {
                    },
                    "beforeTransition": {
                    },
                    "afterTransition": {
                    },
                    "create": {
                    }
                }
            },
            "wijgauge": {
                "properties": {
                    "ranges": {
                        type: "array",
                        elementType: {
                            type: "object",
                            properties: {
                                "startValue": {
                                    type: "number"
                                },
                                "endValue": {
                                    type: "number"
                                },
                                "startDistance": {
                                    type: "number"
                                },
                                "endDistance": {
                                    type: "number"
                                },
                                "startWidth": {
                                    type: "number"
                                },
                                "endWidth": {
                                    type: "number"
                                }
                            }
                        }
                    }
                },
                "events": {
                    "beforeValueChanged": {
                    },
                    "valueChanged": {
                    },
                    "painted": {
                    },
                    "click": {
                    },
                    "create": {
                    }
                }
            },
            "wijlineargauge": {
                inherits: "wijgauge"
            },
            "wijradialgauge": {
                inherits: "wijgauge"
            },
            "wijlightbox": {
                "events": {
                    "show": {
                    },
                    "beforeShow": {
                    },
                    "beforeClose": {
                    },
                    "close": {
                    },
                    "open": {
                    }
                },
                "properties": {
                    "cookie": {
                    }
                }
            },
            "wijdatepager": {
                "events": {
                    "selectedDateChanged": {
                    }
                },
                "properties": {
                    "localization": {
                    }
                }
            },
            "wijevcal": {
                "events": {
                    "viewTypeChanged": {
                    },
                    "selectedDatesChanged": {
                    },
                    "initialized": {
                    },
                    "beforeDeleteCalendar": {
                    },
                    "beforeAddCalendar": {
                    },
                    "beforeUpdateCalendar": {
                    },
                    "beforeAddEvent": {
                    },
                    "beforeUpdateEvent": {
                    },
                    "beforeDeleteEvent": {
                    },
                    "beforeEditEventDialogShow": {
                    },
                    "eventsDataChanged": {
                    },
                    "calendarsChanged": {
                    }
                },
                "properties": {
                    "localization": {
                    },
                    "datePagerLocalization": {
                    },
                    "colors": {
                    },
                    "selectedDate": {
                    },
                    "selectedDates": {
                    }
                }
            },
            wijvideo: {
                isDecorator: true
            },
            "gcSpread": {
                properties: {
                    dataSource: {
                        type: "array"
                    },
                    sheetCount: {
                        type: "number"
                    },
                    sheets: {
                        type: "array",
                        elementType: {
                            type: "object",
                            properties: {
                                rowCount: {
                                    type: "number"
                                },
                                colCount: {
                                    type: "number"
                                },
                                defaultRowCount: {
                                    type: "number"
                                },
                                defaultColCount: {
                                    type: "number"
                                }
                            }
                        }
                    }
                }
            }
        };
        // register directives for all widgets
        $.each($.wijmo, function (name, clazz) {
            return registerDirectiveIfNeeded(name, "wijmo", clazz);
        });
        $.each($.ui, function (name, clazz) {
            return registerDirectiveIfNeeded(name, clazz, "ui", "jqui" + name);
        });
    })(wijmo.ng || (wijmo.ng = {}));
    var ng = wijmo.ng;
})(wijmo || (wijmo = {}));
