/// <reference path="SweetSoftScript.js" />
window.onload = function () {
    document.getElementsByTagName('body')[0].className = 'geEditor';
    SweetSoftScript.commonFunction.initLoad();
    if (window.parent !== null) {
        if (typeof window.parent.ClosedfLoading === "function")
            window.parent.ClosedfLoading();
    }
}

// Extends EditorUi to update I/O action states
//(function() {
var editorUiInit = EditorUi.prototype.init;

EditorUi.prototype.init = function () {
    /*
    this.actions.get('export').setEnabled(false);

    // Updates action states which require a backend
    if (!useLocalStorage) {
    mxUtils.post(SAVE_URL, '', mxUtils.bind(this, function(req) {
    var enabled = req.getStatus() != 404;
    this.actions.get('open').setEnabled(enabled || fileSupport);
    this.actions.get('import').setEnabled(enabled || fileSupport);
    this.actions.get('save').setEnabled(enabled);
    this.actions.get('saveAs').setEnabled(enabled);
    this.actions.get('export').setEnabled(enabled);
    }));
    }
    */

    if (typeof add_main_workflow !== 'undefined')
        SweetSoftScript.Setting.add_main_workflow = add_main_workflow;
    if (typeof reset_workflow !== 'undefined')
        SweetSoftScript.Setting.reset_workflow = reset_workflow;
    if (typeof delete_workflow !== 'undefined')
        SweetSoftScript.Setting.delete_workflow = delete_workflow;


    var ed = this;
    var hdf = jQuery('input[type="hidden"][id$="hdfAuthorize"]');

    if (hdf.length > 0) {
        if (hdf.val().length > 0) {
            try {
                var objEncrypt = JSON.parse(hdf.val().replace(/\s/g, ''));

                if (typeof objEncrypt !== 'undefined') {
                    ed.objEncrypt = objEncrypt;

                }
            }
            catch (ex) {

            }
        }
        hdf.remove();
    }

    try {
        //special graphictype to show
        ed.externalType = eval(jQuery('input[type="hidden"][id$="hdfExternalType"]').val());
    }
    catch (ex) {

    }

    editorUiInit.apply(ed, arguments);
};

//})();

var editorMain = new EditorUi(new Editor());
editorMain.loadFilePath = baseUrl + 'loaddata.ashx';
editorMain.loadCellInfoPath = baseUrl + 'getinfocell.ashx';
editorMain.saveCellInfoPath = baseUrl + 'saveinfocell.ashx';
editorMain.deletePath = baseUrl + 'deleteHandler.ashx';
editorMain.sidebar.loadWorkflowPath = baseUrl + 'getwork.ashx';
editorMain.sidebar.loadMainWorkPath = baseUrl + 'getmachinery.ashx';
//editorMain.openFilePath('Main-Work.xml', true);

//add custom Palette
var dataMachinery = [];

/*#region truc*/
var arr = [];

if (typeof dataTruc !== 'undefined') {
    for (var i = 0; i < dataTruc.length; i++) {
        arr.push({
            title: dataTruc[i].Name,
            fillColor: '#0000FF',
            fontColor: '#ffffff',
            isEdge: false,
            isWrapText: false,
            dataId: dataTruc[i].Id,
            dataCode: '',
            type: 'truc'
        });
    }

    if (typeof dataGraphictype !== 'undefined') {
        for (var i = 0; i < dataGraphictype.length; i++) {
            arr.push({
                title: dataGraphictype[i].Name,
                fillColor: '#0000FF',
                fontColor: '#ffffff',
                isEdge: false,
                isWrapText: false,
                dataId: dataGraphictype[i].Id,
                dataCode: '',
                type: 'graphic'
            });
        }
    }

    var truc = {
        id: 'truc',
        title: SweetSoftScript.ResourceText.listMachinery,
        isExpand: true,
        items: arr,
        setting: {
            thumbWidth: 176,
            thumbHeight: 5,
            scaleThumb: false,
            sidebarTitleSize: 12,
            showThumbImage: false,
            enableTooltips: false
        }
    };

    dataMachinery.push(truc);
}

/*#endregion truc*/

/*#region phong ban*/
arr = [];

if (typeof dataDepts !== 'undefined') {
    for (var i = 0; i < dataDepts.length; i++) {
        arr.push({
            title: dataDepts[i].Name,
            fillColor: '#0000FF',
            fontColor: '#ffffff',
            isEdge: false,
            isWrapText: false,
            dataId: dataDepts[i].Id,
            dataCode: dataDepts[i].Code
        });
    }

    var depts = {
        id: 'depts',
        title: SweetSoftScript.ResourceText.listDept,
        isExpand: true,
        items: arr,
        setting: {
            thumbWidth: 176,
            thumbHeight: 5,
            scaleThumb: false,
            sidebarTitleSize: 12,
            showThumbImage: false,
            enableTooltips: false
        }
    };

    dataMachinery.push(depts);
}
/*#endregion*/


//set custom palette

editorMain.sidebar.customMachinery = dataMachinery;
//then call
editorMain.sidebar.addCustomMachinery();

//editorMain.sidebar.hidePalette('depts');
//editorMain.sidebar.hideMachinery('graphictype');
//editorMain.sidebar.hideMachinery('truc');
editorMain.setHeightDepts();
setTimeout(function () {
    editorMain.sidebar.showOnlyMainWork();
}, 500);
//add generate
//editorMain.sidebar.addGeneralPalette(true);
//editorMain.sidebar.setHeightVisibleMachinery();