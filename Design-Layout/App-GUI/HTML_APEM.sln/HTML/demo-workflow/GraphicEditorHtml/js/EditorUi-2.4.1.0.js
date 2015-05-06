/**
* $Id: EditorUi.js,v 1.50 2014/02/10 11:52:26 gaudenz Exp $
* Copyright (c) 2006-2012, JGraph Ltd
*/
/**
* Constructs a new graph editor
*/
EditorUi = function (editor, container) {
    mxEventSource.call(this);
    this.editor = editor || new Editor();
    this.container = container || document.body;
    this.editor.editorUi = this;

    var graph = this.editor.graph;

    // Pre-fetches submenu image
    new Image().src = mxPopupMenu.prototype.submenuImage;

    // Pre-fetches connect image
    if (mxConnectionHandler.prototype.connectImage !== null) {
        new Image().src = mxConnectionHandler.prototype.connectImage.src;
    }

    // Creates the user interface
    if (typeof Actions !== 'undefined')
        this.actions = new Actions(this);
    if (typeof Menus !== 'undefined')
        this.menus = new Menus(this);
    this.createDivs();
    this.refresh();
    this.createUi();


    // Disables HTML and text selection
    var textEditing = mxUtils.bind(this, function (evt) {
        if (evt == null) {
            evt = window.event;
        }

        if (this.isSelectionAllowed(evt)) {
            return true;
        }

        return graph.isEditing();
    });

    // Disables text selection while not editing and no dialog visible
    if (this.container === document.body) {
        //DEV:hide menu bar
        //this.menubarContainer.onselectstart = textEditing;
        //this.menubarContainer.onmousedown = textEditing;
        this.toolbarContainer.onselectstart = textEditing;
        this.toolbarContainer.onmousedown = textEditing;
        this.diagramContainer.onselectstart = textEditing;
        this.diagramContainer.onmousedown = textEditing;
        this.sidebarContainer.onselectstart = textEditing;
        this.sidebarContainer.onmousedown = textEditing;
        this.footerContainer.onselectstart = textEditing;
        this.footerContainer.onmousedown = textEditing;
    }

    // And uses built-in context menu while editing
    if (mxClient.IS_IE && (typeof (document.documentMode) === 'undefined' || document.documentMode < 9)) {
        mxEvent.addListener(this.diagramContainer, 'contextmenu', textEditing);
    }
    else {
        // Allows browser context menu outside of diagram and sidebar
        this.diagramContainer.oncontextmenu = textEditing;
    }

    // Contains the main graph instance inside the given panel
    graph.init(this.diagramContainer);
    //graph.refresh();

    var textMode = false;
    var nodes = null;
    var ui = this;

    var updateToolbar = mxUtils.bind(this, function () {
        if (textMode !== graph.cellEditor.isContentEditing()) {
            var node = this.toolbar.container.firstChild;
            var newNodes = [];

            while (node != null) {
                var tmp = node.nextSibling;
                node.parentNode.removeChild(node);
                newNodes.push(node);
                node = tmp;
            }

            if (nodes === null) {
                this.toolbar.createTextToolbar();
            }
            else {
                for (var i = 0; i < nodes.length; i++) {
                    this.toolbar.container.appendChild(nodes[i]);
                }
            }

            textMode = graph.cellEditor.isContentEditing();
            nodes = newNodes;
        }
    });

    // Overrides cell editor to update toolbar
    var cellEditorStartEditing = graph.cellEditor.startEditing;
    graph.cellEditor.startEditing = function (cell) {
        cellEditorStartEditing.apply(this, arguments);
        updateToolbar();
        ui.lastSubWorkValue = cell.value;

    };

    var cellEditorStopEditing = graph.cellEditor.stopEditing;
    graph.cellEditor.stopEditing = function (cell, trigger) {
        cellEditorStopEditing.apply(this, arguments);
        updateToolbar();
        if (cell === false) {

            var cur = graph.getSelectionCells();
            if (cur === null || cur.length === 0)
                cur = [EditorUi.prototype.lastClickElement];

            if (cur !== null && typeof cur !== 'undefined' && cur.length > 0) {
                var node = cur[0];
                if (node.isSubWork !== null && typeof node.isSubWork !== 'undefined') {
                    if (node.isSubWork === 1 || node.isSubWork === '1') {
                        var val = node.value;
                        if (val === null || typeof val === 'undefined' || val.length === 0) {
                            /*old
                            alert('remove cell');
                            
                            if (ui.isNewSubwork(node.id)) {
                            }
                            else
                            ui.saveActionNode(node, 'delete');
                            graph.removeCells(cur);
                            */

                            //new
                            SweetSoftScript.mainFunction.OpenRadWindow(undefined,
                              SweetSoftScript.ResourceText.mustEnterSubworkName,
                               'confirmDelete',
                               function () {
                                   if (ui.isNewSubwork(node.id.toString())) {
                                   }
                                   else
                                       ui.saveActionNode(node, 'delete');
                                   graph.removeCells(cur);
                               }, function () {
                                   graph.startEditingAtCell(node);
                               });
                        }
                        else {
                            if (ui.lastSubWorkValue !== node.value) {

                                if (ui.isNewSubwork(node.id.toString()))
                                    ui.saveActionNode(node, 'insert');
                                else
                                    ui.saveActionNode(node, 'update');
                            }
                        }
                    }
                }
            }
        }
    };



    // Enables scrollbars and sets cursor style for the container
    graph.container.setAttribute('tabindex', '0');
    graph.container.style.cursor = 'default';
    graph.container.style.backgroundImage = 'url(' + editor.gridImage + ')';
    graph.container.style.backgroundPosition = '-1px -1px';
    graph.container.focus();

    // Overrides double click handling to use the tolerance and
    // redirect to the image action for image shapes


    //DEV:disable doubleclick on edge for flip
    mxElbowEdgeHandler.prototype.flipEnabled = false;
    mxElbowEdgeHandler.prototype.doubleClickOrientationResource = '';

    var graphDblClick = graph.dblClick;
    graph.dblClick = function (evt, cell) {
        return;
    };


    var graphClick = graph.click;
    graph.click = function (mxMouseEvent) {
        if (graph.isEnabled()) {

            var evtc = mxMouseEvent.evt || window.event;
            if (evtc.which === 3)
                return;
            var st = mxMouseEvent.state;
            if (st !== null && typeof st !== 'undefined') {
                if (st.cell.isEdge()) {

                    EditorUi.prototype.lastClickEdge = st.cell;
                    if (st.cell.source !== null && st.cell.target !== null)
                        EditorUi.prototype.lastSourceAndTarget = [st.cell.source.id.toString(), st.cell.target.id.toString()];
                    var curstyle = st.cell.getStyle();
                    console.log('curstyle : ', curstyle);
                    if (curstyle) {
                        if (curstyle.indexOf(SweetSoftScript.Setting.colorSubLine) >= 0) {
                            $('#linesub').prop('checked', true);
                        }
                        else {
                            $('#linesub').prop('checked', false);
                        }
                    } else {
                        $('#linesub').prop('checked', false);
                    }


                    if ((typeof st.cell.source.isTarget === 'undefined' &&
                        typeof st.cell.source.dataCode === 'undefined' &&
                        typeof st.cell.source.isSource === 'undefined') ||
                        (typeof st.cell.target.isSource === 'undefined' &&
                        typeof st.cell.target.dataCode === 'undefined' &&
                        typeof st.cell.target.isTarget === 'undefined')) {
                        $('#linetype').hide();
                    }
                    else
                        $('#linetype').show();
                }
                else {
                    //console.log(st.cell);
                    EditorUi.prototype.lastClickElement = st.cell;
                    if (typeof EditorUi.prototype.lastClickEdge !== 'undefined' &&
                        typeof EditorUi.prototype.lastSourceAndTarget !== 'undefined') {
                        var edge = EditorUi.prototype.lastClickEdge;
                        if (typeof edge !== 'undefined' && edge !== null && edge.source !== null && edge.target !== null) {
                            var arr = EditorUi.prototype.lastSourceAndTarget;
                            if (edge.source.id.toString() === arr[0] && edge.target.id.toString() === arr[1]) {

                            }
                            else {
                                if (st.cell.source !== null && st.cell.target !== null)
                                    EditorUi.prototype.lastSourceAndTarget = [edge.source.id.toString(), edge.target.id.toString()];
                                ui.saveActionNode(edge, 'update');
                                ui.saveActionNode(edge.source, 'update');
                                ui.saveActionNode(edge.target, 'update');
                                if (edge.source.id.toString() === arr[0]) {
                                    //add target
                                    var allNode = ui.editor.graph.model.root.children[0].children;
                                    if (allNode.length > 0) {
                                        for (var i = 0; i < allNode.length; i++) {
                                            if (allNode[i].id.toString() === arr[1].toString()) {
                                                ui.saveActionNode(allNode[i], 'update');
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    $('#linetype').hide();
                }

                ui.showInfoCell(st.cell);
            }
            else {
                $('#linetype').hide();

                //if (ui.isMainDocument()) {
                //hide info window
                if (ui.editor.outlineWindow !== null && typeof ui.editor.outlineWindow !== 'undefined') { }
                else {
                    ui.editor.createOutlineWindow();
                    ui.editor.outlineWindow.setVisible(false);
                }
                var cell = graph.getSelectionCell();

                if (cell !== null && typeof cell !== 'undefined') {
                    if (cell.isEdge()) {

                        if (cell.source !== null && cell.target !== null) {
                            EditorUi.prototype.lastClickEdge = cell;
                            EditorUi.prototype.lastSourceAndTarget = [cell.source, cell.target];
                        }
                        else {

                            graph.removeCells([cell]);
                            EditorUi.prototype.lastClickEdge = undefined;
                        }
                    }
                    else {
                        EditorUi.prototype.lastClickElement = cell;
                        if (cell.isSubWork === 1 || cell.isSubWork === '1') {
                            if (cell.value === null || typeof cell.value === 'undefined' || cell.value.length === 0)
                                graph.startEditingAtCell(cell);
                        }
                        else {

                            if (cell.edges !== null && cell.edges.length > 0) {
                                var lastEdge = cell.edges[cell.edges.length - 1];
                                if (lastEdge.source !== null && lastEdge.target !== null) { }
                                else {
                                    graph.removeCells([lastEdge]);

                                    EditorUi.prototype.lastClickEdge = undefined;
                                }
                            }
                        }
                    }
                }
                else {
                    cell = EditorUi.prototype.lastClickElement;
                    if (cell !== null && typeof cell !== 'undefined') {
                        if (cell.isSubWork === 1 || cell.isSubWork === '1') {
                            if (cell.value === null || typeof cell.value === 'undefined' || cell.value.length === 0)
                                graph.startEditingAtCell(cell);
                        }
                    }
                }

                if (ui.isMainDocument()) {
                    //DEV:not hide infowindow til click close
                    //if (ui.editor.outlineWindow.isVisible() === true)
                    //    ui.editor.outlineWindow.setVisible(false);
                }
                else {
                    ui.showWork();
                }
            }
            graphClick.call(this, mxMouseEvent);
        }
    }

    var insertEdgeMain = graph.insertEdge;
    graph.insertEdge = function (a, b, c, d, e, f) {
        var node = insertEdgeMain.apply(this, arguments);
        /*
        var islinesub = $('#linesub').prop('checked');
        if (islinesub === true) {
            graph.setCellStyles(mxConstants.STYLE_STROKECOLOR, islinesub ? '#e43ed2' : '#000000', [node]);
            graph.setCellStyles(mxConstants.STYLE_EDGE, 'elbowEdgeStyle', [node]);
        }
        */
        ui.saveActionNode(node, 'insert');
    }

    // Keeps graph container focused on mouse down
    var graphFireMouseEvent = graph.fireMouseEvent;
    graph.fireMouseEvent = function (evtName, me, sender) {
        if (evtName === mxEvent.MOUSE_DOWN) {
            this.container.focus();
        }

        graphFireMouseEvent.apply(this, arguments);
    };

    // Configures automatic expand on mouseover
    graph.popupMenuHandler.autoExpand = true;

    var apply = function (permission) {
        graph.clearSelection();
        graph.setEnabled(true);
        permission.apply(graph);
        // Updates the icons on the shapes - rarely
        // needed and very slow for large graphs
        graph.refresh();
    };

    apply(ui.getPermission());
    // Extends hook functions to use permission object. This could
    // be done by assigning the respective switches (eg.
    // setMovable), but this approach is more flexible, doesn't
    // override any existing behaviour or settings, and allows for
    // dynamic conditions to be used in the functions. See the
    // specification for more functions to extend (eg.
    // isSelectable).

    var oldDisconnectable = graph.isCellDisconnectable;
    graph.isCellDisconnectable = function (cell, terminal, source) {
        return oldDisconnectable.apply(this, arguments) &&
            ui.getPermission().editEdges;
    };

    var oldTerminalPointMovable = graph.isTerminalPointMovable;
    graph.isTerminalPointMovable = function (cell) {
        return oldTerminalPointMovable.apply(this, arguments) &&
            ui.getPermission().editEdges;
    };

    var oldBendable = graph.isCellBendable;
    graph.isCellBendable = function (cell) {
        return oldBendable.apply(this, arguments) &&
            ui.getPermission().editEdges;
    };

    var oldLabelMovable = graph.isLabelMovable;
    graph.isLabelMovable = function (cell) {
        return oldLabelMovable.apply(this, arguments) &&
            ui.getPermission().editEdges;
    };

    var oldMovable = graph.isCellMovable;
    graph.isCellMovable = function (cell) {
        return oldMovable.apply(this, arguments) &&
            ui.getPermission().editVertices;
    };

    var oldResizable = graph.isCellResizable;
    graph.isCellResizable = function (cell) {
        return oldResizable.apply(this, arguments) &&
            ui.getPermission().editVertices;
    };

    var oldEditable = graph.isCellEditable;
    graph.isCellEditable = function (cell) {
        return oldEditable.apply(this, arguments) &&
            (this.getModel().isVertex(cell) &&
            ui.getPermission().editVertices) ||
            (this.getModel().isEdge(cell) &&
            ui.getPermission().editEdges);
    };

    var oldDeletable = graph.isCellDeletable;
    graph.isCellDeletable = function (cell) {
        if (typeof cell === 'undefined' || cell === null)
            cell = graph.getSelectionCell();
        return oldDeletable.apply(this, arguments) &&
            (this.getModel().isVertex(cell) &&
            ui.getPermission().editVertices) ||
            (this.getModel().isEdge(cell) &&
            ui.getPermission().editEdges);
    };

    var oldCloneable = graph.isCellCloneable;
    graph.isCellCloneable = function (cell) {
        return oldCloneable.apply(this, arguments) &&
            ui.getPermission().cloneCells;
    };


    /*
    // Installs context menu
    graph.popupMenuHandler.factoryMethod = mxUtils.bind(this, function (menu, cell, evt) {
    this.menus.createPopupMenu(menu, cell, evt);
    });
    */

    // Initializes the outline
    editor.outline.init(this.outlineContainer);

    // Hides context menu
    mxEvent.addGestureListeners(document, mxUtils.bind(this, function (evt) {
        graph.popupMenuHandler.hideMenu();
    }));

    // Adds gesture handling (pinch to zoom)
    if (mxClient.IS_TOUCH) {
        mxEvent.addListener(graph.container, 'gesturechange',
            mxUtils.bind(this, function (evt) {
                graph.view.getDrawPane().setAttribute('transform', 'scale(' + evt.scale + ')');
                graph.view.getOverlayPane().style.visibility = 'hidden';
                mxEvent.consume(evt);
            })
        );

        mxEvent.addListener(graph.container, 'gestureend',
            mxUtils.bind(this, function (evt) {
                graph.view.getDrawPane().removeAttribute('transform');
                graph.view.setScale(graph.view.scale * evt.scale);
                graph.view.getOverlayPane().style.visibility = 'visible';
                mxEvent.consume(evt);
            })
        );

        // Disables pinch to resize
        graph.handleGesture = function () {
            // do nothing
        };
    }

    // Create handler for key events
    var keyHandler = this.createKeyHandler(editor);

    // Getter for key handler
    this.getKeyHandler = function () {
        return keyHandler;
    };

    // Updates the editor UI after the window has been resized
    mxEvent.addListener(window, 'resize', mxUtils.bind(this, function () {
        this.refresh();
        graph.sizeDidChange();
        this.editor.outline.update(false);
        this.editor.outline.outline.sizeDidChange();
    }));

    //DEV:add go back for dept detail
    /*
    var btnback = mxUtils.button(mxResources.get('goback'), function () {
    ui.showMainDocument();
    });
    btnback.id = 'btnback';
    btnback.style.position = 'absolute';
    btnback.style.position = 'absolute';
    btnback.style.zIndex = '3';
    btnback.style.display = 'none';
    btnback.style.left = '230px';
    btnback.style.top = '70px';
    ui.goBackButton = btnback;
    ui.container.appendChild(btnback);
    */

    // Updates action and menu states
    this.init();
    //this.open();

    //add title elem
    var spanTitle = document.createElement('span');
    spanTitle.setAttribute('id', 'titletruc');
    spanTitle.setAttribute('class', 'titletruc');

    document.getElementsByTagName('body')[0].appendChild(spanTitle);

    //DEV:call init height    
    setTimeout(function () {
        ui.callTrigger('resize', window);
    }, 500);

};

// Extends mxEventSource
mxUtils.extend(EditorUi, mxEventSource);


EditorUi.prototype.lastParentId = undefined;
EditorUi.prototype.currentId = undefined;
EditorUi.prototype.lastSource = undefined;
EditorUi.prototype.lastTarget = undefined;
EditorUi.prototype.lastWorkDocument = undefined;
EditorUi.prototype.lastSubWorkValue = undefined;
EditorUi.prototype.newSubWorkIds = [];
EditorUi.prototype.objEncrypt = undefined;
EditorUi.prototype.actionNode = [];

EditorUi.prototype.getActionFile = function (idmap) {
    if (typeof idmap === 'undefined') {
        var dataid = '0';
        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');

        idmap = this.editor.getOrCreateFilename() + '*' + dataid;
    }
    var arr = EditorUi.prototype.actionNode;
    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id.toString() === idmap.toString()) {
                return arr[i];
            }
        }
    }

    if (arr.length > 0)
        return arr[0];
}

EditorUi.prototype.addActionNode = function (name) {
    var arr = EditorUi.prototype.actionNode;
    if (arr.length > 0) {
        var found = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id.toString() === name.toString()) {
                found = true;
                break;
            }
        }
        if (found === false)
            EditorUi.prototype.actionNode.push({ id: name, arr: { ins: [], del: [], upd: [] } });
    }
    else
        EditorUi.prototype.actionNode.push({ id: name, arr: { ins: [], del: [], upd: [] } });
}

EditorUi.prototype.resetActionNode = function (objectSave, isAdd) {
    if (typeof objectSave === 'undefined')
        objectSave = this.getActionFile();
    //console.log(objectSave);
    if (typeof objectSave !== 'undefined') {
        objectSave.arr = { ins: [], del: [], upd: [] };
    }
    if (typeof isAdd !== 'undefined' && isAdd === true) {
        var roots = this.editor.graph.getModel().root.children[0].children;
        if (roots !== null && roots.length > 0) {
            for (var i = 0; i < roots.length; i++)
                objectSave.arr.ins.push(roots[i]);
        }
    }
}

EditorUi.prototype.removeActionNode = function (id) {
    //console.log('id', id);
    var arr = EditorUi.prototype.actionNode;
    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id.toString() === id.toString()) {
                arr.splice(i, 1);
                break;
            }
        }
    }
}

EditorUi.prototype.saveActionNode = function (node, type) {
   // if (this.checkEnable() === false)
        return;
    //console.log(node, type);
    var objectSave = this.getActionFile();

    if (typeof type !== 'undefined' && type.length > 0
        && typeof objectSave !== 'undefined') {

        switch (type) {
            case 'insert':
                /*#region insert*/

                if (node.isEdge()) {
                    if (node.source !== null && node.target !== null) {
                        var found = false;
                        /*#region find from recent add*/

                        for (var i = 0; i < objectSave.arr.ins.length; i++) {
                            if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                                found = true;
                                break;
                            }
                        }

                        /*#endregion*/

                        if (found === false) {

                            var foundsource = false, foundtarget = false;

                            /*#region find from recent add*/

                            for (var i = 0; i < objectSave.arr.ins.length; i++) {
                                if (objectSave.arr.ins[i].id.toString() === node.source.id.toString()) {
                                    foundsource = true;
                                    objectSave.arr.ins[i] = node.source;
                                    break;
                                }
                            }
                            for (var i = 0; i < objectSave.arr.ins.length; i++) {
                                if (objectSave.arr.ins[i].id.toString() === node.target.id.toString()) {
                                    foundtarget = true;
                                    objectSave.arr.ins[i] = node.target;
                                    break;
                                }
                            }

                            /*#endregion*/

                            /*#region find from recent update*/

                            for (var i = 0; i < objectSave.arr.upd.length; i++) {
                                if (foundsource === true && foundtarget === true)
                                    break;
                                if (objectSave.arr.upd[i].id.toString() === node.source.id.toString()) {
                                    objectSave.arr.upd[i] = node.source;
                                    foundsource = true;
                                    continue;
                                }
                                else if (objectSave.arr.upd[i].id.toString() === node.target.id.toString()) {
                                    objectSave.arr.upd[i] = node.target;
                                    foundtarget = true;
                                    continue;
                                }
                            }

                            /*#endregion*/

                            /*#region if not found*/
                            //connect to itself
                            if (node.source.id.toString() === node.target.id.toString()) {
                                objectSave.arr.upd.push(node.source);
                            }
                            else {
                                if (foundsource === false)
                                    objectSave.arr.upd.push(node.source);
                                if (foundtarget === false)
                                    objectSave.arr.upd.push(node.target);
                            }
                            /*#endregion*/

                            objectSave.arr.ins.push(node);
                        }
                    }
                    //else
                    //console.log('not');
                }
                else {
                    if (node.isSubWork === 1 || node.isSubWork === '1') {
                        if (node.value.length > 0) {
                            var found = false;

                            /*#region find from recent add*/

                            for (var i = 0; i < objectSave.arr.ins.length; i++) {
                                if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                                    found = true;
                                    break;
                                }
                            }

                            /*#endregion*/

                            /*#region find from recent update*/

                            if (found === false) {
                                for (var i = 0; i < objectSave.arr.upd.length; i++) {
                                    if (objectSave.arr.upd[i].id.toString() === node.id.toString()) {
                                        found = true;
                                        objectSave.arr.upd[i] = node;
                                        break;
                                    }
                                }
                            }

                            /*#endregion*/

                            if (found === false)
                                objectSave.arr.ins.push(node);
                        }
                    }
                    else {
                        var found = false;

                        /*#region find from recent add*/

                        for (var i = 0; i < objectSave.arr.ins.length; i++) {
                            if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                                found = true;
                                break;
                            }
                        }

                        /*#endregion*/

                        /*#region find from recent update*/

                        if (found === false) {
                            for (var i = 0; i < objectSave.arr.upd.length; i++) {
                                if (objectSave.arr.upd[i].id.toString() === node.id.toString()) {
                                    found = true;
                                    objectSave.arr.upd[i] = node;
                                    break;
                                }
                            }
                        }

                        /*#endregion*/

                        if (found === false)
                            objectSave.arr.ins.push(node);
                    }
                }

                /*#endregion*/
                break;
            case 'delete':
                var found = false;

                /*#region find from recent add*/

                for (var i = 0; i < objectSave.arr.ins.length; i++) {
                    if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                        found = true;
                        objectSave.arr.ins.splice(i, 1);
                        break;
                    }
                }

                /*#endregion*/

                if (found === true) {
                }
                else {
                    objectSave.arr.del.push(node);

                    if (node.isEdge()) {
                        if (node.source !== null && node.target !== null) {
                            var foundsource = false, foundtarget = false;
                            for (var i = 0; i < objectSave.arr.upd.length; i++) {
                                if (foundsource === true && foundtarget === true)
                                    break;
                                if (objectSave.arr.upd[i].id.toString() === node.source.id.toString()) {
                                    objectSave.arr.upd[i] = node.source;
                                    foundsource = true;
                                    continue;
                                }
                                else if (objectSave.arr.upd[i].id.toString() === node.target.id.toString()) {
                                    objectSave.arr.upd[i] = node.target;
                                    foundtarget = true;
                                    continue;
                                }
                            }

                            //connect to itself
                            if (node.source.id.toString() === node.target.id.toString()) {
                                objectSave.arr.upd.push(node.source);
                            }
                            else {
                                if (foundsource === false)
                                    objectSave.arr.upd.push(node.source);
                                if (foundtarget === false)
                                    objectSave.arr.upd.push(node.target);
                            }
                        }
                    }
                }
                break;
            case 'update':
                if (node.isSubWork === 1 || node.isSubWork === '1') {
                    if (node.value.length > 0) {
                        var found = false;
                        for (var i = 0; i < objectSave.arr.ins.length; i++) {
                            if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                                found = true;
                                objectSave.arr.ins[i] = node;
                                break;
                            }
                        }

                        if (found === false) {
                            for (var i = 0; i < objectSave.arr.upd.length; i++) {

                                if (objectSave.arr.upd[i].id.toString() === node.id.toString()) {
                                    objectSave.arr.upd[i] = node;
                                    found = true;
                                    break;
                                }
                            }
                            if (found === false)
                                objectSave.arr.upd.push(node);
                        }
                    }
                }
                else {
                    var found = false;
                    for (var i = 0; i < objectSave.arr.ins.length; i++) {
                        if (objectSave.arr.ins[i].id.toString() === node.id.toString()) {
                            found = true;
                            objectSave.arr.ins[i] = node;
                            break;
                        }
                    }

                    if (found === false) {
                        for (var i = 0; i < objectSave.arr.upd.length; i++) {

                            if (objectSave.arr.upd[i].id.toString() === node.id.toString()) {
                                objectSave.arr.upd[i] = node;
                                found = true;
                                break;
                            }
                        }
                        if (found === false)
                            objectSave.arr.upd.push(node);
                    }
                }
            default:
                break;
        }

    }
}

EditorUi.prototype.convertToObjSave = function (obj) {
    var st = obj.getStyle();
    var linetype = (st && st.length > 0 && st.indexOf(SweetSoftScript.Setting.colorSubLine) >= 0) ? 'DuongChuyenPhu' : 'DuongChuyenChinh';
    var cell = {
        id: obj.id,
        dataId: obj.dataId || '',
        dataGraphId: obj.dataGraphId || '',
        dataCode: obj.dataCode || '',
        value: obj.value || '',
        isEdge: obj.isEdge(),
        linetype: obj.isEdge() ? linetype : '',
        from: '',
        to: '',
        isSubWork: (typeof obj.isSubWork !== 'undefined' && obj.isSubWork.toString().toLowerCase() === '1' ? true : false) || false,
        isSource: (typeof obj.isSource !== 'undefined' && obj.isSource.toString().toLowerCase() === '1' ? true : false) || false,
        isTarget: (typeof obj.isTarget !== 'undefined' && obj.isTarget.toString().toLowerCase() === '1' ? true : false) || false,
        //source: (typeof obj.source !== 'undefined' && obj.source !== null ? this.convertToObjSave(obj.source) : undefined),
        //target: (typeof obj.target !== 'undefined' && obj.target !== null ? this.convertToObjSave(obj.target) : undefined)
        sourceid: (typeof obj.source !== 'undefined' && obj.source !== null) ? obj.source.id : 0,
        targetid: (typeof obj.target !== 'undefined' && obj.target !== null) ? obj.target.id : 0
    };

    return cell;
}

EditorUi.prototype.convertActionNode = function () {
    var objectSave = this.getActionFile();
    var dataReturn = { id: objectSave.id, arr: { ins: [], upd: [], del: [] } };

    if (typeof objectSave !== 'undefined' && objectSave.arr !== 'undefined') {
        if (objectSave.arr.ins.length > 0) {
            for (var i = 0; i < objectSave.arr.ins.length; i++) {
                var obj = objectSave.arr.ins[i];
                var item = this.convertToObjSave(obj);
                if (obj.edges !== null && obj.edges.length > 0) {
                    var from = '', to = '';
                    for (var k = 0; k < obj.edges.length; k++) {
                        if (typeof obj.edges[k].source !== 'undefined' && obj.edges[k].source !== null
                            && typeof obj.edges[k].target !== 'undefined' && obj.edges[k].target !== null) {
                            if (obj.edges[k].source.id.toString() === obj.edges[k].target.id.toString()) {
                                to += obj.edges[k].source.id.toString() + ',';
                                from += obj.edges[k].source.id.toString() + ',';
                            }
                            else {
                                if (obj.edges[k].source.id.toString() === item.id.toString())
                                    to += obj.edges[k].target.id.toString() + ',';
                                else if (obj.edges[k].target.id.toString() === item.id.toString())
                                    from += obj.edges[k].source.id.toString() + ',';
                            }
                        }
                    }

                    if (from.length > 0)
                        from = from.substring(0, from.length - 1);
                    if (to.length > 0)
                        to = to.substring(0, to.length - 1);

                    item.from = from;
                    item.to = to;
                }


                dataReturn.arr.ins.push(item);
            }
        }
        if (objectSave.arr.upd.length > 0) {
            for (var i = 0; i < objectSave.arr.upd.length; i++) {
                var obj = objectSave.arr.upd[i];
                var item = this.convertToObjSave(obj);
                if (obj.edges !== null && obj.edges.length > 0) {
                    var from = '', to = '';
                    for (var k = 0; k < obj.edges.length; k++) {

                        if (typeof obj.edges[k].source !== 'undefined' && obj.edges[k].source !== null
                           && typeof obj.edges[k].target !== 'undefined' && obj.edges[k].target !== null) {
                            if (obj.edges[k].source.id.toString() === obj.edges[k].target.id.toString()) {
                                to += obj.edges[k].source.id.toString() + ',';
                                from += obj.edges[k].source.id.toString() + ',';
                            }
                            else {
                                if (obj.edges[k].source.id.toString() === item.id.toString())
                                    to += obj.edges[k].target.id.toString() + ',';
                                else if (obj.edges[k].target.id.toString() === item.id.toString())
                                    from += obj.edges[k].source.id.toString() + ',';
                            }
                        }
                    }

                    if (from.length > 0)
                        from = from.substring(0, from.length - 1);
                    if (to.length > 0)
                        to = to.substring(0, to.length - 1);

                    item.from = from;
                    item.to = to;
                }

                dataReturn.arr.upd.push(item);
            }
        }
        if (objectSave.arr.del.length > 0) {
            for (var i = 0; i < objectSave.arr.del.length; i++) {
                var obj = objectSave.arr.del[i];
                var item = this.convertToObjSave(obj);

                if (obj.edges !== null && obj.edges.length > 0) {
                    var from = '', to = '';
                    for (var k = 0; k < obj.edges.length; k++) {
                        if (typeof obj.edges[k].source !== 'undefined' && obj.edges[k].source !== null
                           && typeof obj.edges[k].target !== 'undefined' && obj.edges[k].target !== null) {
                            if (obj.edges[k].source.id.toString() === obj.edges[k].target.id.toString()) {
                                to += obj.edges[k].source.id.toString() + ',';
                                from += obj.edges[k].source.id.toString() + ',';
                            }
                            else {
                                if (obj.edges[k].source.id.toString() === item.id.toString())
                                    to += obj.edges[k].target.id.toString() + ',';
                                else if (obj.edges[k].target.id.toString() === item.id.toString())
                                    from += obj.edges[k].source.id.toString() + ',';
                            }
                        }
                    }

                    if (from.length > 0)
                        from = from.substring(0, from.length - 1);
                    if (to.length > 0)
                        to = to.substring(0, to.length - 1);

                    item.from = from;
                    item.to = to;
                }
            }

            dataReturn.arr.del.push(item);
        }
    }


    return dataReturn;
}

//DEV:save last click line
EditorUi.prototype.lastClickEdge = undefined;
EditorUi.prototype.lastSourceAndTarget = undefined;

//DEV:save last click element
EditorUi.prototype.lastClickElement = undefined;
/**
* Specifies the size of the split bar.
*/
EditorUi.prototype.splitSize = (mxClient.IS_TOUCH || mxClient.IS_POINTER) ? 12 : 8;

//DEV:process open work
//cell is edge
EditorUi.prototype.IdOfLineMap = undefined;
EditorUi.prototype.processOpenWork = function (cell) {
    var so = cell.source;
    var ta = cell.target;
    //console.log(ta, ta.dataId);
    if (ta !== null && typeof ta !== 'undefined'
        && so.dataId !== null && typeof so.dataId !== 'undefined') {
        EditorUi.prototype.IdOfLineMap = cell.id;

        //this.sidebar.hidePaletteByIndex(0);
        this.sidebar.hideAllPalette();

        var arr = this.externalType;
        if (arr !== null && typeof arr !== 'undefined') {
            var isShowTruc = true;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].sid.toString() === so.dataId.toString()
                    /*&& arr[i].eid.toString() === ta.dataId.toString()*/) {
                    //not show
                    //this.sidebar.showMachinery('graphictype');
                    isShowTruc = false;
                    break;
                }
            }
            if (isShowTruc === true)
                this.sidebar.showMachinery('truc');
        }
        else
            this.sidebar.showMachinery('truc');

        //tab item left
        var idload = '';
        if (typeof ta.dataId === 'undefined')
            idload = so.dataId;
        else
            idload = ta.dataId;
        this.sidebar.showPalette('workas' + idload + '-' + so.dataId);
        this.sidebar.setHeightVisiblePalette();
        //draw in right

        //set active machinery
        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine === null)
            this.sidebar.setActiveMachineryByIndex(0);

        //EditorUi.prototype.lastParentId = '0';
        EditorUi.prototype.lastSource = so;
        EditorUi.prototype.lastTarget = ta;

        this.processLoadDraw(ta, so);

        //show subwork item
        if (this.toolbar.itemSubWork !== null)
            this.toolbar.itemSubWork.style.display = 'block';
        return;
    }
};

EditorUi.prototype.productionPropertyWindows = [];
EditorUi.prototype.productionPropertyNodes = [];

EditorUi.prototype.getVisibleProductionPropertyWindow = function () {
    var arr = EditorUi.prototype.productionPropertyWindows;
    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].window.isVisible())
                return arr[i];
        }
    }
    return undefined;
}

EditorUi.prototype.getProductionPropertyWindow = function () {
    var iddept = '';
    var palettes = this.sidebar.palettes;

    if (typeof palettes !== 'undefined' && palettes !== null) {
        for (var key in palettes) {
            //var obj = pals[key];

            if (key === 'depts')
                continue;
            var elts = palettes[key];

            if (elts !== null) {
                if (elts[0].style.display !== 'none') {
                    iddept = key;
                    break;
                }
            }
        }
    }


    if (iddept.length > 0) {
        var arr = EditorUi.prototype.productionPropertyWindows;
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].id.toString() === iddept.toString())
                    return arr[i];
            }
        }
    }

    return undefined;
}


EditorUi.prototype.getProductionProperty = function (idnode) {
    var arr = EditorUi.prototype.productionPropertyNodes;
    if (arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id.toString() === idnode.toString())
                return arr[i];
        }
    }
    return undefined;
}

EditorUi.prototype.loadProductionProperty = function (code) {
    document.getElementById('page_spinner').style.display = 'block';
    var onload = function (req) {
        var data = req.getText();

        if (typeof data !== 'undefined' && data.length > 0) {

        }
        else {

        }
    }

    var onerror = function (req) {
        console.log(req.getStatus());
        document.getElementById('page_spinner').style.display = 'none';
    }

    new mxXmlRequest(this.loadTrucPath, 'IdTruc=' + id + '&idparent=0').send(onload, onerror);
}

EditorUi.prototype.WorkPropertiesMapping = [];

EditorUi.prototype.saveInfoWork = function (cell, data) {
    var dataid = '0';

    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    if (typeof dataid !== 'undefined') {
        //for load data cell
        var code = EditorUi.prototype.lastWorkDocument;
        if (typeof code === 'undefined' || code.length === 0)
            code = 'Main-Work';
        var arr = EditorUi.prototype.WorkPropertiesMapping;
        if (typeof arr === 'undefined' || arr === null)
            arr = [];
        if (arr.length > 0) {
            var found = false;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].id.toString() === cell.id.toString() && arr[i].datadesign === code + '*' + dataid) {
                    arr[i].datavalue = data;

                    arr[i].datavalue.id = '0';
                    //if (arr[i].datavalue.id !== '0')
                    //    this.processSaveInfoCell(arr[i]);
                    found = true;
                    break;
                }
            }
            if (found === false)
                arr.push({ id: cell.id.toString(), datadesign: code + '*' + dataid, datavalue: data });
        }
        else
            arr.push({ id: cell.id.toString(), datadesign: code + '*' + dataid, datavalue: data });
        EditorUi.prototype.WorkPropertiesMapping = arr;
    }
}


EditorUi.prototype.loadInfoWork = function (cell, callback) {
    if (this.loadCellInfoPath.length > 0) {
        var dataid = '0';

        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');

        if (typeof dataid !== 'undefined') {
            //for load data cell
            var code = EditorUi.prototype.lastWorkDocument;
            if (typeof code === 'undefined' || code.length === 0)
                code = 'Main-Work';
            var ed = this;
            var onload = function (req) {
                var data = req.getText();
                if (data !== null && typeof data !== 'undefined' && data.length > 0) {
                    var parseData;
                    try {
                        parseData = JSON.parse(data);
                        if (typeof parseData !== 'undefined') {


                            ed.saveInfoWork(cell, parseData);

                            if (typeof callback === 'function') {
                                callback(parseData);
                            }
                        }
                    }
                    catch (ex) {
                        console.log('error parse : ', ex);
                    }
                }
                else if (typeof callback === 'function') {
                    callback();
                }
            }
            var onerror = function (req) {
                console.log(req.getStatus());
            }
            new mxXmlRequest(this.loadCellInfoPath, 'idcell=' +
                cell.id + '&idtruc=' + dataid + '&filename=' + code + '&type=work').send(onload, onerror);
        }
    }
}

EditorUi.prototype.getInfoWork = function (cell) {

    var dataReturn;
    var dataid = '0';

    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    if (typeof dataid !== 'undefined') {

        //for load data cell
        var code = EditorUi.prototype.lastWorkDocument;
        if (typeof code === 'undefined' || code.length === 0)
            code = 'Main-Work';

        var arr = EditorUi.prototype.WorkPropertiesMapping;

        if (typeof arr !== 'undefined' && arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {

                if (arr[i].id.toString() === cell.id.toString() && arr[i].datadesign === code + '*' + dataid) {
                    dataReturn = arr[i].datavalue;
                    break;
                }
            }
        }
    }
    return dataReturn;
}

EditorUi.prototype.showProductionProperty = function (cell) {

    var infowindow = this.getProductionPropertyWindow();
    if (infowindow !== null && typeof infowindow !== 'undefined') {
        infowindow.window.show();

        //show mapped
        if (typeof cell === 'undefined' || cell === null)
            cell = EditorUi.prototype.lastClickElement;
        if (typeof cell !== 'undefined' && cell !== null) {
            var info = this.getInfoWork(cell);
            var div = $(infowindow.window.td).find('#divtemplate');
            if (div !== null) {
                var allcheck = div.find('input[type="checkbox"]:checked');
                if (allcheck.length > 0)
                    allcheck.prop('checked', false);
            }
            if (typeof info !== 'undefined' && info.arr.length > 0) {
                if (div !== null) {
                    //console.log(info);
                    if (info.updatepropertiesvalues && info.updatepropertiesvalues === true)
                        $('#chkghinhan').prop('checked', true);
                    else
                        $('#chkghinhan').prop('checked', false);

                    for (var i = 0; i < info.arr.length; i++) {
                        var inp = div.find('input[type="checkbox"][dataid="' + info.arr[i] + '"]');
                        if (inp.length > 0)
                            inp.prop('checked', true);
                    }
                }
                else
                    $('#chkghinhan').prop('checked', false);
            }
            else
                $('#chkghinhan').prop('checked', false);
        }

        var btn = document.getElementById('btnShowRecord');
        if (btn !== null) {
            var table = $(btn).closest('table');
            if (table.length > 0) {
                var h3title = $(infowindow.window.td).find('h3.title');
                if (h3title.length > 0) {
                    var spanName = table.find('tr.row:eq(0) .data > span');
                    var spanType = table.find('tr.row:eq(1) .data > span');
                    h3title.text(spanType.text() + ": " + spanName.text());
                }
            }
        }
    }
}

EditorUi.prototype.checkProductionProperty = function () {
    var infowindow = this.getProductionPropertyWindow();
    if (infowindow !== null && typeof infowindow !== 'undefined')
        return true;
    else
        return false;
}

//DEV:outline
EditorUi.prototype.createProductionProperty = function (id, data) {
    if (typeof data === 'undefined' || data === null)
        return;

    var ww = 400;
    var wh = 450;
    var ws = SweetSoftScript.mainFunction.getWindowSize();
    var pl = ws.width - ww - 30;
    /*
    var pt = ws.height - wh - 35;
    if (pt < 0)
    pt = 0;
    */
    var pt = 0;
    //mxWindow(title, content, x, y, width, height, minimizable, movable, replaceNode, style)
    var divmain = document.createElement('div');
    divmain.className = 'divmain';

    var h3Title = document.createElement('h3');
    h3Title.className = 'title';
    divmain.appendChild(h3Title);

    var labelchk = document.createElement('label');
    labelchk.setAttribute('for', 'chkghinhan');

    var inputCheckState = document.createElement('input');
    inputCheckState.setAttribute('type', 'checkbox');
    inputCheckState.className = 'chkstate';
    inputCheckState.setAttribute('id', 'chkghinhan');
    labelchk.appendChild(inputCheckState);

    var spanstate = document.createElement('span');
    $(spanstate).text(SweetSoftScript.ResourceText.ghinhanThongSo);
    labelchk.appendChild(spanstate);

    divmain.appendChild(labelchk);

    var divfixed = $('#divtemplate').clone();
    divfixed.removeAttr('style');
    var bodyContent = '';
    for (var i = 0; i < data.length; i++) {
        bodyContent += '<tr><td><label><input type="checkbox" dataid="' + data[i].id + '" /></td><td>' +
            (i + 1) + '</td><td>' + data[i].name + '</td><td>' +
            data[i].type + '</td></tr>';
    }
    divfixed.find('tbody').html(bodyContent);

    divmain.appendChild(divfixed[0]);

    var divclear = document.createElement('div');
    divclear.className = 'clear';
    divmain.appendChild(divclear);
    var ed = this;

    var chkall = $(divmain).find('table input[type="checkbox"]');
    if (chkall.length > 0) {
        chkall.change(function () {
            var countCheck = $(this).closest('table').find('input[type="checkbox"]:checked');
            if (countCheck.length > 0) {
                //$('#chkghinhan').prop('checked', true);
            }
            else
                $('#chkghinhan').prop('checked', false);
        });
    }

    //permissison
    if (ed.checkEnable() === true) {
        var btnchkall = mxUtils.button(SweetSoftScript.ResourceText.btnCheckAll, function () {
            divfixed.find('table').find('td > label > input[type="checkbox"]').prop('checked', true);
        });
        btnchkall.className = 'button first';
        divmain.appendChild(btnchkall);

        var btnunchkall = mxUtils.button(SweetSoftScript.ResourceText.btnUnCheckAll, function () {
            divfixed.find('table').find('td > label  > input[type="checkbox"]').prop('checked', false);
        });
        btnunchkall.className = 'button marginlr';
        divmain.appendChild(btnunchkall);
        var btnSaveRecord = mxUtils.button(SweetSoftScript.ResourceText.btnSaveRecord, function () {
            var chkColl = divfixed.find('table').find('td > label > input[type="checkbox"]:checked');
            var selectedList = [];
            var dataCellInfo = ed.getInfoWork(EditorUi.prototype.lastClickElement);
            if (chkColl.length > 0) {
                if (typeof dataCellInfo === 'undefined')
                    dataCellInfo = { id: '0', arr: [], updatepropertiesvalues: false };
                chkColl.each(function (i, o) {
                    selectedList.push(o.getAttribute('dataid'));
                });
            }
            dataCellInfo.updatepropertiesvalues = $('#chkghinhan').prop('checked');
            dataCellInfo.arr = selectedList;


            //save infocell
            ed.saveInfoWork(EditorUi.prototype.lastClickElement, dataCellInfo);

            wnddesign.hide();
            chkColl.prop('checked', false);

            return false;

        });
        btnSaveRecord.className = 'button';
        divmain.appendChild(btnSaveRecord);
    }

    var wnddesign = new mxWindow(SweetSoftScript.ResourceText.ghinhanThongSo, divmain, pl, pt, ww, wh, true, true);

    //wnddesign.setVisible(false);
    wnddesign.setResizable(false);
    wnddesign.setClosable(true);
    //wnddesign.setLocation(x,y);

    wnddesign.destroyOnClose = false;
    wnddesign.setMaximizable(false);
    wnddesign.setMinimizable(false);
    //wnddesign.setTitle(title);

    wnddesign.addListener(mxEvent.HIDE, function (e) {
        wnddesign.div.style.zIndex = '-1';
    });
    wnddesign.addListener(mxEvent.SHOW, function (e) {
        wnddesign.div.style.zIndex = '30';
    });

    wnddesign.addListener(mxEvent.ACTIVATE, function (e) {
        //wnddesign.div.style.zIndex = '1';

        if (ed.editor.outlineWindow.isVisible() === true)
            ed.editor.outlineWindow.div.style.zIndex = '1';
    });


    var arr = EditorUi.prototype.productionPropertyWindows;
    if (typeof arr === 'undefined' || arr === null)
        arr = [];

    arr.push({ id: id, window: wnddesign });
    EditorUi.prototype.productionPropertyWindows = arr;
}

/*DEV:show work*/
EditorUi.prototype.showWork = function (cell) {
    if (this.editor.isShowWork === true) {

        return;
    }
    else {

    }

    this.editor.isShowWork = true;


    var so = null, ta = null;
    for (var cellItem in this.editor.graph.model.cells) {

        if (so !== null && ta !== null)
            break;


        if (this.editor.graph.model.cells[cellItem].isSource === 1) {

            so = this.editor.graph.model.cells[cellItem];
            continue;
        }

        if (this.editor.graph.model.cells[cellItem].isTarget === 1) {

            ta = this.editor.graph.model.cells[cellItem];
            continue;
        }
    }

    var data = {
        isEdge: typeof cell !== 'undefined' ? cell.isEdge() : true,
        targetValue: ta !== null ? ta.value : '',
        sourceValue: so !== null ? so.value : '',
        isMain: typeof cell !== 'undefined' ? (cell.isSource || cell.isTarget) : false,
        isSubWork: typeof cell !== 'undefined' ? cell.isSubWork : false,
        value: typeof cell !== 'undefined' ? cell.value : ''
    };

    this.editor.displayInfoData(data);
};
/**
* Specifies the height of the menubar. Default is 34.
*/
//DEV:hide menu bar
EditorUi.prototype.menubarHeight = 0;

/**
* Specifies the height of the toolbar. Default is 36.
*/
EditorUi.prototype.toolbarHeight = 34;

/**
* Specifies the height of the footer. Default is 28.
*/
EditorUi.prototype.footerHeight = 28;

/**
* Specifies the height of the optional sidebarFooterContainer. Default is 34.
*/
EditorUi.prototype.sidebarFooterHeight = 34;

/**
* Specifies the height of the horizontal split bar. Default is 212.
*/
EditorUi.prototype.hsplitPosition = 204;

/**
* Specifies the position of the vertical split bar. Default is 190.
*/
EditorUi.prototype.vsplitPosition = 190;

/**
* Specifies if animations are allowed in <executeLayout>. Default is true.
*/
EditorUi.prototype.allowAnimation = true;

/**
* Installs the listeners to update the action states.
*/
EditorUi.prototype.init = function () {
    // Updates action states
    this.addUndoListener();
    this.addSelectionListener();
    this.addBeforeUnloadListener();

    // Overrides clipboard to update paste action state
    if (typeof Actions !== 'undefined') {
        var paste = this.actions.get('paste');

        var updatePaste = mxUtils.bind(this, function () {
            paste.setEnabled(this.editor.graph.cellEditor.isContentEditing() || !mxClipboard.isEmpty());
        });


        var mxClipboardCut = mxClipboard.cut;
        mxClipboard.cut = function (graph) {
            if (graph.cellEditor.isContentEditing()) {
                document.execCommand('cut');
            }
            else {
                mxClipboardCut.apply(this, arguments);
            }

            updatePaste();
        };

        var mxClipboardCopy = mxClipboard.copy;
        mxClipboard.copy = function (graph) {
            if (graph.cellEditor.isContentEditing()) {
                document.execCommand('copy');
            }
            else {
                mxClipboardCopy.apply(this, arguments);
            }

            updatePaste();
        };

        var mxClipboardPaste = mxClipboard.paste;
        mxClipboard.paste = function (graph) {
            if (graph.cellEditor.isContentEditing()) {
                document.execCommand('paste');
            }
            else {
                mxClipboardPaste.apply(this, arguments);
            }

            updatePaste();
        };

        // Overrides cell editor to update paste action state
        var cellEditorStartEditing = this.editor.graph.cellEditor.startEditing;

        this.editor.graph.cellEditor.startEditing = function () {
            cellEditorStartEditing.apply(this, arguments);
            updatePaste();
        };

        var cellEditorStopEditing = this.editor.graph.cellEditor.stopEditing;

        this.editor.graph.cellEditor.stopEditing = function (cell, trigger) {
            cellEditorStopEditing.apply(this, arguments);
            updatePaste();
        };

        updatePaste();
    }

};


EditorUi.prototype.isNewSubwork = function (id) {

    var arr = EditorUi.prototype.newSubWorkIds;
    if (typeof arr !== undefined && arr !== null && arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].toString() === id.toString())
                return true;
        }
    }
    return false;
}

EditorUi.prototype.addNewSubwork = function (id) {
    var arr = EditorUi.prototype.newSubWorkIds;
    if (typeof arr === 'undefined' || arr === null)
        arr = [id];
    else
        arr.push(id);
    EditorUi.prototype.newSubWorkIds = arr;
}

EditorUi.prototype.checkEnable = function () {
    return true;
}

EditorUi.prototype.getPermission = function () {
    if (this.checkEnable())
        return new Permission();
    else
        return new Permission(true, false, false, false, false);
}

/**
* Hook for allowing selection and context menu for certain events.
*/
EditorUi.prototype.isSelectionAllowed = function (evt) {
    return false;
};

/**
* Installs dialog if browser window is closed without saving
* This must be disabled during save and image export.
*/
EditorUi.prototype.addBeforeUnloadListener = function () {
    // Installs dialog if browser window is closed without saving
    // This must be disabled during save and image export
    window.onbeforeunload = mxUtils.bind(this, function () {
        return this.onBeforeUnload();
    });
};

/**
* Sets the onbeforeunload for the application
*/
EditorUi.prototype.onBeforeUnload = function () {
    /*old
    if (this.editor.modified) {
    return mxResources.get('allChangesLost');
    }
    */

    //new
    var objectSave = this.getActionFile();

    if (typeof objectSave !== 'undefined') {
        if (objectSave.arr.ins.length > 0 ||
            objectSave.arr.del.length > 0 ||
            objectSave.arr.upd.length > 0) {
            return SweetSoftScript.ResourceText.confirmLostData;
            //return mxResources.get('allChangesLost');
        } /*
        else {
            if (this.editor.modified) {
                this.save(this.editor.getOrCreateFilename(), false);
            }
        }*/
    } /*
    else {
        if (this.editor.modified) {
            this.save(this.editor.getOrCreateFilename(), false);
        }
    }*/
};
EditorUi.prototype.callTrigger = function (eventName, element) {
    var event; // The custom event that will be created    
    if (document.createEvent) {
        event = document.createEvent("HTMLEvents");
        event.initEvent(eventName, true, true);
    } else {
        event = document.createEventObject();
        event.eventType = eventName;
    }

    event.eventName = eventName;

    if (document.createEvent) {
        element.dispatchEvent(event);
    } else {
        element.fireEvent("on" + event.eventType, event);
    }
}

/**
* Opens the current diagram via the window.opener if one exists.
*/
EditorUi.prototype.open = function () {
    // Cross-domain window access is not allowed in FF, so if we
    // were opened from another domain then this will fail.
    try {
        if (window.opener != null && window.opener.openFile != null) {
            window.opener.openFile.setConsumer(mxUtils.bind(this, function (xml, filename) {
                try {
                    var doc = mxUtils.parseXml(xml);
                    this.editor.setGraphXml(doc.documentElement);
                    this.editor.setModified(false);
                    this.editor.undoManager.clear();

                    if (filename != null) {
                        this.editor.setFilename(filename);
                        this.updateDocumentTitle();
                    }
                }
                catch (e) {
                    SweetSoftScript.mainFunction.OpenRadWindow(undefined, mxResources.get('invalidOrMissingFile') + ': ' + e.message, 'alert');
                }
            }));
        }
    }
    catch (e) {
        // ignore
    }
};

/**
* Updates the document title.
*/
EditorUi.prototype.updateDocumentTitle = function (title) {
    if (typeof title === 'undefined')
        title = this.editor.getOrCreateFilename();
    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        title = $.trim($(activeMachine).text()) + ' - ' + title;
    title = title.replace(/(\r\n|\n|\r)/gm, "");

    $(document.getElementById('titletruc')).text(title);

    if (this.editor.appName !== null) {
        title += ' - ' + this.editor.appName;
    }

    document.title = title;
};

EditorUi.prototype.isMainDocument = function () {
    var j = this.WorkDocument.length;
    if (j === 0)
        return true;
    else {
        var fileName = this.editor.getFilename();
        if (fileName !== null && typeof fileName !== 'undefined') {
            for (var i = 0; i < j; i++) {
                if (this.WorkDocument[i].id.toString() === fileName.toString()) {
                    return false;
                }
            }
        }
        return true;
    }
};
/**
* Returns the URL for a copy of this editor with no state.
*/
EditorUi.prototype.redo = function () {
    if (this.editor.graph.cellEditor.isContentEditing()) {
        document.execCommand('redo');
    }
    else {
        this.editor.graph.stopEditing(false);
        this.editor.undoManager.redo();
    }
};

/**
* Returns the URL for a copy of this editor with no state.
*/
EditorUi.prototype.undo = function () {
    if (this.editor.graph.cellEditor.isContentEditing()) {
        document.execCommand('undo');
    }
    else {
        this.editor.graph.stopEditing(false);
        this.editor.undoManager.undo();
    }
};

/**
* Returns the URL for a copy of this editor with no state.
*/
EditorUi.prototype.canRedo = function () {
    return this.editor.graph.cellEditor.isContentEditing() || this.editor.undoManager.canRedo();
};

/**
* Returns the URL for a copy of this editor with no state.
*/
EditorUi.prototype.canUndo = function () {
    return this.editor.graph.cellEditor.isContentEditing() || this.editor.undoManager.canUndo();
};

/**
* Returns the URL for a copy of this editor with no state.
*/
EditorUi.prototype.getUrl = function (pathname) {
    var href = (pathname != null) ? pathname : window.location.pathname;
    var parms = (href.indexOf('?') > 0) ? 1 : 0;

    // Removes template URL parameter for new blank diagram
    for (var key in urlParams) {
        if (parms == 0) {
            href += '?';
        }
        else {
            href += '&';
        }

        href += key + '=' + urlParams[key];
        parms++;
    }

    return href;
};

/**
* Loads the stylesheet for this graph.
*/
EditorUi.prototype.setBackgroundColor = function (value) {
    this.editor.graph.background = value;
    this.editor.updateGraphComponents();

    this.fireEvent(new mxEventObject('backgroundColorChanged'));
};

/**
* Loads the stylesheet for this graph.
*/
EditorUi.prototype.setPageFormat = function (value) {
    this.editor.graph.pageFormat = value;
    this.editor.outline.outline.pageFormat = this.editor.graph.pageFormat;

    if (!this.editor.graph.pageVisible) {
        this.actions.get('pageView').funct();
    }
    else {
        this.editor.updateGraphComponents();
        this.editor.graph.view.validateBackground();
        this.editor.graph.sizeDidChange();
        this.editor.outline.update();
    }

    this.fireEvent(new mxEventObject('pageFormatChanged'));
};


/**
* Updates the states of the given undo/redo items.
*/
EditorUi.prototype.addUndoListener = function () {
    if (typeof Actions !== 'undefined') {
        var undo = this.actions.get('undo');
        var redo = this.actions.get('redo');

        var undoMgr = this.editor.undoManager;

        var undoListener = mxUtils.bind(this, function () {
            undo.setEnabled(this.canUndo());
            redo.setEnabled(this.canRedo());
        });

        undoMgr.addListener(mxEvent.ADD, undoListener);
        undoMgr.addListener(mxEvent.UNDO, undoListener);
        undoMgr.addListener(mxEvent.REDO, undoListener);
        undoMgr.addListener(mxEvent.CLEAR, undoListener);

        // Overrides cell editor to update action states
        var cellEditorStartEditing = this.editor.graph.cellEditor.startEditing;

        this.editor.graph.cellEditor.startEditing = function () {
            cellEditorStartEditing.apply(this, arguments);
            undoListener();
        };

        var cellEditorStopEditing = this.editor.graph.cellEditor.stopEditing;

        this.editor.graph.cellEditor.stopEditing = function (cell, trigger) {
            cellEditorStopEditing.apply(this, arguments);
            undoListener();
        };

        // Updates the button states once
        undoListener();
    }
};

/**
* Updates the states of the given toolbar items based on the selection.
*/
EditorUi.prototype.addSelectionListener = function () {
    var selectionListener = mxUtils.bind(this, function () {
        var graph = this.editor.graph;
        var selected = !graph.isSelectionEmpty();
        var vertexSelected = false;
        var edgeSelected = false;

        var cells = graph.getSelectionCells();

        if (cells != null) {
            for (var i = 0; i < cells.length; i++) {
                var cell = cells[i];

                if (graph.getModel().isEdge(cell)) {
                    edgeSelected = true;
                }

                if (graph.getModel().isVertex(cell)) {
                    vertexSelected = true;
                }

                if (edgeSelected && vertexSelected) {
                    break;
                }
            }
        }

        // Updates action states
        var actions = ['cut', 'copy', 'bold', 'italic', 'underline', 'fontColor',
                   'delete', 'duplicate', 'style', 'fillColor', 'gradientColor', 'strokeColor',
                   'backgroundColor', 'borderColor', 'toFront', 'toBack', 'dashed', 'rounded',
                   'shadow', 'tilt', 'autosize', 'lockUnlock', 'editData'];

        if (typeof Actions !== 'undefined') {
            for (var i = 0; i < actions.length; i++) {
                var act = this.actions.get(actions[i]);
                if (act !== null && typeof act !== 'undefined')
                    act.setEnabled(selected);
            }
            /*
            this.actions.get('curved').setEnabled(edgeSelected);
            this.actions.get('rotation').setEnabled(vertexSelected);
            this.actions.get('wordWrap').setEnabled(vertexSelected);
            this.actions.get('group').setEnabled(graph.getSelectionCount() > 1);
            this.actions.get('ungroup').setEnabled(graph.getSelectionCount() == 1 &&
            graph.getModel().getChildCount(graph.getSelectionCell()) > 0);
            var oneVertexSelected = vertexSelected && graph.getSelectionCount() == 1;
            this.actions.get('removeFromGroup').setEnabled(oneVertexSelected &&
            graph.getModel().isVertex(graph.getModel().getParent(graph.getSelectionCell())));
            */
        }

        // Updates menu states
        var menus = [/*'fontFamily', 'fontSize', 'alignment',*/'position'/*, 'text', 'format'*/, 'linewidth',
                     'spacing'/*, 'gradient'*/];

        if (typeof Menus !== 'undefined') {
            for (var i = 0; i < menus.length; i++) {
                this.menus.get(menus[i]).setEnabled(selected);
            }
        }

        menus = ['line', 'lineend', 'linestart'];

        if (typeof Menus !== 'undefined') {
            for (var i = 0; i < menus.length; i++) {
                this.menus.get(menus[i]).setEnabled(edgeSelected);
            }
        }

        //if (typeof Actions !== 'undefined')
        //    this.actions.get('setAsDefaultEdge').setEnabled(edgeSelected);

        if (typeof Menus !== 'undefined') {
            this.menus.get('align').setEnabled(graph.getSelectionCount() > 1);
            this.menus.get('direction').setEnabled(vertexSelected || (edgeSelected &&
                    graph.isLoop(graph.view.getState(graph.getSelectionCell()))));
            this.menus.get('navigation').setEnabled(graph.foldingEnabled && ((graph.view.currentRoot != null) ||
                    (graph.getSelectionCount() == 1 && graph.isValidRoot(graph.getSelectionCell()))));
            this.menus.get('layers').setEnabled(graph.view.currentRoot == null);
        }
        if (typeof Actions !== 'undefined') {
            this.actions.get('home').setEnabled(graph.view.currentRoot != null);
            this.actions.get('exitGroup').setEnabled(graph.view.currentRoot != null);

            var groupEnabled = graph.getSelectionCount() == 1 && graph.isValidRoot(graph.getSelectionCell());
            this.actions.get('enterGroup').setEnabled(groupEnabled);
            this.actions.get('expand').setEnabled(groupEnabled);
            this.actions.get('collapse').setEnabled(groupEnabled);
            this.actions.get('editLink').setEnabled(graph.getSelectionCount() == 1);
            this.actions.get('openLink').setEnabled(graph.getSelectionCount() == 1 &&
                    graph.getLinkForCell(graph.getSelectionCell()) != null);
        }
    });

    this.editor.graph.getSelectionModel().addListener(mxEvent.CHANGE, selectionListener);
    selectionListener();
};

/**
* Refreshes the viewport.
*/
EditorUi.prototype.refresh = function () {
    var quirks = mxClient.IS_IE && (document.documentMode == null || document.documentMode == 5);
    var w = this.container.clientWidth;
    var h = this.container.clientHeight;

    if (this.container === document.body) {
        w = document.body.clientWidth || document.documentElement.clientWidth;
        h = (quirks) ? document.body.clientHeight || document.documentElement.clientHeight : document.documentElement.clientHeight;
    }

    var effHsplitPosition = Math.max(0, Math.min(this.hsplitPosition, w - this.splitSize - 20));
    var effVsplitPosition = Math.max(0, Math.min(this.vsplitPosition, h - this.menubarHeight - this.toolbarHeight - this.footerHeight - this.splitSize - 1));
    //DEV:hide menu bar
    //this.menubarContainer.style.height = this.menubarHeight + 'px';
    this.toolbarContainer.style.top = this.menubarHeight + 'px';
    this.toolbarContainer.style.height = this.toolbarHeight + 'px';

    var tmp = this.menubarHeight + this.toolbarHeight;

    if (!mxClient.IS_QUIRKS) {
        tmp += 1;
    }

    var sidebarFooterHeight = 0;

    if (this.sidebarFooterContainer !== null) {
        var bottom = (effVsplitPosition + this.splitSize + this.footerHeight);
        sidebarFooterHeight = Math.max(0, Math.min(h - tmp - bottom, this.sidebarFooterHeight));
        this.sidebarFooterContainer.style.width = effHsplitPosition + 'px';
        this.sidebarFooterContainer.style.height = sidebarFooterHeight + 'px';
        this.sidebarFooterContainer.style.bottom = bottom + 'px';
    }

    this.sidebarContainer.style.top = tmp + 'px';
    this.sidebarContainer.style.width = effHsplitPosition + 'px';

    this.outlineContainer.style.width = '100%';
    this.outlineContainer.style.height = '100%';

    //this.outlineContainer.style.width = effHsplitPosition + 'px';
    //this.outlineContainer.style.height = effVsplitPosition + 'px';
    //this.outlineContainer.style.bottom = this.footerHeight + 'px';

    this.diagramContainer.style.left = (effHsplitPosition + this.splitSize) + 'px';
    this.diagramContainer.style.top = this.sidebarContainer.style.top;
    this.footerContainer.style.height = this.footerHeight + 'px';
    //DEV:hide HSplit
    //this.hsplit.style.top = this.sidebarContainer.style.top;
    //this.hsplit.style.bottom = this.outlineContainer.style.bottom;
    //this.hsplit.style.left = effHsplitPosition + 'px';
    //this.vsplit.style.width = this.sidebarContainer.style.width;
    //this.vsplit.style.bottom = (effVsplitPosition + this.footerHeight) + 'px';

    if (quirks) {
        //DEV:hide menu bar
        //this.menubarContainer.style.width = w + 'px';
        this.toolbarContainer.style.width = w + 'px';
        var sidebarHeight = Math.max(0, h - effVsplitPosition - this.splitSize - this.footerHeight - this.menubarHeight - this.toolbarHeight);
        //this.sidebarContainer.style.height = (sidebarHeight - sidebarFooterHeight) + 'px';
        this.sidebarContainer.style.height = this.footerHeight + 'px';
        this.diagramContainer.style.width = Math.max(0, w - effHsplitPosition - this.splitSize) + 'px';
        var diagramHeight = Math.max(0, h - this.footerHeight - this.menubarHeight - this.toolbarHeight);
        this.diagramContainer.style.height = diagramHeight + 'px';
        this.footerContainer.style.width = w + 'px';
        //DEV:hide HSplit
        //this.hsplit.style.height = diagramHeight + 'px';
    }
    else {

        //this.sidebarContainer.style.bottom = (effVsplitPosition + this.splitSize + this.footerHeight + sidebarFooterHeight) + 'px';
        this.sidebarContainer.style.bottom = this.footerHeight + 'px';
        this.diagramContainer.style.bottom = this.footerHeight + 'px';

        this.setHeightDepts();
        //if (typeof this.sidebar !== 'undefined')
        //    this.sidebar.setHeightVisiblePalette(this.sidebarContainer.offsetHeight);
    }
    var wdoutline = this.editor.outlineWindowDesign;
    if (typeof wdoutline !== 'undefined') {
        wdoutline.setLocation(wdoutline.getX(),
            SweetSoftScript.mainFunction.getWindowSize().height - wdoutline.div.offsetHeight);
    }
};

EditorUi.prototype.setHeightDepts = function () {
    var divdepts = document.getElementById('divdepts');
    if (divdepts !== null) {
        if (divdepts.parentNode.style.display !== 'none') {

            var fullheight = this.sidebarContainer.clientHeight;
            var arrPanelDepts = this.sidebar.getPallete('depts');
            var arrPanelTruc = this.sidebar.getMachinery('truc');
            var otherh = arrPanelTruc[0].clientHeight + arrPanelTruc[1].childNodes[0].clientHeight;
            otherh += arrPanelDepts[0].clientHeight;

            var newh = fullheight - otherh - 2;

            document.getElementById('divdepts').style.height = newh + 'px';
            //document.getElementById('divdepts').style.height = fullheight + 'px';

        }
        else
            this.sidebar.setHeightVisiblePalette();
    }
};

/**
* Creates the required containers.
*/
EditorUi.prototype.createDivs = function () {
    //DEV:hide menu bar
    //this.menubarContainer = this.createDiv('geMenubarContainer');
    this.toolbarContainer = this.createDiv('geToolbarContainer');
    this.sidebarContainer = this.createDiv('geSidebarContainer');
    this.outlineContainer = this.createDiv('geOutlineContainer');
    this.diagramContainer = this.createDiv('geDiagramContainer');
    this.footerContainer = this.createDiv('geFooterContainer');
    //DEV:hide HSplit
    //this.hsplit = this.createDiv('geHsplit');
    //this.vsplit = this.createDiv('geVsplit');

    // Sets static style for containers

    //DEV:hide menu bar
    //this.menubarContainer.style.top = '0px';
    //this.menubarContainer.style.left = '0px';
    //this.menubarContainer.style.right = '0px';

    this.toolbarContainer.style.left = '0px';
    this.toolbarContainer.style.right = '0px';
    this.sidebarContainer.style.left = '0px';
    //this.outlineContainer.style.left = '0px';
    this.diagramContainer.style.right = '0px';
    this.footerContainer.style.left = '0px';
    this.footerContainer.style.right = '0px';
    this.footerContainer.style.bottom = '0px';
    //this.vsplit.style.left = '0px';
    //this.vsplit.style.height = this.splitSize + 'px';
    //DEV:hide HSplit
    //this.hsplit.style.width = this.splitSize + 'px';

    this.sidebarFooterContainer = this.createSidebarFooterContainer();

    if (this.sidebarFooterContainer) {
        this.sidebarFooterContainer.style.left = '0px';
    }

};

/**
* Hook for sidebar footer container. This implementation returns null.
*/
EditorUi.prototype.createSidebarFooterContainer = function () {
    return null;
};

//DEV:outline
EditorUi.prototype.createOutlineWindowDesign = function () {
    var ww = 300;
    var wh = 300;
    var ws = SweetSoftScript.mainFunction.getWindowSize();
    var pl = ws.width - ww - 30;
    var pt = ws.height - wh - 35;
    //mxWindow(title, content, x, y, width, height, minimizable, movable, replaceNode, style)
    var wnddesign = new mxWindow(SweetSoftScript.ResourceText.titleOutline, this.outlineContainer, pl, pt, ww, wh, true, true);

    wnddesign.setVisible(false);
    wnddesign.div.style.zIndex = '-1';
    wnddesign.setResizable(true);
    wnddesign.setClosable(true);
    //wnddesign.setLocation(x,y);

    wnddesign.destroyOnClose = false;
    wnddesign.setMaximizable(false);
    //wnddesign.setTitle(title);
    //add close function
    wnddesign.addListener(mxEvent.HIDE, function (e) {
        document.getElementById('btnoutline').className = 'outline geButton show';
        wnddesign.div.style.zIndex = '-1';
    });
    wnddesign.addListener(mxEvent.SHOW, function (e) {
        document.getElementById('btnoutline').className = 'outline geButton hide';
        wnddesign.div.style.zIndex = '2';
    });
    var ed = this;
    wnddesign.addListener(mxEvent.ACTIVATE, function (e) {
        //wnddesign.div.style.zIndex = '1';
        if (typeof ed.editor.outlineWindow !== 'undefined' && ed.editor.outlineWindow.isVisible() === true)
            ed.editor.outlineWindow.div.style.zIndex = '1';
        var selectprop = ed.getProductionPropertyWindow();
        if (typeof selectprop !== 'undefined')
            selectprop.window.div.style.zIndex = '1';
    });

    this.editor.outlineWindowDesign = wnddesign;
}

EditorUi.prototype.toggleOutLine = function () {
    if (this.editor.outlineWindowDesign !== null && typeof this.editor.outlineWindowDesign !== 'undefined') { }
    else
        this.createOutlineWindowDesign();
    this.editor.outlineWindowDesign.setVisible(!this.editor.outlineWindowDesign.isVisible());
};

/**
* Creates the required containers.
*/
EditorUi.prototype.createUi = function () {
    // Creates menubar
    if (typeof Menus !== 'undefined') {
        this.menubar = this.menus.createMenubar(this.createDiv('geMenubar'));
        //DEV:hide menu bar
        //this.menubarContainer.appendChild(this.menubar.container);
    }
    // Creates toolbar
    if (typeof Toolbar !== 'undefined') {
        this.toolbar = this.createToolbar(this.createDiv('geToolbar'));
        this.toolbarContainer.appendChild(this.toolbar.container);
    }

    // Creates the sidebar
    this.sidebar = this.createSidebar(this.sidebarContainer);

    // Creates the footer
    this.footerContainer.appendChild(this.createFooter());

    // Adds status bar in menubar
    this.statusContainer = this.createStatusContainer();

    // Connects the status bar to the editor status
    this.editor.addListener('statusChanged', mxUtils.bind(this, function () {
        this.setStatusText(this.editor.getStatus());
    }));

    this.setStatusText(this.editor.getStatus());
    if (typeof Menus !== 'undefined')
        this.menubar.container.appendChild(this.statusContainer);

    // Inserts into DOM
    //DEV:hide menu bar
    //this.container.appendChild(this.menubarContainer);
    this.container.appendChild(this.toolbarContainer);
    this.container.appendChild(this.sidebarContainer);
    //DEV:not add outline
    //this.container.appendChild(this.outlineContainer);
    this.createOutlineWindowDesign();
    this.container.appendChild(this.diagramContainer);
    this.container.appendChild(this.footerContainer);
    //DEV:hide HSplit
    //this.container.appendChild(this.hsplit);
    //this.container.appendChild(this.vsplit);

    if (this.sidebarFooterContainer) {
        this.container.appendChild(this.sidebarFooterContainer);
    }
    /*DEV:hide HSplit
    // HSplit
    this.addSplitHandler(this.hsplit, true, 0, mxUtils.bind(this, function (value) {
    this.hsplitPosition = value;
    this.refresh();
    this.editor.graph.sizeDidChange();
    this.editor.outline.update(false);
    this.editor.outline.outline.sizeDidChange();
    }));
    */
    // VSplit
    /*
    this.addSplitHandler(this.vsplit, false, this.footerHeight, mxUtils.bind(this, function (value) {
    this.vsplitPosition = value;
    this.refresh();
    this.editor.outline.update(false);
    this.editor.outline.outline.sizeDidChange();
    }));
    */
};

/**
* Creates a new toolbar for the given container.
*/
EditorUi.prototype.createStatusContainer = function () {
    var container = document.createElement('a');
    container.className = 'geItem geStatus';

    return container;
};

/**
* Creates a new toolbar for the given container.
*/
EditorUi.prototype.setStatusText = function (value) {
    this.statusContainer.innerHTML = value;
};

/**
* Creates a new toolbar for the given container.
*/
EditorUi.prototype.createToolbar = function (container) {
    if (typeof Toolbar !== 'undefined')
        return new Toolbar(this, container);
};

/**
* Creates a new sidebar for the given container.
*/
EditorUi.prototype.createSidebar = function (container) {
    if (typeof Sidebar !== 'undefined')
        return new Sidebar(this, container);
};

/**
* Creates and returns a new footer.
*/
EditorUi.prototype.createFooter = function () {
    return this.createDiv('geFooter');
};

/**
* Creates the actual toolbar for the toolbar container.
*/
EditorUi.prototype.createDiv = function (classname) {
    var elt = document.createElement('div');
    elt.className = classname;

    return elt;
};

/**
* Updates the states of the given undo/redo items.
*/
EditorUi.prototype.addSplitHandler = function (elt, horizontal, dx, onChange) {
    var start = null;
    var initial = null;

    // Disables built-in pan and zoom in IE10 and later
    if (mxClient.IS_POINTER) {
        elt.style.msTouchAction = 'none';
    }

    function getValue() {
        return parseInt(((horizontal) ? elt.style.left : elt.style.bottom));
    };

    function moveHandler(evt) {
        if (start != null) {
            var pt = new mxPoint(mxEvent.getClientX(evt), mxEvent.getClientY(evt));
            onChange(Math.max(0, initial + ((horizontal) ? (pt.x - start.x) : (start.y - pt.y)) - dx));
            mxEvent.consume(evt);
        }
    };

    function dropHandler(evt) {
        moveHandler(evt);
        start = null;
        initial = null;
    };

    mxEvent.addGestureListeners(elt, function (evt) {
        start = new mxPoint(mxEvent.getClientX(evt), mxEvent.getClientY(evt));
        initial = getValue();
        mxEvent.consume(evt);
    });

    mxEvent.addGestureListeners(document, null, moveHandler, dropHandler);
};

/**
* Displays a print dialog.
*/
EditorUi.prototype.showDialog = function (elt, w, h, modal, closable, onClose) {
    this.hideDialog(true);
    this.editor.graph.tooltipHandler.hideTooltip();
    this.dialog = new Dialog(this, elt, w, h, modal, closable, onClose);
};

/**
* Displays a print dialog.
*/
EditorUi.prototype.hideDialog = function (cancel) {
    if (this.dialog != null) {
        if (this.editor.graph.container.style.visibility != 'hidden') {
            this.editor.graph.container.focus();
        }

        var dlg = this.dialog;
        this.dialog = null;
        dlg.close(cancel);
    }
};
/*DEV:show main document*/
EditorUi.prototype.showMainDocument = function () {
    var ed = this;
    if (ed.checkEnable()) {
        var isEdit = ed.onBeforeUnload();

        if (isEdit !== null && typeof isEdit !== 'undefined') {
            SweetSoftScript.mainFunction.OpenRadWindow(undefined,
            isEdit,
            'confirmCancelSave',
            function () {
                ed.resetActionNode();
                ed.openMainDocument();
                ed.sidebar.hideAllPalette();
                ed.sidebar.hideAllMachineries();
                //ed.sidebar.showPaletteByIndex(0);
                //ed.sidebar.showPalette('depts');
                //ed.sidebar.showMachinery('truc');
                ed.sidebar.showPalette('depts');
            }, function () {
                ed.save(ed.editor.getOrCreateFilename(), false);
                ed.openMainDocument();
                ed.sidebar.hideAllPalette();
                ed.sidebar.hideAllMachineries();
                //ed.sidebar.showPaletteByIndex(0);
                //ed.sidebar.showPalette('depts');
                //ed.sidebar.showMachinery('truc');
                ed.sidebar.showPalette('depts');
            });
            return;
        }
        else {
            ed.openMainDocument();
            ed.sidebar.hideAllPalette();
            ed.sidebar.hideAllMachineries();
            //ed.sidebar.showMachinery('truc');
            ed.sidebar.showPalette('depts');
            //this.sidebar.showPaletteByIndex(0);
        }
    }
    else {
        ed.openMainDocument();
        ed.sidebar.hideAllPalette();
        ed.sidebar.hideAllMachineries();
        //ed.sidebar.showMachinery('truc');
        ed.sidebar.showPalette('depts');
        //this.sidebar.showPaletteByIndex(0);
    }
};

EditorUi.prototype.loadCurrentActiveNode = function () {
    if (this.loadCellInfoPath.length > 0) {
        var onload = function (req) {
            var data = req.getText();
            if (data !== null && typeof data !== 'undefined' && data.length > 0) {
                if (typeof callback === 'function') {
                    callback(data);
                }
            }
        }

        var onerror = function (req) {
            console.log(req.getStatus());
        }

        new mxXmlRequest(this.loadCellInfoPath, 'idcell=' +
            idcell + '&idtruc=' + idtruc + '&filename=' + code).send(onload, onerror);
    }
}

EditorUi.prototype.resetDesign = function () {
    //permission
    if (this.checkEnable() === false)
        return;
    var graph = this.editor.graph;

    //var allEdge = graph.selectEdges(); this.toolbar.item
    //var allVertex = graph.selectVertices();

    var model = graph.getModel();
    var allNode = model.root.children[0].children;
    if (allNode.length > 2) {
        var countDeleteable = 0;
        for (var i = 0; i < allNode.length; i++) {
            if (countDeleteable > 0)
                break;
            if ((typeof allNode[i].isSource !== 'undefined' && allNode[i].isSource !== null)
                || (typeof allNode[i].isTarget !== 'undefined' && allNode[i].isTarget !== null)) { }
            else {
                countDeleteable += 1;
                break;
            }
        }
        if (countDeleteable > 0) {
            var ed = this;
            SweetSoftScript.mainFunction.OpenRadWindow(undefined,
                SweetSoftScript.ResourceText.confirmResetThisDesign,
                    'confirmResetDesign',
                    function () {
                        ed.processResetDesign();
                    });
        }
    }
}

EditorUi.prototype.markAsReset = undefined;
EditorUi.prototype.processResetDesign = function () {
    var dataid = '0';
    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    //for load data cell
    var code = EditorUi.prototype.lastWorkDocument;
    if (typeof code === 'undefined' || code.length === 0)
        code = 'Main-Work';
    var ed = this;
    var graph = ed.editor.graph;

    //begin delete in client
    var model = graph.getModel();
    var allNode = model.root.children[0].children;
    if (allNode.length > 2) {
        var arrDelete = [];
        for (var i = 0; i < allNode.length; i++) {
            if ((typeof allNode[i].isSource !== 'undefined' && allNode[i].isSource !== null)
                || (typeof allNode[i].isTarget !== 'undefined' && allNode[i].isTarget !== null)) {
                console.log('root : ', allNode[i]);
                this.saveActionNode(allNode[i], 'insert');
            }
            else
                arrDelete.push(allNode[i]);
        }
        if (arrDelete.length > 0)
            graph.removeCells(arrDelete);
    }

    //reset action node
    ed.resetActionNode(undefined, true);
    EditorUi.prototype.markAsReset = true;
}

/*DEV:file collection*/
EditorUi.prototype.fileCollection = null;
/*DEV:load file path*/
EditorUi.prototype.loadFilePath = null;

EditorUi.prototype.deletePath = null;

EditorUi.prototype.openFilePath = function (text, isMain) {

    var ed = this;
    var onload = function (req) {
        var data = req.getText();
        /*old
        
        //var dataXML = req.getDocumentElement();
        
        
        var doc = mxUtils.parseXml(data);
        

        var enc = new mxCodec(doc.documentElement.ownerDocument);
        
        enc.decode(doc.documentElement, ed.editor.graph.getModel());

        ed.editor.setModified(false);
        ed.editor.setFilename(text);
        ed.updateDocumentTitle();
        */

        /*new*/
        var doc = mxUtils.parseXml(data);

        ed.editor.setGraphXml(doc.documentElement);
        ed.editor.setModified(false);
        ed.editor.undoManager.clear();

        if (text !== null) {
            ed.editor.setFilename(text);
            ed.updateDocumentTitle();
        }


        //remove whitespace and linebreak
        data = data.replace(/^\s+|\s+$/g, '').replace(/\s{2,}/g, ' ');

        //save data loaded
        if (isMain === true) {//main document

            ed.mainDocument = { id: text, content: data };
        }
        else {//other document
            var obj = { id: text, content: data };
            ed.WorkDocument.push(obj);
        }
    }

    var onerror = function (req) {
        console.log(req.getStatus());
    }


    new mxXmlRequest(ed.loadFilePath, 'filename=' + text).send(onload, onerror);

    ed.hideDialog();
};
/*DEV:main document*/
EditorUi.prototype.mainDocument = null;
/*DEV:other work document*/
EditorUi.prototype.WorkDocument = [];
EditorUi.prototype.TrucDocument = [];
/*DEV:go back button*/
EditorUi.prototype.goBackButton = null;
EditorUi.prototype.processOpen = function () {
    //DEV:mod
    var dlg = new OpenFileDialog(this, mxResources.get('open'),
        mxUtils.bind(this, function (name) {
            if (name !== null && typeof name !== 'undefined') {
                this.OpenFilePath($(name).text());
            }
            else {
                SweetSoftScript.mainFunction.OpenRadWindow(undefined, 'Please select data!', 'alert');
            }
        }), 'select file');
    this.dialog = new Dialog(this, dlg.container, 300, 200, true, true, undefined);
};
/*DEV:open main document*/
EditorUi.prototype.openMainDocument = function () {
    var ed = this;

    if (ed.TrucDocument !== null && ed.TrucDocument.length > 0) {
        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            activeMachine.className = 'geItem';

        var obj = eval('(' + ed.TrucDocument[0].content + ')');
        var doc = mxUtils.parseXml(obj.data);

        ed.editor.setGraphXml(doc.documentElement);
        ed.editor.setFilename('Main-Work');
        ed.updateDocumentTitle(SweetSoftScript.ResourceText.mainworkTitle);
        editorMain.sidebar.hideMachinery('truc');
        ed.editor.setModified(false);
        ed.editor.undoManager.clear();

        EditorUi.prototype.IdOfLineMap = undefined;
        EditorUi.prototype.lastWorkDocument = undefined;
        EditorUi.prototype.lastParentId = obj.id.toString();
        EditorUi.prototype.currentId = obj.id.toString();

        this.goBackButton.style.display = 'none';
        var selectprop = this.getVisibleProductionPropertyWindow();
        if (typeof selectprop !== 'undefined')
            selectprop.window.hide();
    }
};

EditorUi.prototype.processDrawAndSave = function (target, source, isMainWork) {
    var dataid = '0';
    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    var graphNew = new mxGraph();
    var name = (target.dataGraphId || 'end') + '-' + (source.dataGraphId || 'start');
    name = 'workas' + name + '.xml';
    if (isMainWork === true) {
        source.isSource = '1';
        target.isTarget = '1';
        name = 'Main-Work';
    }
    // Gets the default parent for inserting new cells. This
    // is normally the first child of the root (ie. layer 0).
    var parent = graphNew.getDefaultParent();
    var ta, so;
    // Adds cells to the model in a single step
    graphNew.getModel().beginUpdate();
    try {
        /*test:done
        var cellId = "tempmain";
        var v0 = graphNew.insertVertex(parent, cellId, 'Dummy', 0, 0, 60, 40);
        var requestId = 'test';
        // Creates the random links and cells for the response
        for (var i = 0; i < 4; i++) {
        var id = requestId + '-' + i;
        var v = graphNew.insertVertex(parent, id, id, 0, 0, 60, 40);
        //var e = graphNew.insertEdge(parent, null, 'Link ' + i, v0, v);
        
        }
        */

        if (source !== null && typeof source !== 'undefined') {
            //so = graphNew.insertVertex(parent, source.dataId + '-' + source.dataCode, source.value, 120,
            so = graphNew.insertVertex(parent, null, source.value, 120,
                 30, source.geometry.width, source.geometry.height, source.style);

            if (typeof source.dataId !== 'undefined')
                so.dataId = source.dataId;
            so.isSource = 1;
            if (typeof source.dataCode !== 'undefined')
                so.dataCode = source.dataCode;
            if (typeof source.dataGraphId !== 'undefined')
                so.dataGraphId = source.dataGraphId;

            //ta = graphNew.insertVertex(parent, target.dataId + '-' + target.dataCode, target.value, 120,
            ta = graphNew.insertVertex(parent, null, target.value, 120,
                 30 + source.geometry.height + 300, target.geometry.width, target.geometry.height, target.style);
            if (target.dataId !== null)
                ta.dataId = target.dataId;
            ta.isTarget = 1;
            if (typeof target.dataCode !== 'undefined')
                ta.dataCode = target.dataCode;
            if (typeof target.dataGraphId !== 'undefined')
                ta.dataGraphId = target.dataGraphId;
        }
        /*
        else {
        //ta = graphNew.insertVertex(parent, target.dataId + '-' + target.dataCode, target.value, 120,
        ta = graphNew.insertVertex(parent, null, target.value, 120,
        50, target.geometry.width, target.geometry.height, target.style);
        if (target.dataId !== null)
        ta.dataId = target.dataId;
        ta.isTarget = '1';
        if (typeof target.dataCode !== 'undefined')
        ta.dataCode = target.dataCode;
        if (typeof target.dataGraphId !== 'undefined')
        ta.dataGraphId = target.dataGraphId;
        }
        */
    }
    finally {
        // Updates the display
        graphNew.getModel().endUpdate();
    }

    var enc = new mxCodec();
    var node = enc.encode(graphNew.getModel());


    var dataText = mxUtils.getXml(node);


    var doc = mxUtils.parseXml(dataText);
    this.editor.setGraphXml(doc.documentElement);

    //set center
    this.editor.graph.selectCells(true);
    this.editor.graph.alignCells(mxConstants.ALIGN_CENTER);
    this.editor.graph.clearSelection();

    this.addActionNode(name + '*' + dataid);

    this.saveActionNode(so, 'insert');
    this.saveActionNode(ta, 'insert');

    //save to xml  
    this.editor.setFilename(name);
    //this.save(name, false);
    document.getElementById('page_spinner').style.display = 'none';
    var title = source.value + ' - ' + target.value;
    this.updateDocumentTitle(title);
}

EditorUi.prototype.processLoadDraw = function (target, source) {
    var name = (target.dataGraphId || 'end') + '-' + (source.dataGraphId || 'start');
    name = 'workas' + name + '.xml';
    var dataid = '0';
    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    //var trucDoc = this.getTrucDocument(activeMachine.getAttribute('dataid'));

    var dataText = this.getWorkDocument(name);
    this.goBackButton.style.display = 'block';

    if (typeof dataText === 'undefined') {
        //try load data
        var ed = this;
        var title = source.value + ' - ' + target.value;
        ed.updateDocumentTitle(title);
        ed.editor.setFilename(name);
        document.getElementById('page_spinner').style.display = 'block';
        var onload = function (req) {
            var data = req.getText();
            if (typeof data !== 'undefined' && data.length > 0) {
                /*new*/
                var obj = eval('(' + data + ')');
                if (obj.data.length > 0) {
                    var doc = mxUtils.parseXml(obj.data);

                    ed.editor.setGraphXml(doc.documentElement);
                    ed.editor.setModified(false);
                    ed.editor.undoManager.clear();


                    //remove whitespace and linebreak
                    //save data

                    data = data.replace(/^\s+|\s+$/g, '').replace(/\s{2,}/g, ' ');
                    if ((typeof (EditorUi.prototype.currentId) !== 'undefined') && EditorUi.prototype.currentId !== null)
                        EditorUi.prototype.lastParentId = EditorUi.prototype.currentId;
                    EditorUi.prototype.currentId = obj.id.toString();
                    EditorUi.prototype.lastWorkDocument = name;

                    //save data
                    obj = {
                        id: name + '*' + dataid,
                        content: data
                    };
                    ed.WorkDocument.push(obj);
                    ed.addActionNode(name + '*' + dataid);

                    setTimeout(function () {
                        document.getElementById('page_spinner').style.display = 'none';
                    }, 500);
                }
                else
                    ed.processDrawAndSave(target, source);
            }
            else
                ed.processDrawAndSave(target, source);
        }

        var onerror = function (req) {
            console.log(req.getStatus());
            ed.processDrawAndSave(target, source);
        }

        new mxXmlRequest(ed.loadFilePath, 'filename=' + name + '&idparent='
            + EditorUi.prototype.lastParentId + '&IdTruc=' + dataid).send(onload, onerror);
    }
    else {

        var obj = eval('(' + dataText.content + ')');
        var doc = mxUtils.parseXml(obj.data);
        this.editor.setGraphXml(doc.documentElement);
        var title = source.value + ' - ' + target.value;
        this.editor.setFilename(name);
        this.editor.setModified(false);
        this.updateDocumentTitle(title);

        if ((typeof (EditorUi.prototype.currentId) !== 'undefined') && EditorUi.prototype.currentId !== null)
            EditorUi.prototype.lastParentId = EditorUi.prototype.currentId;
        EditorUi.prototype.currentId = obj.id.toString();
        EditorUi.prototype.lastWorkDocument = name;

    }
};

EditorUi.prototype.updateTrucDocument = function (id, data) {
    var otherArr = this.TrucDocument;

    for (var i = 0; i < otherArr.length; i++) {
        if (otherArr[i].id.toString() === id.toString()) {
            otherArr[i].content = data;
            break;
        }
    }
};

EditorUi.prototype.updateWorkDocument = function (id, data) {

    var otherArr = this.WorkDocument;

    for (var i = 0; i < otherArr.length; i++) {
        if (otherArr[i].id.toString() === id.toString()) {
            otherArr[i].content = data;
            break;
        }
    }
};

EditorUi.prototype.getTrucDocument = function (id) {
    var otherArr = this.TrucDocument;
    var obj = undefined;
    for (var i = 0; i < otherArr.length; i++) {

        if (otherArr[i].id.toString() === id.toString()) {
            obj = otherArr[i];
            break;
        }
    }
    return obj;
};

EditorUi.prototype.removeWorkDocument = function (fullid) {
    var otherArr = this.WorkDocument;
    for (var i = 0; i < otherArr.length; i++) {
        if (otherArr[i].id.toString() === fullid) {
            otherArr.splice(i, 1);
            break;
        }
    }
}

EditorUi.prototype.getWorkDocument = function (code, dataid) {
    if (typeof dataid === 'undefined') {
        dataid = '0';
        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');
    }
    var otherArr = this.WorkDocument;

    var obj = undefined;
    for (var i = 0; i < otherArr.length; i++) {
        console.log(otherArr[i].id.toString(), (code + '*' + dataid));
        if (otherArr[i].id.toString() === code + '*' + dataid) {
            obj = otherArr[i];
            break;
        }
    }

    return obj;
};

/**
* Adds the label menu items to the given menu and parent.
*/
EditorUi.prototype.openFile = function () {
    var ed = this;
    var isEdit = ed.onBeforeUnload();

    if (isEdit !== null && typeof isEdit !== 'undefined') {
        SweetSoftScript.mainFunction.OpenRadWindow(undefined, isEdit, 'confirmCancelSave', function () {
            ed.processOpen();
        });
        //var data = mxUtils.confirm(isEdit);        
        return;
    }
    else
        ed.processOpen();

    /*original
    // Closes dialog after open
    window.openFile = new OpenFile(mxUtils.bind(this, function (cancel) {
    this.hideDialog(cancel);
    }));

    // Removes openFile if dialog is closed
    this.showDialog(new OpenDialog(this).container, 300, 180, true, true, function () {
    window.openFile = null;
    });
    */
};

/**
* Constructs a new open file dialog.
*/
OpenFileDialog = function (editorUi, buttonText, callback, label) {
    /// <summary>Constructs a new open file dialog.</summary>
    /// <param name="buttonText" type="string">Text of button save.</param>
    /// <param name="callback" type="function">The function of button save click.</param>
    /// <param name="label" type="string">The label of file name.</param>
    /// <returns type="object">The Filename Dialog.</returns>
    var row, td;
    var table = document.createElement('table');
    table.style.width = '100%';
    var tbody = document.createElement('tbody');

    row = document.createElement('tr');

    td = document.createElement('td');
    td.style.fontSize = '10pt';
    td.style.width = '100%';
    var span = document.createElement('span');
    $(span).text((label || mxResources.get('filename')) + ':');
    td.appendChild(span);


    var divmain = document.createElement('div');
    divmain.className = 'windowmain';
    //divmain.style.width = '300px';
    //divmain.style.height = '200px';
    var ul = document.createElement('ul');

    function AppendLi(indx) {
        var li = document.createElement('li');
        $(li).text(editorUi.fileCollection[indx]);
        mxEvent.addListener(li, 'click', function (evt) {
            var liColl = this.parentNode.childNodes;
            for (var i = 0, j = liColl.length; i < j; i++) {

                if (liColl[i].className === 'active') {
                    liColl[i].className = '';
                    break;
                }
            }
            this.className = 'active';
        });
        ul.appendChild(li);
    }

    if (editorUi.fileCollection === null) {
        var onload = function (req) {
            var data = req.getText();

            var parseData = JSON.parse(data);
            editorUi.fileCollection = parseData;
            if (editorUi.fileCollection !== null) {
                for (var i = 0, j = editorUi.fileCollection.length; i < j; i++) {
                    AppendLi(i);
                }
            }
        }

        var onerror = function (req) {
            console.log(req.getStatus());
        }

        new mxXmlRequest(editorUi.loadFilePath, 'filename=').send(onload, onerror);
    }
    else {
        if (editorUi.fileCollection !== null) {
            for (var i = 0, j = editorUi.fileCollection.length; i < j; i++) {
                AppendLi(i);
            }
        }
    }

    divmain.appendChild(ul);

    td.appendChild(divmain);
    row.appendChild(td);

    this.init = function () {
        //
    };

    tbody.appendChild(row);

    row = document.createElement('tr');
    td = document.createElement('td');
    td.colSpan = 2;
    td.style.paddingTop = '20px';
    td.style.whiteSpace = 'nowrap';
    td.setAttribute('align', 'right');


    var genericBtn = mxUtils.button(buttonText, function () {

        //send file name to function "SweetSoftScript.mainFunction.editorUi.save"
        var liColl = divmain.childNodes[0].childNodes;
        var li;
        for (var i = 0, j = liColl.length; i < j; i++) {
            if (liColl[i].className === 'active') {
                li = liColl[i];
                break;
            }
        }
        callback(li);
    });


    td.appendChild(genericBtn);

    td.appendChild(mxUtils.button(mxResources.get('cancel'), function () {
        editorUi.hideDialog();
    }));

    row.appendChild(td);
    tbody.appendChild(row);

    tbody.appendChild(row);
    table.appendChild(tbody);
    this.container = table;
};

/**
* Adds the label menu items to the given menu and parent.
*/
EditorUi.prototype.saveFile = function (forceDialog) {

    if (!forceDialog && this.editor.filename !== null) {
        this.save(this.editor.getOrCreateFilename());
    }
    else {
        var dlg = new FilenameDialog(this, this.editor.getOrCreateFilename(), mxResources.get('save'), mxUtils.bind(this, function (name) {
            this.save(name, true);
        }));
        this.showDialog(dlg.container, 300, 100, true, true);
        dlg.init();
    }
};

EditorUi.prototype.processDeleteCell = function (cell) {
    if (this.editor.graph.isCellDeletable()) {
        /*if ((typeof cell !== 'undefined' && typeof cell.dataId !== 'undefined') ||
        (typeof cell !== 'undefined' && typeof cell.isSubWork !== 'undefined')) {
        */
        var ui = this;
        var dataid = '0';
        var activeMachine = ui.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');

        var code = EditorUi.prototype.lastWorkDocument;
        if (typeof code === 'undefined' || code.length === 0)
            code = 'Main-Work';
        document.getElementById('page_spinner').style.display = 'block';
        var onload = function (req) {
            var data = req.getText();
            if (typeof data !== 'undefined' && data !== null && data.length > 0) {
                var parseData = JSON.parse(data);

                if (typeof parseData !== 'undefined' && parseData.isError === false) {
                    ui.editor.graph.removeCells([cell]);
                    ui.saveActionNode(cell, 'delete');
                    if (cell.isEdge()) {
                        var arr = this.externalType;
                        var arrNodeTruc = undefined;
                        if (arr !== null && typeof arr !== 'undefined') {
                            var isgraphic = true;
                            for (var i = 0; i < arr.length; i++) {
                                if (arr[i].sid.toString() === cell.source.dataId.toString()
                                    /*&& arr[i].eid.toString() === cell.target.dataId.toString()*/) {
                                    arrNodeTruc = ui.sidebar.getMachinery('graphictype');
                                    isgraphic = false;
                                    break;
                                }
                            }
                            if (isgraphic === true)
                                arrNodeTruc = ui.sidebar.getMachinery('truc');
                        }
                        else
                            arrNodeTruc = ui.sidebar.getMachinery('truc');

                        if (arrNodeTruc !== null) {
                            var nodeColl = arrNodeTruc[1].childNodes[0].childNodes;
                            var namecode = 'workas' + cell.target.dataGraphId + '-' + cell.source.dataGraphId + '.xml';
                            if (nodeColl !== null && nodeColl.length > 0) {
                                for (var i = 0; i < nodeColl.length; i++) {
                                    ui.removeActionNode(namecode + '*' + nodeColl[i].getAttribute('dataid'));
                                    ui.removeWorkDocument(namecode + '*' + nodeColl[i].getAttribute('dataid'));
                                }
                            }
                        }
                    }

                    document.getElementById('page_spinner').style.display = 'none';
                }
                else {
                    //mxUtils.alert(parseData.errorMessage);
                    document.getElementById('page_spinner').style.display = 'none';
                    SweetSoftScript.mainFunction.OpenRadWindow(SweetSoftScript.Data.notifyTitle, parseData.errorMessage, 'alert');
                }
            }
            else {
                document.getElementById('page_spinner').style.display = 'none';
            }
        }

        var onerror = function (req) {
            console.log(req.getStatus());
            document.getElementById('page_spinner').style.display = 'none';
        }

        new mxXmlRequest(ui.deletePath, 'objId=' + cell.id + '&idtruc=' +
                dataid + '&filename=' + code + ('&isedge=' + (cell.isEdge() ? '1' : '0'))
                ).send(onload, onerror);
        /*}
        else {

            this.saveActionNode(cell, 'delete');
        this.editor.graph.removeCells([cell]);
        }*/
    }
}

/**
* Saves the current graph under the given filename.
*/
EditorUi.prototype.save = function (name, showMessage) {
    if (this.checkEnable() === false) {
        document.getElementById('page_spinner').style.display = 'none';
        return;
    }
    if (name !== null) {
        var xml = mxUtils.getXml(this.editor.getGraphXml());

        try {
            if (useLocalStorage) {
                if (localStorage.getItem(name) !== null &&
                    !mxUtils.confirm(mxResources.get('replace', [name]))) {
                    return;
                }

                localStorage.setItem(name, xml);
                this.editor.setStatus(mxResources.get('saved') + ' ' + new Date());
            }
            else {
                if (xml.length < MAX_REQUEST_SIZE) {
                    name = encodeURIComponent(name);
                    //new mxXmlRequest(SAVE_URL, 'filename=' + name + '&xml=' + xml).simulate(document, '_blank');

                    var dataid = '0';
                    var activeMachine = this.sidebar.getActiveMachinery();
                    if (activeMachine !== null)
                        dataid = activeMachine.getAttribute('dataid');

                    var ed = this;
                    var objectSave = ed.convertActionNode();

                    document.getElementById('page_spinner').style.display = 'block';
                    var onload = function (req) {
                        var data = req.getText();
                        if (data === 'login') {
                            ed.resetActionNode();
                            location.href = 'fix/NodeTest.aspx';
                        }
                        else {
                            if (ed.mainDocument !== null && name === ed.mainDocument.id.toString()) {
                                ed.mainDocument.content = xml;
                                document.getElementById('page_spinner').style.display = 'none';
                            }
                            else {
                                if (typeof data !== 'undefined' && data.length > 0
                                    && data.split('||').length > 1 && data.split('||')[1].length > 0) {
                                    var obj;
                                    if (name.toLowerCase().indexOf('main-work') >= 0)
                                        obj = ed.getTrucDocument(dataid);
                                    else
                                        obj = ed.getWorkDocument(name, dataid);

                                    console.log(obj);

                                    if (typeof obj === 'undefined') {
                                        obj = {
                                            id: name.toLowerCase().indexOf('main-work') >= 0 ? dataid : (name + '*' + dataid),
                                            content: '{"id":"' + data.split('||')[1] + '","data":"' + xml.replace(/["]/g, "'") + '"}',
                                            source: { id: data.split('||')[1], idparent: EditorUi.prototype.lastParentId }
                                        };

                                        if (name.toLowerCase().indexOf('main-work') >= 0)
                                            ed.TrucDocument.push(obj);
                                        else
                                            ed.WorkDocument.push(obj);
                                    }
                                    else {
                                        if (name.toLowerCase().indexOf('main-work') >= 0)
                                            ed.updateTrucDocument(dataid, '{"id":"' + data.split('||')[1] + '","data":"' + xml.replace(/["]/g, "'") + '"}');
                                        else {
                                            ed.updateWorkDocument((name + '*' + dataid), '{"id":"' + data.split('||')[1] + '","data":"' + xml.replace(/["]/g, "'") + '"}');
                                        }
                                    }

                                    EditorUi.prototype.markAsReset = undefined;

                                    //EditorUi.prototype.lastParentId = obj.source.id;

                                    document.getElementById('page_spinner').style.display = 'none';
                                    if (showMessage !== false) {
                                        //show success save                         
                                        SweetSoftScript.mainFunction.OpenRadWindow(SweetSoftScript.Data.notifyTitle, data.split('||')[0], 'alert');
                                        if ((typeof (EditorUi.prototype.currentId) !== 'undefined') && EditorUi.prototype.currentId !== null)
                                            EditorUi.prototype.lastParentId = EditorUi.prototype.currentId;
                                        EditorUi.prototype.currentId = data.split('||')[1];
                                        //reset                         
                                        ed.resetActionNode();
                                    }
                                    else {
                                        //reset                         
                                        //console.log(name + '*' + dataid, ed.getActionFile(name + '*' + dataid));
                                        ed.resetActionNode(ed.getActionFile(name + '*' + dataid));
                                        //console.log(EditorUi.prototype.lastParentId, EditorUi.prototype.currentId, obj.source.id);
                                    }

                                    EditorUi.prototype.lastWorkDocument = name;
                                    //we save unmapped
                                    console.log(dataid);
                                    ed.processSaveInfoCell(undefined, dataid);
                                }
                                else {
                                    if (data && data.length > 0) {
                                        //console.log(data);
                                        var arrError = eval(data);
                                        //console.log(arrError.length, arrError[0]);
                                        if (arrError.length > 0) {
                                            for (var h = 0; h < arrError.length; h++) {
                                                var nodeDel = ed.editor.graph.model.getCell(arrError[h]);
                                                if (nodeDel) {
                                                    //not delete two root
                                                    if (nodeDel.isSource === null && typeof nodeDel.isSource === 'undefined' &&
                                                       nodeDel.isTarget === null && typeof nodeDel.isTarget === 'undefined')
                                                        ed.editor.graph.removeCells([nodeDel]);
                                                }
                                            }
                                        }
                                    }
                                    document.getElementById('page_spinner').style.display = 'none';
                                }
                            }
                        }
                    }

                    var onerror = function (req) {
                        console.log(req.getStatus());
                        document.getElementById('page_spinner').style.display = 'none';
                    }


                    new mxXmlRequest(SAVE_URL, 'filename=' + name + '&xml=' + encodeURIComponent(xml) + '&idparent=' +
                       (name.toLowerCase().indexOf('main-work') >= 0 ? '0' : (typeof EditorUi.prototype.lastParentId !== 'undefined' ? EditorUi.prototype.lastParentId : 0))
                        + '&IdTruc=' + dataid + '&an=' + (typeof objectSave !== 'undefined' ? JSON.stringify(objectSave) : '')
                        + ((typeof EditorUi.prototype.markAsReset !== 'undefined' && EditorUi.prototype.markAsReset === true) ? '&act=reset' : '')
                        + ((typeof EditorUi.prototype.IdOfLineMap !== 'undefined') ? ('&lineid=' + EditorUi.prototype.IdOfLineMap) : '')).send(onload, onerror);
                }

                else {
                    SweetSoftScript.mainFunction.OpenRadWindow(undefined, mxResources.get('drawingTooLarge'), 'alert');
                    mxUtils.popup(xml);

                    return;
                }
            }

            this.editor.setModified(false);
            this.editor.setFilename(name);
            //this.updateDocumentTitle();
        }
        catch (e) {
            this.editor.setStatus('Error saving file');
            document.getElementById('page_spinner').style.display = 'none';
        }
    }
};

/**
* Translates this point by the given vector.
* 
* @param {number} dx X-coordinate of the translation.
* @param {number} dy Y-coordinate of the translation.
*/
EditorUi.prototype.getSvg = function (background, scale, border) {
    scale = (scale != null) ? scale : 1;
    border = (border != null) ? border : 1;

    var graph = this.editor.graph;
    var imgExport = new mxImageExport();
    var bounds = graph.getGraphBounds();
    var vs = graph.view.scale;

    // Prepares SVG document that holds the output
    var svgDoc = mxUtils.createXmlDocument();
    var root = (svgDoc.createElementNS != null) ?
            svgDoc.createElementNS(mxConstants.NS_SVG, 'svg') : svgDoc.createElement('svg');

    if (background != null) {
        if (root.style != null) {
            root.style.backgroundColor = background;
        }
        else {
            root.setAttribute('style', 'background-color:' + background);
        }
    }

    if (svgDoc.createElementNS == null) {
        root.setAttribute('xmlns', mxConstants.NS_SVG);
    }
    else {
        // KNOWN: Ignored in IE9-11, adds namespace for each image element instead. No workaround.
        root.setAttributeNS('http://www.w3.org/2000/xmlns/', 'xmlns:xlink', mxConstants.NS_XLINK);
    }

    root.setAttribute('width', (Math.ceil(bounds.width * scale / vs) + 2 * border) + 'px');
    root.setAttribute('height', (Math.ceil(bounds.height * scale / vs) + 2 * border) + 'px');
    root.setAttribute('version', '1.1');

    // Adds group for anti-aliasing via transform
    var group = (svgDoc.createElementNS != null) ?
            svgDoc.createElementNS(mxConstants.NS_SVG, 'g') : svgDoc.createElement('g');
    group.setAttribute('transform', 'translate(0.5,0.5)');
    root.appendChild(group);
    svgDoc.appendChild(root);

    // Renders graph. Offset will be multiplied with state's scale when painting state.
    var svgCanvas = new mxSvgCanvas2D(group);
    svgCanvas.translate(Math.floor((border / scale - bounds.x) / vs), Math.floor((border / scale - bounds.y) / vs));
    svgCanvas.scale(scale / vs);

    // Paints background image
    var bgImg = graph.backgroundImage;

    if (bgImg != null) {
        var tr = graph.view.translate;
        svgCanvas.image(tr.x, tr.y, bgImg.width, bgImg.height, bgImg.src, false);
    }

    imgExport.drawState(graph.getView().getState(graph.model.root), svgCanvas);

    return root;
};

/**
* Executes the given layout.
*/
EditorUi.prototype.executeLayout = function (exec, animate, post) {
    var graph = this.editor.graph;

    if (graph.isEnabled()) {
        graph.getModel().beginUpdate();
        try {
            exec();
        }
        catch (e) {
            throw e;
        }
        finally {
            // Animates the changes in the graph model except
            // for Camino, where animation is too slow
            if (this.allowAnimation && animate && navigator.userAgent.indexOf('Camino') < 0) {
                // New API for animating graph layout results asynchronously
                var morph = new mxMorphing(graph);
                morph.addListener(mxEvent.DONE, mxUtils.bind(this, function () {
                    graph.getModel().endUpdate();

                    if (post != null) {
                        post();
                    }
                }));

                morph.startAnimation();
            }
            else {
                graph.getModel().endUpdate();
            }
        }
    }
};

/**
* Creates the keyboard event handler for the current graph and history.
*/
EditorUi.prototype.createKeyHandler = function (editor) {
    var graph = this.editor.graph;
    var keyHandler = new mxKeyHandler(graph);

    // Routes command-key to control-key on Mac
    keyHandler.isControlDown = function (evt) {
        return mxEvent.isControlDown(evt) || (mxClient.IS_MAC && evt.metaKey);
    };

    // Helper function to move cells with the cursor keys
    function nudge(keyCode) {
        if (!graph.isSelectionEmpty() && graph.isEnabled()) {
            var dx = 0;
            var dy = 0;

            if (keyCode === 37) {
                dx = -1;
            }
            else if (keyCode === 38) {
                dy = -1;
            }
            else if (keyCode === 39) {
                dx = 1;
            }
            else if (keyCode === 40) {
                dy = 1;
            }

            graph.moveCells(graph.getSelectionCells(), dx, dy);
            graph.scrollCellToVisible(graph.getSelectionCell());
        }
    };

    // Binds keystrokes to actions
    var bindAction = mxUtils.bind(this, function (code, control, key, shift) {
        if (typeof Actions !== 'undefined') {
            var action = this.actions.get(key);

            if (action !== null) {
                var f = function () {
                    if (action.isEnabled()) {
                        action.funct();
                    }
                };

                if (control) {
                    if (shift) {
                        keyHandler.bindControlShiftKey(code, f);
                    }
                    else {
                        keyHandler.bindControlKey(code, f);
                    }
                }
                else {
                    if (shift) {
                        keyHandler.bindShiftKey(code, f);
                    }
                    else {
                        keyHandler.bindKey(code, f);
                    }
                }
            }
        }
    });

    var ui = this;
    var keyHandleEscape = keyHandler.escape;
    keyHandler.escape = function (evt) {
        ui.hideDialog();
        keyHandleEscape.apply(this, arguments);
    };

    // Ignores enter keystroke. Remove this line if you want the
    // enter keystroke to stop editing.
    keyHandler.enter = function () { };
    keyHandler.bindShiftKey(13, function () { graph.foldCells(true); }); // Shift-Enter
    keyHandler.bindKey(13, function () { graph.foldCells(false); }); // Enter
    keyHandler.bindKey(33, function () { graph.exitGroup(); }); // Page Up
    keyHandler.bindKey(34, function () { graph.enterGroup(); }); // Page Down
    keyHandler.bindKey(36, function () { graph.home(); }); // Home
    keyHandler.bindKey(35, function () { graph.refresh(); }); // End
    keyHandler.bindKey(37, function () { nudge(37); }); // Left arrow
    keyHandler.bindKey(38, function () { nudge(38); }); // Up arrow
    keyHandler.bindKey(39, function () { nudge(39); }); // Right arrow
    keyHandler.bindKey(40, function () { nudge(40); }); // Down arrow    
    //keyHandler.bindKey(113, function () { graph.startEditingAtCell(); });
    keyHandler.bindKey(8, function () { graph.foldCells(true); }); // Backspace
    bindAction(8, false, 'delete'); // Backspace
    bindAction(46, false, 'delete'); // Delete
    bindAction(82, true, 'tilt'); // Ctrl+R
    bindAction(83, true, 'save'); // Ctrl+S
    bindAction(83, true, 'saveAs', true); // Ctrl+Shift+S
    bindAction(107, false, 'zoomIn'); // Add
    bindAction(109, false, 'zoomOut'); // Subtract
    bindAction(65, true, 'selectAll'); // Ctrl+A
    bindAction(86, true, 'selectVertices', true); // Ctrl+Shift+V
    bindAction(69, true, 'selectEdges', true); // Ctrl+Shift+E
    bindAction(66, true, 'toBack'); // Ctrl+B
    bindAction(70, true, 'toFront', true); // Ctrl+Shift+F
    bindAction(68, true, 'duplicate'); // Ctrl+D
    bindAction(90, true, 'undo'); // Ctrl+Z
    bindAction(89, true, 'redo'); // Ctrl+Y
    bindAction(88, true, 'cut'); // Ctrl+X
    bindAction(67, true, 'copy'); // Ctrl+C
    bindAction(81, true, 'connect'); // Ctrl+Q
    bindAction(86, true, 'paste'); // Ctrl+V
    bindAction(71, true, 'group'); // Ctrl+G
    bindAction(77, true, 'editData'); // Ctrl+M
    bindAction(71, true, 'grid', true); // Ctrl+Shift+G
    bindAction(76, true, 'lockUnlock'); // Ctrl+L
    bindAction(80, true, 'print'); // Ctrl+P
    bindAction(85, true, 'ungroup'); // Ctrl+U
    //bindAction(112, false, 'about'); // F1

    return keyHandler;
};

EditorUi.prototype.cellMapping = [];
EditorUi.prototype.loadCellInfoPath = '';

EditorUi.prototype.saveInfoCell = function (cell, data) {
    var dataid = '0';
    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    if (typeof dataid !== 'undefined') {
        if (cell.isSubWork === '1' || cell.isSubWork === 1) {
            //for load data cell
            var code = EditorUi.prototype.lastWorkDocument;
            if (typeof code === 'undefined' || code.length === 0)
                code = 'Main-Work';
            var arr = EditorUi.prototype.cellMapping;
            if (typeof arr === 'undefined' || arr === null)
                arr = [];
            if (arr.length > 0) {
                var found = false;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.toString() === cell.id.toString() && arr[i].datadesign === code + '*' + dataid) {
                        arr[i].datavalue = data;
                        arr[i].datavalue.id = '0';


                        //if (arr[i].datavalue.id !== '0')
                        //    this.processSaveInfoCell(arr[i]);

                        found = true;
                        break;
                    }
                }
                if (found === false)
                    arr.push({ id: cell.id.toString(), datadesign: code + '*' + dataid, datavalue: data });
            }
            else
                arr.push({ id: cell.id.toString(), datadesign: code + '*' + dataid, datavalue: data });
            EditorUi.prototype.cellMapping = arr;
        }
    }

}

EditorUi.prototype.loadInfoCell = function (cell, callback) {
    if (this.loadCellInfoPath.length > 0) {
        var dataid = '0';
        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');

        if (typeof dataid !== 'undefined') {
            if (cell.isSubWork === '1' || cell.isSubWork === 1) {
                //for load data cell
                var code = EditorUi.prototype.lastWorkDocument;
                if (typeof code === 'undefined' || code.length === 0)
                    code = 'Main-Work';
                var ed = this;
                var onload = function (req) {
                    var data = req.getText();
                    if (data !== null && typeof data !== 'undefined' && data.length > 0) {
                        var parseData;
                        try {
                            parseData = JSON.parse(data);
                            if (typeof parseData !== 'undefined') {

                                ed.saveInfoCell(cell, parseData);

                                if (typeof callback === 'function') {
                                    callback(parseData);
                                }
                            }
                        }
                        catch (ex) {
                            console.log('error parse : ', ex);
                        }
                    }
                    else if (typeof callback === 'function') {
                        callback();
                    }
                }
                var onerror = function (req) {
                    console.log(req.getStatus());
                }
                new mxXmlRequest(this.loadCellInfoPath, 'idcell=' +
                    cell.id + '&idtruc=' + dataid + '&filename=' + code + '&type=subwork').send(onload, onerror);
            }
        }
    }
}

EditorUi.prototype.getInfoCell = function (cell) {

    var dataReturn;
    var dataid = '0';

    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    if (typeof dataid !== 'undefined') {
        if (cell.isSubWork === '1' || cell.isSubWork === 1) {
            //for load data cell
            var code = EditorUi.prototype.lastWorkDocument;
            if (typeof code === 'undefined' || code.length === 0)
                code = 'Main-Work';

            var arr = EditorUi.prototype.cellMapping;

            if (typeof arr !== 'undefined' && arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.toString() === cell.id.toString() && arr[i].datadesign === code + '*' + dataid) {
                        dataReturn = arr[i].datavalue;
                        break;
                    }
                }
            }
        }
    }

    return dataReturn;
}

EditorUi.prototype.showInfoCell = function (cell) {
    if (this.editor.outlineWindow === null || typeof this.editor.outlineWindow === 'undefined')
        return;
    if (this.editor.outlineWindow.isVisible() === false)
        return;

    if (typeof cell === 'undefined' || cell === null)
        cell = EditorUi.prototype.lastClickElement || EditorUi.prototype.lastClickEdge;
    if (typeof cell === 'undefined' || cell === null)
        return;

    var ui = this;

    var datacell;
    if (cell.isEdge() === false) {
        if (cell.isSubWork === '1' || cell.isSubWork === 1)
            datacell = ui.getInfoCell(cell);
        else if (typeof cell.dataCode === 'undefined')
            datacell = ui.getInfoWork(cell);
    }

    /*
    if (cell.isSubWork === true || cell.isSubWork === 1) {
    
    
    }
    */

    if (datacell !== null && typeof datacell !== 'undefined') {
        var data = {
            isEdge: cell.isEdge(),
            targetValue: cell.target !== null ? cell.target.value : '',
            sourceValue: cell.source !== null ? cell.source.value : '',
            isMain: cell.isSource || cell.isTarget,
            isSubWork: cell.isSubWork,
            value: cell.value
        };
        var obj = { dataobj: data, cellobj: datacell };

        ui.editor.displayInfoCell(obj);
        /* show info+
        if (cell.isSubWork === '1' || cell.isSubWork === 1) { }
        else
        ui.showProductionProperty(cell);
        */
    }
    else {
        if (cell.isSubWork === '1' || cell.isSubWork === 1) {
            ui.loadInfoCell(cell, function (parseData) {

                var data = {
                    isEdge: cell.isEdge(),
                    targetValue: cell.target !== null ? cell.target.value : '',
                    sourceValue: cell.source !== null ? cell.source.value : '',
                    isMain: cell.isSource || cell.isTarget,
                    isSubWork: cell.isSubWork,
                    value: cell.value
                };
                var obj = { dataobj: data, cellobj: parseData };

                ui.editor.displayInfoCell(obj);
            });
        }
        else if (cell.isEdge() === false && typeof cell.dataCode === 'undefined') {
            ui.loadInfoWork(cell, function (parseData) {

                var data = {
                    isEdge: cell.isEdge(),
                    targetValue: cell.target !== null ? cell.target.value : '',
                    sourceValue: cell.source !== null ? cell.source.value : '',
                    isMain: cell.isSource || cell.isTarget,
                    isSubWork: cell.isSubWork,
                    value: cell.value
                };
                var obj = { dataobj: data, cellobj: parseData };

                ui.editor.displayInfoCell(obj);
            });
        }
        else {
            var data = {
                isEdge: cell.isEdge(),
                targetValue: cell.target !== null ? cell.target.value : '',
                sourceValue: cell.source !== null ? cell.source.value : '',
                isMain: cell.isSource || cell.isTarget,
                isSubWork: cell.isSubWork,
                value: cell.value
            };
            var obj = { dataobj: data, cellobj: undefined };
            ui.editor.displayInfoCell(obj);
        }
    }
}


EditorUi.prototype.processUpdateMapped = function () {
    var dataid = '0';

    var activeMachine = this.sidebar.getActiveMachinery();
    if (activeMachine !== null)
        dataid = activeMachine.getAttribute('dataid');

    if (typeof dataid !== 'undefined') {
        //for load data cell
        var code = EditorUi.prototype.lastWorkDocument;
        if (typeof code === 'undefined' || code.length === 0)
            code = 'Main-Work';
        var arr = EditorUi.prototype.cellMapping;
        if (typeof arr === 'undefined' || arr === null)
            arr = [];
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].datavalue.id.toString() === '0')
                    arr[i].datavalue.id = arr[i].id.toString();
            }
            EditorUi.prototype.cellMapping = arr;

        }

        arr = EditorUi.prototype.WorkPropertiesMapping;
        if (typeof arr === 'undefined' || arr === null)
            arr = [];
        if (arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].datavalue.id.toString() === '0')
                    arr[i].datavalue.id = arr[i].id.toString();
            }
            EditorUi.prototype.WorkPropertiesMapping = arr;
        }
    }
}

EditorUi.prototype.processSaveInfoCell = function (objsave, dataid) {
    if (typeof dataid === 'undefined') {
        dataid = '0';

        var activeMachine = this.sidebar.getActiveMachinery();
        if (activeMachine !== null)
            dataid = activeMachine.getAttribute('dataid');
    }

    //for load data cell
    var code = EditorUi.prototype.lastWorkDocument;
    if (typeof code === 'undefined' || code.length === 0)
        code = 'Main-Work';
    var arrSub = EditorUi.prototype.cellMapping;
    if (typeof arrSub === 'undefined' || arrSub === null)
        arrSub = [];
    var arrWork = EditorUi.prototype.WorkPropertiesMapping;
    if (typeof arrWork === 'undefined' || arrWork === null)
        arrWork = [];
    if (arrSub.length > 0 || arrWork.length > 0) {
        var arraySubSave = [];
        var arrayWorkSave = [];
        if (arrSub.length > 0) {
            for (var i = 0; i < arrSub.length; i++) {
                if (arrSub[i].datavalue.id.toString() === '0')
                    arraySubSave.push(arrSub[i]);
            }
        }

        if (arrWork.length > 0) {
            for (var i = 0; i < arrWork.length; i++) {
                if (arrWork[i].datavalue.id.toString() === '0')
                    arrayWorkSave.push(arrWork[i]);
            }
        }

        if (arraySubSave.length > 0 || arrayWorkSave.length > 0) {

            var ed = this;
            document.getElementById('page_spinner').style.display = 'block';
            var onload = function (req) {
                var data = req.getText();
                if (typeof data !== 'undefined' && data.length > 0) {
                    setTimeout(function () {
                        document.getElementById('page_spinner').style.display = 'none';
                    }, 500);
                    if (data === '1')
                        ed.processUpdateMapped();
                }
            }

            var onerror = function (req) {
                console.log(req.getStatus());
                document.getElementById('page_spinner').style.display = 'none';
            }

            new mxXmlRequest(ed.saveCellInfoPath, 'filename=' + code +
                    '&IdTruc=' + dataid +
                    '&cellsub=' + JSON.stringify(arraySubSave) +
                    '&cellwork=' + JSON.stringify(arrayWorkSave)).send(onload, onerror);

        }
    }
}

EditorUi.prototype.processShowSelectWork = function () {
    var div = document.createElement('div');
    div.className = 'abc';
    var sb = '<section id="connected">\
        <div class="left">\
        <span class="titleleft">' + SweetSoftScript.ResourceText.listWorktask + '</span>\
		<ul class="connected list"></ul>\
        </div>\
        <div class="right">\
        <span class="titleright">' + SweetSoftScript.ResourceText.worktaskofSubwork + '</span>\
		<ul class="connected list no2"></ul>\
        </div>\
        <div class="clear"></div>';
    //permissison
    if (this.checkEnable()) {
        sb += '<input type="button" id="btnSelect" value="' + SweetSoftScript.ResourceText.btnSelect +
        '"/><input type="button" id="btnApply" value="' + SweetSoftScript.ResourceText.btnApply +
        '"/><input type="button" id="btnRemoveSelect" value="' + SweetSoftScript.ResourceText.btnRemoveSelect + '"/>';
    }
    sb += '</section>';
    div.innerHTML = sb;

    var ed = this;

    var dataPalette = ed.sidebar.getVisiblePalette();
    if (dataPalette.length > 0) {

        var workcoll = dataPalette[1].childNodes[0].childNodes;
        if (workcoll.length > 0) {
            var arrselected = [];

            var dataCell = ed.getInfoCell(EditorUi.prototype.lastClickElement);

            if (typeof dataCell !== 'undefined') {
                var selected = '';
                if (dataCell.arr !== null && typeof dataCell.arr !== 'undefined' &&
                    dataCell.arr.length > 0) {
                    for (var i = 0; i < dataCell.arr.length; i++) {
                        var pos = -1;
                        for (var j = 0; j < workcoll.length; j++) {
                            if (workcoll[j].getAttribute('dataid') === dataCell.arr[i].toString()) {
                                pos = j;
                                break;
                            }
                        }
                        if (pos !== -1) {
                            arrselected.push(workcoll[pos].getAttribute('dataid'));
                            selected += '<li dataid="' + workcoll[pos].getAttribute('dataid') + '"><label><input type="checkbox"><span>' +
                                $.trim($(workcoll[pos]).text()) + '</span></label></li>';
                        }
                    }
                }
                if (selected.length > 0) {
                    selected = '<li><label><input type="checkbox" id="deselectall"><span>' + SweetSoftScript.ResourceText.btnSelectAll + '</span></label></li>' + selected;
                    $(div).find('ul.connected:eq(1)').html(selected);

                    $(div).find('#deselectall').change(function () {
                        var isCheck = $(this).is(':checked');
                        var ul = $(this).parent().parent().parent();
                        if (isCheck) {

                            ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', true);
                        }
                        else {
                            ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', false);
                        }
                    });

                    $(div).find('.connected:eq(1) > li:gt(0) input[type="checkbox"]').change(function () {
                        var ul = $(this).parent().parent().parent();
                        var countinput = ul.find('> li:gt(0) input[type="checkbox"]').length;
                        var isCheck = $(this).is(':checked');
                        if (isCheck) {
                            if (ul.find('> li:gt(0) input[type="checkbox"]:checked').length === countinput)
                                ul.find('li:eq(0) input[type="checkbox"]').prop('checked', true);
                        }
                        else {
                            if (ul.find('> li:gt(0) input[type="checkbox"]:checked').length !== countinput)
                                ul.find('li:eq(0) input[type="checkbox"]').prop('checked', false);
                        }
                    });
                }
            }

            var allwork = '';
            for (var i = 0; i < workcoll.length; i++) {
                if (arrselected.indexOf(workcoll[i].getAttribute('dataid')) < 0)
                    allwork += '<li dataid="' + workcoll[i].getAttribute('dataid') + '"><label><input type="checkbox"><span>'
                        + $.trim($(workcoll[i]).text()) + '</span></label></li>';
            }
            if (allwork.length > 0) {
                allwork = '<li><label><input type="checkbox" id="selectall"><span>' + SweetSoftScript.ResourceText.btnSelectAll + '</span></label></li>' + allwork;
                $(div).find('ul.connected:eq(0)').html(allwork);

                $(div).find('#selectall').change(function () {
                    var isCheck = $(this).is(':checked');
                    var ul = $(this).parent().parent().parent();
                    if (isCheck) {
                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', true);
                    }
                    else {
                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', false);
                    }
                });

                $(div).find('.connected:eq(0) > li:gt(0) input[type="checkbox"]').change(function () {
                    var ul = $(this).parent().parent().parent();
                    var countinput = ul.find('> li:gt(0) input[type="checkbox"]').length;
                    var isCheck = $(this).is(':checked');
                    if (isCheck) {
                        if (ul.find('> li:gt(0) input[type="checkbox"]:checked').length === countinput)
                            ul.find('li:eq(0) input[type="checkbox"]').prop('checked', true);
                        else {
                        }
                    }
                    else {
                        if (ul.find('> li:gt(0) input[type="checkbox"]:checked').length !== countinput)
                            ul.find('li:eq(0) input[type="checkbox"]').prop('checked', false);
                    }
                });
            }
        }
    }

    $(div).find('#btnApply').click(function () {
        var lstSelected = $('#connected .no2 > li:gt(0)');
        var selectedList = [];
        var dataCellInfo = ed.getInfoCell(EditorUi.prototype.lastClickElement);

        if (lstSelected.length > 0) {
            if (typeof dataCellInfo === 'undefined')
                dataCellInfo = { id: '0', arr: [] };
            lstSelected.each(function (i, o) {
                selectedList.push(o.getAttribute('dataid'));
            });

            //dataCellInfo.map.id=EditorUi.prototype.lastClickElement.id;


        }

        dataCellInfo.arr = selectedList;


        //save infocell
        ed.saveInfoCell(EditorUi.prototype.lastClickElement, dataCellInfo);

        setTimeout(function () {
            var btndetail = document.getElementById('btnShowRelated');
            if (btndetail != null)
                ed.showRelatedWork(undefined, btndetail.parentNode.childNodes[0]);
        }, 300);

        dig.close();
        return false;
    });

    $(div).find('#btnSelect').click(function () {
        var selected = $('#connected .connected:eq(0) > li:gt(0) input[type="checkbox"]:checked');
        if (selected.length > 0) {
            if ($('#connected .connected:eq(1) > li').length === 0) {
                var liall = $('<li><label><input type="checkbox" id="deselectall"><span>' + SweetSoftScript.ResourceText.btnSelectAll + '</span></label></li>');
                liall.find('#deselectall').change(function () {
                    var isCheck = $(this).is(':checked');
                    var ul = $(this).parent().parent().parent();
                    if (isCheck) {
                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', true);
                    }
                    else {

                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', false);
                    }
                });
                liall.appendTo($('#connected .connected:eq(1)'));
            }
            selected.each(function () {
                $(this).prop('checked', false).parent().parent().appendTo($('#connected .connected:eq(1)'));
            });

            if ($('#connected .connected:eq(0) > li').length === 1)
                $('#connected .connected:eq(0)').empty();
        }
        return false;
    });

    $(div).find('#btnRemoveSelect').click(function () {
        var selected = $('#connected .connected:eq(1) > li:gt(0) input[type="checkbox"]:checked');
        if (selected.length > 0) {
            if ($('#connected .connected:eq(0) > li').length === 0) {
                var liall = $('<li><label><input type="checkbox" id="selectall"><span>' + SweetSoftScript.ResourceText.btnSelectAll + '</span></label></li>');
                liall.find('#selectall').change(function () {
                    var isCheck = $(this).is(':checked');
                    var ul = $(this).parent().parent().parent();
                    if (isCheck) {
                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', true);
                    }
                    else {
                        ul.find(' > li:gt(0) input[type="checkbox"]').prop('checked', false);
                    }
                });
                liall.appendTo($('#connected .connected:eq(0)'));
            }
            selected.each(function () {
                $(this).prop('checked', false).parent().parent().appendTo($('#connected .connected:eq(0)'));
            });

            if ($('#connected .connected:eq(1) > li').length === 1)
                $('#connected .connected:eq(1)').empty();
        }
        return false;
    });

    var dig = new SubworkDialog(this, div, 600, 400, function () {
        //alert('close');
    });

}

EditorUi.prototype.showRelatedWork = function (obj, span) {

    var ed = this;

    var dataCell = ed.getInfoCell(EditorUi.prototype.lastClickElement);

    if (typeof dataCell !== 'undefined'
        && dataCell.arr !== 'undefined' && dataCell.arr.length > 0) {
        if (dataCell.arr.length === 1) {
            var id = dataCell.arr[0];
            var dataPalette = ed.sidebar.getVisiblePalette();
            if (dataPalette.length > 0) {

                var workcoll = dataPalette[1].childNodes[0].childNodes;
                if (workcoll.length > 0) {
                    for (var i = 0; i < workcoll.length; i++) {
                        if (workcoll[i].getAttribute('dataid') === id.toString()) {
                            $(span).text($.trim($(workcoll[i]).text()));
                            break;
                        }
                    }
                }
            }
        }
        else {
            $(span).text(dataCell.arr.length);
        }
    }
    else
        $(span).text('0');
}


function Permission(locked, createEdges, editEdges, editVertices, cloneCells) {
    this.locked = (locked != null) ? locked : false;
    this.createEdges = (createEdges != null) ? createEdges : true;
    this.editEdges = (editEdges != null) ? editEdges : true;
    this.editVertices = (editVertices != null) ? editVertices : true;
    this.cloneCells = (cloneCells != null) ? cloneCells : true;
};

Permission.prototype.apply = function (graph) {
    graph.setCellsLocked(this.locked);
    graph.setConnectable(this.createEdges);
    graph.connectionHandler.setEnabled(this.createEdges);
    graph.setConnectableEdges(this.createEdges);
};
