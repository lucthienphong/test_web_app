﻿/**
* $Id: Editor.js,v 1.35 2014/01/22 09:58:22 gaudenz Exp $
* Copyright (c) 2006-2012, JGraph Ltd
*/
// Specifies if local storage should be used (eg. on the iPad which has no filesystem)
if (typeof IMAGE_PATH === 'undefined')
    IMAGE_PATH = '';
if (typeof RESOURCE_BASE === 'undefined')
    RESOURCE_BASE = '';
if (typeof STENCIL_PATH === 'undefined')
    STENCIL_PATH = '';
if (typeof RESOURCE_BASE === 'undefined')
    RESOURCE_BASE = '';
if (typeof urlParams === 'undefined') {
    // Parses URL parameters. Supported parameters are:
    // - lang=xy: Specifies the language of the user interface.
    // - touch=1: Enables a touch-style user interface.
    // - storage=local: Enables HTML5 local storage.
    var urlParams = (function (url) {
        var result = new Object();
        var idx = url.lastIndexOf('?');

        if (idx > 0) {
            var params = url.substring(idx + 1).split('&');

            for (var i = 0; i < params.length; i++) {
                idx = params[i].indexOf('=');

                if (idx > 0) {
                    result[params[i].substring(0, idx)] = params[i].substring(idx + 1);
                }
            }
        }

        return result;
    })(window.location.href);
}
var useLocalStorage = typeof (Storage) != 'undefined' && (mxClient.IS_IOS || urlParams['storage'] == 'local');
var fileSupport = window.File != null && window.FileReader != null && window.FileList != null;

// Specifies if the touch UI should be used
var touchStyle = mxClient.IS_TOUCH || navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0 || urlParams['touch'] == '1';

// Counts open editor tabs (must be global for cross-window access)
var counter = 0;

// Cross-domain window access is not allowed in FF, so if we
// were opened from another domain then this will fail. 
try {
    var op = window;

    while (op.opener != null && !isNaN(op.opener.counter)) {
        op = op.opener;
    }

    // Increments the counter in the first opener in the chain
    if (op != null) {
        op.counter++;
        counter = op.counter;
    }
}
catch (e) {
    // ignore
}

/**
* Editor constructor executed on page load.
*/
Editor = function (graphUi) {
    useLocalStorage = false;
    mxEventSource.call(this);
    this.init();
    //this.initStencilRegistry();

    if (typeof graphUi !== 'undefined')
        this.graph = graphUi;
    else if (typeof Graph !== 'undefined')
        this.graph = new Graph();


    // Uses the entity edge style as default
    this.graph.stylesheet.getDefaultEdgeStyle()[mxConstants.STYLE_EDGE] = mxEdgeStyle.SegmentConnector;
    //this.graph.stylesheet.getDefaultEdgeStyle()[mxConstants.STYLE_EDGE] = 'elbowEdgeStyle';

    this.outline = new mxOutline(this.graph);
    this.outline.updateOnPan = true;
    this.undoManager = this.createUndoManager();
    this.status = '';

    this.getOrCreateFilename = function () {
        return this.filename || mxResources.get('drawing', [counter]) + '.xml';
    };

    this.getFilename = function () {
        return this.filename;
    };

    // Sets the status and fires a statusChanged event
    this.setStatus = function (value) {
        this.status = value;
        this.fireEvent(new mxEventObject('statusChanged'));
    };

    // Returns the current status
    this.getStatus = function () {
        return this.status;
    };

    // Updates modified state if graph changes
    this.graphChangeListener = function () {
        this.setModified(true);
    };

    this.graph.getModel().addListener(mxEvent.CHANGE, mxUtils.bind(this, function () {
        this.graphChangeListener.apply(this, arguments);
    }));

    // Sets persistent graph state defaults
    this.graph.resetViewOnRootChange = false;
    this.graph.scrollbars = this.defaultScrollbars;

    //DEV:alway show scroll bar
    this.graph.scrollbars = true;

    this.graph.background = null;
};

// Editor inherits from mxEventSource
mxUtils.extend(Editor, mxEventSource);

/**
* Specifies the image URL to be used for the grid.
*/
Editor.prototype.gridImage = IMAGE_PATH + '/grid.gif';

/**
* Scrollbars are enabled on non-touch devices. Disabled on Firefox because it causes problems with touch
* events and touch feature cannot be detected.
*/
Editor.prototype.defaultScrollbars = !touchStyle && (!mxClient.IS_NS || mxClient.IS_SF || mxClient.IS_GC);

/**
* Specifies the image URL to be used for the transparent background.
*/
Editor.prototype.transparentImage = IMAGE_PATH + '/transparent.gif';

/**
* Specifies if the editor is enabled. Default is true.
*/
Editor.prototype.enabled = true;

/**
* Contains the name which was used for the last save. Default value is null.
*/
Editor.prototype.filename = null;

/**
* Contains the current modified state of the diagram. This is false for
* new diagrams and after the diagram was saved.
*/
Editor.prototype.modified = false;

/**
* Specifies if the diagram should be saved automatically if possible. Default
* is true.
*/
Editor.prototype.autosave = true;

/**
* Specifies the app name. Default is document.title.
*/
Editor.prototype.appName = document.title;

Editor.prototype.editorUi = undefined;

/**
* Sets the XML node for the current diagram.
*/
Editor.prototype.resetGraph = function () {
    this.graph.view.scale = 1;
    this.graph.gridEnabled = true;
    this.graph.graphHandler.guidesEnabled = true;
    this.graph.setTooltips(true);
    this.graph.setConnectable(true);
    this.graph.foldingEnabled = true;
    this.graph.scrollbars = this.defaultScrollbars;
    this.graph.pageVisible = true;
    this.graph.pageBreaksVisible = this.graph.pageVisible;
    this.graph.preferPageSize = this.graph.pageBreaksVisible;
    this.graph.background = null;
    this.graph.pageScale = mxGraph.prototype.pageScale;
    this.graph.view.setScale(1);
    this.updateGraphComponents();
};

/**
* Sets the XML node for the current diagram.
*/
Editor.prototype.setGraphXml = function (node) {
    var dec = new mxCodec(node.ownerDocument);
    if (node.nodeName == 'mxGraphModel') {
        this.graph.view.scale = 1;
        this.graph.gridEnabled = node.getAttribute('grid') != '0';
        this.graph.graphHandler.guidesEnabled = node.getAttribute('guides') != '0';
        this.graph.setTooltips(node.getAttribute('tooltips') != '0');
        this.graph.setConnectable(node.getAttribute('connect') != '0');
        this.graph.foldingEnabled = node.getAttribute('fold') != '0';

        this.graph.pageVisible = node.getAttribute('page') == '1';
        this.graph.pageBreaksVisible = this.graph.pageVisible;
        this.graph.preferPageSize = this.graph.pageBreaksVisible;

        // Loads the persistent state settings
        var ps = node.getAttribute('pageScale');

        if (ps != null) {
            this.graph.pageScale = ps;
        }
        else {
            this.graph.pageScale = mxGraph.prototype.pageScale;
        }

        var pw = node.getAttribute('pageWidth');
        var ph = node.getAttribute('pageHeight');

        if (pw != null && ph != null) {
            this.graph.pageFormat = new mxRectangle(0, 0, parseFloat(pw), parseFloat(ph));
            this.outline.outline.pageFormat = this.graph.pageFormat;
        }

        // Loads the persistent state settings
        var bg = node.getAttribute('background');

        if (bg != null && bg.length > 0) {
            this.graph.background = bg;
        }
        else {
            this.graph.background = null;
        }

        dec.decode(node, this.graph.getModel());
        this.updateGraphComponents();
    }
    else if (node.nodeName == 'root') {
        this.resetGraph();

        // Workaround for invalid XML output in Firefox 20 due to bug in mxUtils.getXml
        var wrapper = dec.document.createElement('mxGraphModel');
        wrapper.appendChild(node);

        dec.decode(wrapper, this.graph.getModel());
        this.updateGraphComponents();
    }
    else {
        throw {
            message: 'Cannot open file',
            toString: function () { return this.message; }
        };
    }

    //permissison
    var isEnable = this.editorUi.checkEnable();
    if (isEnable === false)
        this.graph.setConnectable(false);
};

/**
* Returns the XML node that represents the current diagram.
*/
Editor.prototype.getGraphXml = function () {
    var enc = new mxCodec(mxUtils.createXmlDocument());
    var node = enc.encode(this.graph.getModel());
    if (this.graph.view.translate.x != 0 || this.graph.view.translate.y != 0) {
        node.setAttribute('dx', Math.round(this.graph.view.translate.x * 100) / 100);
        node.setAttribute('dy', Math.round(this.graph.view.translate.y * 100) / 100);
    }

    node.setAttribute('grid', (this.graph.isGridEnabled()) ? '1' : '0');
    node.setAttribute('guides', (this.graph.graphHandler.guidesEnabled) ? '1' : '0');
    node.setAttribute('guides', (this.graph.graphHandler.guidesEnabled) ? '1' : '0');
    node.setAttribute('tooltips', (this.graph.tooltipHandler.isEnabled()) ? '1' : '0');
    node.setAttribute('connect', (this.graph.connectionHandler.isEnabled()) ? '1' : '0');
    node.setAttribute('fold', (this.graph.foldingEnabled) ? '1' : '0');
    node.setAttribute('page', (this.graph.pageVisible) ? '1' : '0');
    node.setAttribute('pageScale', this.graph.pageScale);
    node.setAttribute('pageWidth', this.graph.pageFormat.width);
    node.setAttribute('pageHeight', this.graph.pageFormat.height);

    if (this.graph.background != null) {
        node.setAttribute('background', this.graph.background);
    }

    return node;
};

/**
* Keeps the graph container in sync with the persistent graph state
*/
Editor.prototype.updateGraphComponents = function () {
    var graph = this.graph;
    var outline = this.outline;

    if (graph.container != null && outline.outline.container != null) {
        var bg = (graph.background == null || graph.background == 'none') ? '#ffffff' : graph.background;

        if (graph.view.backgroundPageShape != null) {
            graph.view.backgroundPageShape.fill = bg;
            graph.view.backgroundPageShape.reconfigure();
        }

        graph.container.style.backgroundColor = bg;

        if (graph.pageVisible) {
            graph.container.style.backgroundColor = '#ebebeb';
            graph.container.style.borderStyle = 'solid';
            graph.container.style.borderColor = '#e5e5e5';
            graph.container.style.borderTopWidth = '1px';
            graph.container.style.borderLeftWidth = '1px';
            graph.container.style.borderRightWidth = '0px';
            graph.container.style.borderBottomWidth = '0px';
        }
        else {
            graph.container.style.border = '';
        }

        outline.outline.container.style.backgroundColor = graph.container.style.backgroundColor;

        if (outline.outline.pageVisible != graph.pageVisible || outline.outline.pageScale != graph.pageScale) {
            outline.outline.pageScale = graph.pageScale;
            outline.outline.pageVisible = graph.pageVisible;
            outline.outline.view.validate();
        }

        if (!graph.scrollbars) {
            graph.container.style.overflow = 'hidden';
        }
        else if (graph.scrollbars) {
            graph.container.style.overflow = 'auto';
        }

        // Transparent.gif is a workaround for focus repaint problems in IE
        var noBackground = (mxClient.IS_IE && document.documentMode >= 9) ? 'url(' + this.transparentImage + ')' : 'none';
        graph.container.style.backgroundImage = (!graph.pageVisible && graph.isGridEnabled()) ? 'url(' + this.gridImage + ')' : noBackground;

        if (graph.view.backgroundPageShape != null) {
            graph.view.backgroundPageShape.node.style.backgroundImage = (this.graph.isGridEnabled()) ? 'url(' + this.gridImage + ')' : 'none';
        }
    }
};

/**
* Sets the modified flag.
*/
Editor.prototype.setModified = function (value) {
    this.modified = value;
};

/**
* Sets the filename.
*/
Editor.prototype.setFilename = function (value) {
    this.filename = value;
};

/**
* Initializes the environment.
*/
Editor.prototype.init = function () {
    // Adds stylesheet for IE6
    if (mxClient.IS_IE6) {
        mxClient.link('stylesheet', CSS_PATH + '/grapheditor-ie6.css');
    }

    // Adds required resources (disables loading of fallback properties, this can only
    // be used if we know that all keys are defined in the language specific file)
    mxResources.loadDefaultBundle = false;
    mxResources.add(RESOURCE_BASE);

    // Makes the connection hotspot smaller
    mxConstants.DEFAULT_HOTSPOT = 0.3;
    /*
    var mxConnectionHandlerCreateMarker = mxConnectionHandler.prototype.createMarker;
    mxConnectionHandler.prototype.createMarker = function () {
    var marker = mxConnectionHandlerCreateMarker.apply(this, arguments);

        // Overrides to ignore hotspot only for target terminal
    marker.intersects = mxUtils.bind(this, function (state, evt) {
    if (this.isConnecting()) {
    return true;
    }

            return mxCellMarker.prototype.intersects.apply(marker, arguments);
    });

        return marker;
    };
    */
    //DEV:all init editor here
    // Makes the shadow brighter
    mxConstants.SHADOWCOLOR = '#d0d0d0';

    // Changes some default colors
    mxConstants.HANDLE_FILLCOLOR = '#99ccff';
    mxConstants.HANDLE_STROKECOLOR = '#0088cf';
    mxConstants.VERTEX_SELECTION_COLOR = '#00a8ff';
    mxConstants.OUTLINE_COLOR = '#00a8ff';
    mxConstants.OUTLINE_HANDLE_FILLCOLOR = '#99ccff';
    mxConstants.OUTLINE_HANDLE_STROKECOLOR = '#00a8ff';
    mxConstants.CONNECT_HANDLE_FILLCOLOR = '#cee7ff';
    mxConstants.EDGE_SELECTION_COLOR = '#00a8ff';
    mxConstants.DEFAULT_VALID_COLOR = '#00a8ff';
    mxConstants.LABEL_HANDLE_FILLCOLOR = '#cee7ff';
    mxConstants.GUIDE_COLOR = '#0088cf';

    mxGraph.prototype.pageBreakColor = '#c0c0c0';
    mxGraph.prototype.pageScale = 1;

    // Adds rotation handle and live preview
    //DEV:not rotate
    //mxVertexHandler.prototype.rotationEnabled = true;
    mxVertexHandler.prototype.manageSizers = true;
    mxVertexHandler.prototype.livePreview = true;

    // Matches label positions of mxGraph 1.x
    mxText.prototype.baseSpacingTop = 5;
    mxText.prototype.baseSpacingBottom = 1;

    // Increases default rubberband opacity (default is 20)
    mxRubberband.prototype.defaultOpacity = 30;

    // Changes border color of background page shape
    mxGraphView.prototype.createBackgroundPageShape = function (bounds) {
        return new mxRectangleShape(bounds, this.graph.background || 'white', '#cacaca');
    };

    // Fits the number of background pages to the graph
    mxGraphView.prototype.getBackgroundPageBounds = function () {
        var gb = this.getGraphBounds();

        // Computes unscaled, untranslated graph bounds
        var x = (gb.width > 0) ? gb.x / this.scale - this.translate.x : 0;
        var y = (gb.height > 0) ? gb.y / this.scale - this.translate.y : 0;
        var w = gb.width / this.scale;
        var h = gb.height / this.scale;

        var fmt = this.graph.pageFormat;
        var ps = this.graph.pageScale;

        var pw = fmt.width * ps;
        var ph = fmt.height * ps;

        var x0 = Math.floor(Math.min(0, x) / pw);
        var y0 = Math.floor(Math.min(0, y) / ph);
        var xe = Math.ceil(Math.max(1, x + w) / pw);
        var ye = Math.ceil(Math.max(1, y + h) / ph);

        var rows = xe - x0;
        var cols = ye - y0;

        var bounds = new mxRectangle(this.scale * (this.translate.x + x0 * pw), this.scale *
				(this.translate.y + y0 * ph), this.scale * rows * pw, this.scale * cols * ph);

        return bounds;
    };

    // Add panning for background page in VML
    var graphPanGraph = mxGraph.prototype.panGraph;
    mxGraph.prototype.panGraph = function (dx, dy) {
        graphPanGraph.apply(this, arguments);

        if ((this.dialect != mxConstants.DIALECT_SVG && this.view.backgroundPageShape != null) &&
			(!this.useScrollbarsForPanning || !mxUtils.hasScrollbars(this.container))) {
            this.view.backgroundPageShape.node.style.marginLeft = dx + 'px';
            this.view.backgroundPageShape.node.style.marginTop = dy + 'px';
        }
    };

    // Adds pinch support for background page
    // TODO: Scale background page on iOS
    /*var panningHandlerScaleGraph = mxPanningHandler.prototype.scaleGraph;
    mxPanningHandler.prototype.scaleGraph = function(scale, preview)
    {
    panningHandlerScaleGraph.apply(this, arguments);
		
		var shape = this.graph.view.backgroundPageShape;
		
		if (shape != null)
    {
    if (preview)
    {
    mxUtils.setPrefixedStyle(shape.node.style, 'transform', 'scale(' + scale + ')');
    }
    else
    {
    mxUtils.setPrefixedStyle(shape.node.style, 'transform', '');
    }
    }
    };*/

    var editor = this;

    // Uses HTML for background pages (to support grid background image)
    mxGraphView.prototype.validateBackground = function () {
        var bg = this.graph.getBackgroundImage();

        if (bg != null) {
            if (this.backgroundImage == null || this.backgroundImage.image != bg.src) {
                if (this.backgroundImage != null) {
                    this.backgroundImage.destroy();
                }

                var bounds = new mxRectangle(0, 0, 1, 1);

                this.backgroundImage = new mxImageShape(bounds, bg.src);
                this.backgroundImage.dialect = this.graph.dialect;
                this.backgroundImage.init(this.backgroundPane);
                this.backgroundImage.redraw();
            }

            this.redrawBackgroundImage(this.backgroundImage, bg);
        }
        else if (this.backgroundImage != null) {
            this.backgroundImage.destroy();
            this.backgroundImage = null;
        }

        if (this.graph.pageVisible) {
            var bounds = this.getBackgroundPageBounds();

            if (this.backgroundPageShape == null) {
                this.backgroundPageShape = this.createBackgroundPageShape(bounds);
                this.backgroundPageShape.scale = 1;
                this.backgroundPageShape.isShadow = true;
                this.backgroundPageShape.dialect = mxConstants.DIALECT_STRICTHTML;
                this.backgroundPageShape.init(this.graph.container);
                // Required for the browser to render the background page in correct order
                this.graph.container.firstChild.style.position = 'absolute';
                this.graph.container.insertBefore(this.backgroundPageShape.node, this.graph.container.firstChild);
                this.backgroundPageShape.redraw();

                this.backgroundPageShape.node.className = 'geBackgroundPage';
                this.backgroundPageShape.node.style.backgroundPosition = '-1px -1px';

                // Adds listener for double click handling on background
                mxEvent.addListener(this.backgroundPageShape.node, 'dblclick',
					mxUtils.bind(this, function (evt) {
					    this.graph.dblClick(evt);
					})
				);

                // Adds basic listeners for graph event dispatching outside of the
                // container and finishing the handling of a single gesture
                mxEvent.addGestureListeners(this.backgroundPageShape.node,
					mxUtils.bind(this, function (evt) {
					    this.graph.fireMouseEvent(mxEvent.MOUSE_DOWN, new mxMouseEvent(evt));
					}),
					mxUtils.bind(this, function (evt) {
					    // Hides the tooltip if mouse is outside container
					    if (this.graph.tooltipHandler != null && this.graph.tooltipHandler.isHideOnHover()) {
					        this.graph.tooltipHandler.hide();
					    }

					    if (this.graph.isMouseDown && !mxEvent.isConsumed(evt)) {
					        this.graph.fireMouseEvent(mxEvent.MOUSE_MOVE, new mxMouseEvent(evt));
					    }
					}),
					mxUtils.bind(this, function (evt) {
					    this.graph.fireMouseEvent(mxEvent.MOUSE_UP, new mxMouseEvent(evt));
					}));
            }
            else {
                this.backgroundPageShape.scale = 1;
                this.backgroundPageShape.bounds = bounds;
                this.backgroundPageShape.redraw();
            }

            this.backgroundPageShape.node.style.backgroundImage = (this.graph.isGridEnabled()) ?
					'url(' + editor.gridImage + ')' : 'none';
        }
        else if (this.backgroundPageShape != null) {
            this.backgroundPageShape.destroy();
            this.backgroundPageShape = null;
        }
    };

    // Draws page breaks only within the page
    mxGraph.prototype.updatePageBreaks = function (visible, width, height) {
        var scale = this.view.scale;
        var tr = this.view.translate;
        var fmt = this.pageFormat;
        var ps = scale * this.pageScale;

        var bounds2 = this.view.getBackgroundPageBounds();

        width = bounds2.width;
        height = bounds2.height;
        var bounds = new mxRectangle(scale * tr.x, scale * tr.y, fmt.width * ps, fmt.height * ps);

        // Does not show page breaks if the scale is too small
        visible = visible && Math.min(bounds.width, bounds.height) > this.minPageBreakDist;

        var horizontalCount = (visible) ? Math.ceil(width / bounds.width) - 1 : 0;
        var verticalCount = (visible) ? Math.ceil(height / bounds.height) - 1 : 0;
        var right = bounds2.x + width;
        var bottom = bounds2.y + height;

        if (this.horizontalPageBreaks == null && horizontalCount > 0) {
            this.horizontalPageBreaks = [];
        }

        if (this.horizontalPageBreaks != null) {
            for (var i = 0; i <= horizontalCount; i++) {
                var pts = [new mxPoint(bounds2.x + (i + 1) * bounds.width, bounds2.y),
				           new mxPoint(bounds2.x + (i + 1) * bounds.width, bottom)];

                if (this.horizontalPageBreaks[i] != null) {
                    this.horizontalPageBreaks[i].scale = 1;
                    this.horizontalPageBreaks[i].points = pts;
                    this.horizontalPageBreaks[i].redraw();
                }
                else {
                    var pageBreak = new mxPolyline(pts, this.pageBreakColor, this.scale);
                    pageBreak.dialect = this.dialect;
                    pageBreak.isDashed = this.pageBreakDashed;
                    pageBreak.addPipe = false;
                    pageBreak.scale = scale;
                    pageBreak.init(this.view.backgroundPane);
                    pageBreak.redraw();

                    this.horizontalPageBreaks[i] = pageBreak;
                }
            }

            for (var i = horizontalCount; i < this.horizontalPageBreaks.length; i++) {
                this.horizontalPageBreaks[i].destroy();
            }

            this.horizontalPageBreaks.splice(horizontalCount, this.horizontalPageBreaks.length - horizontalCount);
        }

        if (this.verticalPageBreaks == null && verticalCount > 0) {
            this.verticalPageBreaks = [];
        }

        if (this.verticalPageBreaks != null) {
            for (var i = 0; i <= verticalCount; i++) {
                var pts = [new mxPoint(bounds2.x, bounds2.y + (i + 1) * bounds.height),
				           new mxPoint(right, bounds2.y + (i + 1) * bounds.height)];

                if (this.verticalPageBreaks[i] != null) {
                    this.verticalPageBreaks[i].scale = 1; //scale;
                    this.verticalPageBreaks[i].points = pts;
                    this.verticalPageBreaks[i].redraw();
                }
                else {
                    var pageBreak = new mxPolyline(pts, this.pageBreakColor, scale);
                    pageBreak.dialect = this.dialect;
                    pageBreak.isDashed = this.pageBreakDashed;
                    pageBreak.addPipe = false;
                    pageBreak.scale = scale;
                    pageBreak.init(this.view.backgroundPane);
                    pageBreak.redraw();

                    this.verticalPageBreaks[i] = pageBreak;
                }
            }

            for (var i = verticalCount; i < this.verticalPageBreaks.length; i++) {
                this.verticalPageBreaks[i].destroy();
            }

            this.verticalPageBreaks.splice(verticalCount, this.verticalPageBreaks.length - verticalCount);
        }
    };

    // Enables snapping to off-grid terminals for edge waypoints
    mxEdgeHandler.prototype.snapToTerminals = true;

    // Enables guides
    mxGraphHandler.prototype.guidesEnabled = true;

    // Disables removing relative children from parents
    var mxGraphHandlerShouldRemoveCellsFromParent = mxGraphHandler.prototype.shouldRemoveCellsFromParent;
    mxGraphHandler.prototype.shouldRemoveCellsFromParent = function (parent, cells, evt) {
        for (var i = 0; i < cells.length; i++) {
            if (this.graph.getModel().isVertex(cells[i])) {
                var geo = this.graph.getCellGeometry(cells[i]);

                if (geo != null && geo.relative) {
                    return false;
                }
            }
        }

        return mxGraphHandlerShouldRemoveCellsFromParent.apply(this, arguments);
    };

    // Alt-move disables guides
    mxGuide.prototype.isEnabledForEvent = function (evt) {
        return !mxEvent.isAltDown(evt);
    };

    // Consumes click events for disabled menu items
    mxPopupMenuAddItem = mxPopupMenu.prototype.addItem;
    mxPopupMenu.prototype.addItem = function (title, image, funct, parent, iconCls, enabled) {
        var result = mxPopupMenuAddItem.apply(this, arguments);

        if (enabled !== null && !enabled) {
            mxEvent.addListener(result, 'mousedown', function (evt) {
                mxEvent.consume(evt);
            });
        }

        return result;
    };

    // Selects descendants before children selection mode
    var graphHandlerGetInitialCellForEvent = mxGraphHandler.prototype.getInitialCellForEvent;
    mxGraphHandler.prototype.getInitialCellForEvent = function (me) {
        var model = this.graph.getModel();
        var psel = model.getParent(this.graph.getSelectionCell());
        var cell = graphHandlerGetInitialCellForEvent.apply(this, arguments);
        var parent = model.getParent(cell);

        if (psel == null || (psel != cell && psel != parent)) {
            while (!this.graph.isCellSelected(cell) && !this.graph.isCellSelected(parent) &&
					model.isVertex(parent) && !this.graph.isValidRoot(parent)) {
                cell = parent;
                parent = this.graph.getModel().getParent(cell);
            }
        }

        return cell;
    };

    // Selection is delayed to mouseup if child selected
    var graphHandlerIsDelayedSelection = mxGraphHandler.prototype.isDelayedSelection;
    mxGraphHandler.prototype.isDelayedSelection = function (cell) {
        var result = graphHandlerIsDelayedSelection.apply(this, arguments);
        var model = this.graph.getModel();
        var psel = model.getParent(this.graph.getSelectionCell());
        var parent = model.getParent(cell);

        if (psel == null || (psel != cell && psel != parent)) {
            if (!this.graph.isCellSelected(cell) && model.isVertex(parent) && !this.graph.isValidRoot(parent)) {
                result = true;
            }
        }

        return result;
    };

    // Delayed selection of parent group
    mxGraphHandler.prototype.selectDelayed = function (me) {
        if (!this.graph.popupMenuHandler.isPopupTrigger(me)) {
            var cell = me.getCell();

            if (cell == null) {
                cell = this.cell;
            }

            var model = this.graph.getModel();
            var parent = model.getParent(cell);

            while (this.graph.isCellSelected(cell) && model.isVertex(parent) && !this.graph.isValidRoot(parent)) {
                cell = parent;
                parent = model.getParent(cell);
            }

            this.graph.selectCellForEvent(cell, me.getEvent());
        }
    };

    // Returns last selected ancestor
    mxPopupMenuHandler.prototype.getCellForPopupEvent = function (me) {
        var cell = me.getCell();
        var model = this.graph.getModel();
        var parent = model.getParent(cell);

        while (model.isVertex(parent) && !this.graph.isValidRoot(parent)) {
            if (this.graph.isCellSelected(parent)) {
                cell = parent;
            }

            parent = model.getParent(parent);
        }

        return cell;
    };
};


/*DEV:info window*/
Editor.prototype.outlineWindow = undefined;
Editor.prototype.outlineWindowDesign = undefined;
Editor.prototype.createOutlineWindow = function () {
    var tb = document.createElement('div');
    tb.className = 'maintable';
    var row, td;
    var ed = this;

    //line info
    var table = document.createElement('table');
    table.className = 'infoElement';
    table.style.width = '100%';
    table.style.display = 'none';
    var tbody = document.createElement('tbody');
    for (var i = 0; i < 4; i++) {
        row = document.createElement('tr');
        row.className = 'row';
        for (var k = 0; k < 2; k++) {
            td = document.createElement('td');
            td.className = k === 0 ? 'label' : 'data';
            var span = document.createElement('span');
            if (k === 0) {
                var label = '';
                switch (i) {
                    case 0:
                        label = SweetSoftScript.ResourceText.ComponentName;
                        break;
                    case 1:
                        label = SweetSoftScript.ResourceText.ComponentType;
                        break;
                    case 2:
                        label = SweetSoftScript.ResourceText.relatedWork;
                        break;
                    case 3:
                        label = SweetSoftScript.ResourceText.relatedMainStep;
                        break;
                }
                $(span).text(label);
            }
            td.appendChild(span);
            if (i === 2 && k === 1) {
                span.className = 'textsub';

                var btnShowRelated = mxUtils.button(SweetSoftScript.ResourceText.btnDetail, function () {
                    ed.editorUi.processShowSelectWork();
                });
                btnShowRelated.id = 'btnShowRelated';
                btnShowRelated.style.display = 'none';
                td.appendChild(btnShowRelated);
                row.className = 'row aligntop';
            }
            row.appendChild(td);
        }
        tbody.appendChild(row);
    }
    var trinfo = document.createElement('tr');
    var tdinfo = document.createElement('td');
    var btnShowRecord = mxUtils.button(SweetSoftScript.ResourceText.btnShowRecord, function () {
        //ed.outlineWindow.setMinimizable(true);
        ed.editorUi.showProductionProperty();
    });
    btnShowRecord.id = 'btnShowRecord';
    btnShowRecord.style.display = 'none';
    tdinfo.appendChild(btnShowRecord);
    tdinfo.setAttribute('colspan', '2');
    trinfo.setAttribute('id', 'infoElement');
    trinfo.appendChild(tdinfo);
    tbody.appendChild(trinfo);
    table.appendChild(tbody);
    tb.appendChild(table);

    //element info
    table = document.createElement('table');
    table.className = 'infoLine';
    table.style.width = '100%';
    table.style.display = 'none';
    tbody = document.createElement('tbody');
    for (var i = 0; i < 3; i++) {
        row = document.createElement('tr');
        row.className = 'row';
        for (var k = 0; k < 2; k++) {
            td = document.createElement('td');
            td.className = k === 0 ? 'label' : 'data';
            if (k === 1 && i === 2) {
                var cb = document.createElement('input');
                cb.setAttribute('type', 'checkbox');
                cb.setAttribute('checked', 'checked');
                td.appendChild(cb);
            }
            else {
                var span = document.createElement('span');
                if (k === 0) {
                    var label = '';
                    switch (i) {
                        case 0:
                            label = SweetSoftScript.ResourceText.startPlace;
                            break;
                        case 1:
                            label = SweetSoftScript.ResourceText.endPlace;
                            break;
                        case 2:
                            label = SweetSoftScript.ResourceText.xulytheoong;
                            break;
                    }
                    $(span).text(label);
                }
                td.appendChild(span);
            }
            row.appendChild(td);
        }
        tbody.appendChild(row);
    }
    table.appendChild(tbody);
    tb.appendChild(table);

    var ww = 250;
    var wh = 250;
    var ws = SweetSoftScript.mainFunction.getWindowSize();
    var pl = ws.width - ww - 30;
    var pt = 35;
    //mxWindow(title, content, x, y, width, height, minimizable, movable, replaceNode, style)
    var wnd = new mxWindow('', tb, pl, pt, ww, wh, true, true);

    wnd.setVisible(true);
    wnd.setResizable(true);
    wnd.setClosable(true);
    //wnd.setLocation(x,y);
    wnd.addListener(mxEvent.HIDE, function (e) {
        document.getElementById('btnInfo').className = 'btnInfo geButton';
        wnd.div.style.zIndex = '-1';
    });
    wnd.addListener(mxEvent.SHOW, function (e) {
        document.getElementById('btnInfo').className = 'btnInfo geButton hide';
        wnd.div.style.zIndex = '2';
    });

    wnd.addListener(mxEvent.ACTIVATE, function (e) {
        ed.outlineWindowDesign.div.style.zIndex = '1';
        var selectprop = ed.editorUi.getProductionPropertyWindow();
        if (typeof selectprop !== 'undefined')
            selectprop.window.div.style.zIndex = '1';
    });
    wnd.destroyOnClose = false;
    wnd.setMaximizable(false);
    //wnd.setTitle(title);
    ed.outlineWindow = wnd;
};

Editor.prototype.toggleInfo = function (isShow) {
    if (this.outlineWindow !== null && typeof this.outlineWindow !== 'undefined') {
        if (typeof isShow !== 'undefined' && isShow === true)
            this.outlineWindow.setVisible(true);
        else
            this.outlineWindow.setVisible(!this.outlineWindow.isVisible());
    }
    else {
        this.createOutlineWindow();
        this.outlineWindow.setVisible(true);
        document.getElementById('btnInfo').className = 'btnInfo geButton hide';
    }
};

Editor.prototype.isShowWork = false;
Editor.prototype.displayInfoData = function (obj) {
    var data = obj.dataobj;

    if (typeof data === 'undefined')
        return;

    if (data.isEdge === false)
        this.isShowWork = false;

    var title = data.isEdge ? SweetSoftScript.ResourceText.edge : SweetSoftScript.ResourceText.vertex;
    var cl = data.isEdge ? 'infoLine' : 'infoElement';
    var divMain = this.outlineWindow.getElement();
    this.outlineWindow.setTitle(title);
    var tables = divMain.getElementsByTagName('table');
    if (tables !== null && typeof tables !== 'undefined') {
        for (var n = 0, m = tables.length; n < m; n++) {
            if (tables[n].className === cl) {
                tables[n].style.display = 'table';

                var spans = tables[n].getElementsByTagName('span');

                if (spans !== null && typeof spans !== 'undefined' && spans.length > 0) {
                    //process info
                    /*
                    for (var i = 0, j = spans.length; i < j; i += 2) {
                        
                    var s = '';
                    for (var k = i; k < i + 2; k++) {
                    s +=$(spans[k]).text() + (k % 2 === 0 ? 'is ' : '');
                    }
                    
                    }
                    */

                    if (data.isEdge) {
                        $(spans[1]).text(data.sourceValue);
                        $(spans[3]).text(data.targetValue);
                        document.getElementById('btnShowRelated').style.display = 'none';
                        document.getElementById('btnShowRecord').style.display = 'none';
                        var selectprop = this.editorUi.getProductionPropertyWindow();
                        if (typeof selectprop !== 'undefined')
                            selectprop.window.hide();
                    }
                    else {
                        $(spans[1]).text(data.value);
                        if (data.isSubWork !== null && typeof data.isSubWork !== 'undefined') {
                            //$(spans[5]).text(data.value);

                            $(spans[3]).text(SweetSoftScript.ResourceText.groupSubwork);
                            this.editorUi.showRelatedWork(obj, spans[5]);
                            document.getElementById('btnShowRelated').style.display = 'inline-block';
                            document.getElementById('btnShowRecord').style.display = 'none';
                            var selectprop = this.editorUi.getProductionPropertyWindow();
                            if (typeof selectprop !== 'undefined')
                                selectprop.window.hide();
                        }
                        else {
                            $(spans[5]).text(data.value);
                            $(spans[3]).text(data.isMain ? SweetSoftScript.ResourceText.groupDept : SweetSoftScript.ResourceText.groupWorktask);
                            $(spans[7]).text(SweetSoftScript.ResourceText.doesnot);
                            document.getElementById('btnShowRelated').style.display = 'none';

                            if (data.isMain === 1) {
                                document.getElementById('btnShowRecord').style.display = 'none';
                                var selectprop = this.editorUi.getProductionPropertyWindow();
                                if (typeof selectprop !== 'undefined')
                                    selectprop.window.hide();
                            }
                            else {
                                if (this.editorUi.checkProductionProperty() === true)
                                    document.getElementById('btnShowRecord').style.display = 'inline-block';
                                else
                                    document.getElementById('btnShowRecord').style.display = 'none';
                                var selectprop = this.editorUi.getProductionPropertyWindow();
                                if (typeof selectprop !== 'undefined') {
                                    if (selectprop.window.isVisible())
                                        this.editorUi.showProductionProperty();
                                }
                            }
                        }
                    }
                }
            }
            else if (tables[n].className !== 'mxWindow') {

                tables[n].style.display = 'none';
            }
        }

    }
};

Editor.prototype.displayInfoCell = function (obj) {
    this.toggleInfo(true);
    if (obj !== null && typeof obj !== 'undefined') {

        //Qui cách đường chuyển - mũi tên
        //Qui cách thành phần - element

        this.displayInfoData(obj);
    }
};

/**
* Creates and returns a new undo manager.
*/
Editor.prototype.createUndoManager = function () {
    var graph = this.graph;
    var undoMgr = new mxUndoManager();

    this.undoListener = function (sender, evt) {
        undoMgr.undoableEditHappened(evt.getProperty('edit'));
    };

    // Installs the command history
    var listener = mxUtils.bind(this, function (sender, evt) {
        this.undoListener.apply(this, arguments);
    });

    graph.getModel().addListener(mxEvent.UNDO, listener);
    graph.getView().addListener(mxEvent.UNDO, listener);

    // Keeps the selection in sync with the history
    var undoHandler = function (sender, evt) {
        var cand = graph.getSelectionCellsForChanges(evt.getProperty('edit').changes);
        var cells = [];

        for (var i = 0; i < cand.length; i++) {
            if (graph.view.getState(cand[i]) != null) {
                cells.push(cand[i]);
            }
        }

        graph.setSelectionCells(cells);
    };

    undoMgr.addListener(mxEvent.UNDO, undoHandler);
    undoMgr.addListener(mxEvent.REDO, undoHandler);

    return undoMgr;
};

/**
* Adds basic stencil set (no namespace).
*/
Editor.prototype.initStencilRegistry = function () {
    // Loads default stencils
    mxStencilRegistry.loadStencilSet(STENCIL_PATH + '/general.xml');
};

/**
* Overrides stencil registry for dynamic loading of stencils.
*/
(function () {
    /**
    * Maps from library names to an array of Javascript filenames,
    * which are synchronously loaded. Currently only stencil files
    * (.xml) and JS files (.js) are supported.
    * IMPORTANT: For embedded diagrams to work entries must also
    * be added in EmbedServlet.java.
    */
    mxStencilRegistry.libraries = {};

    /**
    * Stores all package names that have been dynamically loaded.
    * Each package is only loaded once.
    */
    mxStencilRegistry.packages = [];

    // Extends the default stencil registry to add dynamic loading
    mxStencilRegistry.getStencil = function (name) {
        var result = mxStencilRegistry.stencils[name];

        if (result == null) {
            var basename = mxStencilRegistry.getBasenameForStencil(name);

            // Loads stencil files and tries again
            if (basename != null) {
                var libs = mxStencilRegistry.libraries[basename];

                if (libs != null) {
                    if (mxStencilRegistry.packages[basename] == null) {
                        mxStencilRegistry.packages[basename] = 1;

                        for (var i = 0; i < libs.length; i++) {
                            var fname = libs[i];

                            if (fname.toLowerCase().substring(fname.length - 4, fname.length) == '.xml') {
                                mxStencilRegistry.loadStencilSet(fname, null);
                            }
                            else if (fname.toLowerCase().substring(fname.length - 3, fname.length) == '.js') {
                                var req = mxUtils.load(fname);

                                if (req != null) {
                                    eval.call(window, req.getText());
                                }
                            }
                            else {
                                // FIXME: This does not yet work as the loading is triggered after
                                // the shape was used in the graph, at which point the keys have
                                // typically been translated in the calling method.
                                //mxResources.add(fname);
                            }
                        }
                    }
                }
                else {
                    mxStencilRegistry.loadStencilSet(STENCIL_PATH + '/' + basename + '.xml', null);
                }

                result = mxStencilRegistry.stencils[name];
            }
        }

        return result;
    };

    // Returns the basename for the given stencil or null if no file must be
    // loaded to render the given stencil.
    mxStencilRegistry.getBasenameForStencil = function (name) {
        var parts = name.split('.');
        var tmp = null;

        if (parts.length > 0 && parts[0] == 'mxgraph') {
            tmp = parts[1];

            for (var i = 2; i < parts.length - 1; i++) {
                tmp += '/' + parts[i];
            }
        }

        return tmp;
    };

    // Loads the given stencil set
    mxStencilRegistry.loadStencilSet = function (stencilFile, postStencilLoad, force) {
        force = (force != null) ? force : false;

        // Uses additional cache for detecting previous load attempts
        var xmlDoc = mxStencilRegistry.packages[stencilFile];

        if (force || xmlDoc == null) {
            var install = false;

            if (xmlDoc == null) {
                var req = mxUtils.load(stencilFile);
                xmlDoc = req.getXml();
                mxStencilRegistry.packages[stencilFile] = xmlDoc;
                install = true;
            }

            mxStencilRegistry.parseStencilSet(xmlDoc.documentElement, postStencilLoad, install);
        }
    };

    // Parses the given stencil set
    mxStencilRegistry.parseStencilSet = function (root, postStencilLoad, install) {
        install = (install != null) ? install : true;
        var shape = root.firstChild;
        var packageName = '';
        var name = root.getAttribute('name');

        if (name != null) {
            packageName = name + '.';
        }

        while (shape != null) {
            if (shape.nodeType == mxConstants.NODETYPE_ELEMENT) {
                name = shape.getAttribute('name');

                if (name != null) {
                    packageName = packageName.toLowerCase();
                    var stencilName = name.replace(/ /g, "_");

                    if (install) {
                        mxStencilRegistry.addStencil(packageName + stencilName.toLowerCase(), new mxStencil(shape));
                    }

                    if (postStencilLoad != null) {
                        var w = shape.getAttribute('w');
                        var h = shape.getAttribute('h');

                        w = (w == null) ? 80 : parseInt(w, 10);
                        h = (h == null) ? 80 : parseInt(h, 10);

                        postStencilLoad(packageName, stencilName, name, w, h);
                    }
                }
            }

            shape = shape.nextSibling;
        }
    };
})();

/**
* Class for asynchronously opening a new window and loading a file at the same
* time. This acts as a bridge between the open dialog and the new editor.
*/
OpenFile = function (done) {
    this.producer = null;
    this.consumer = null;
    this.done = done;
};

/**
* Registers the editor from the new window.
*/
OpenFile.prototype.setConsumer = function (value) {
    this.consumer = value;
    this.execute();
};

/**
* Sets the data from the loaded file.
*/
OpenFile.prototype.setData = function (value, filename) {
    this.data = value;
    this.filename = filename;
    this.execute();
};

/**
* Displays an error message.
*/
OpenFile.prototype.error = function (msg) {
    this.cancel(true);
    mxUtils.alert(msg);
};

/**
* Consumes the data.
*/
OpenFile.prototype.execute = function () {
    if (this.consumer != null && this.data != null) {
        this.cancel(false);
        this.consumer(this.data, this.filename);
    }
};

/**
* Cancels the operation.
*/
OpenFile.prototype.cancel = function (cancel) {
    if (this.done != null) {
        this.done((cancel != null) ? cancel : true);
    }
};
