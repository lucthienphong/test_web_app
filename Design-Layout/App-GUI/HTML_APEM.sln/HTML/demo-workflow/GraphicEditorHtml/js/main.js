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
   
    editorUiInit.apply(this, arguments);
};

//})();

var editorMain = new EditorUi(new Editor());
editorMain.loadFilePath = baseUrl + 'loaddata.ashx';
editorMain.loadCellInfoPath = baseUrl + 'getinfocell.ashx';
editorMain.saveCellInfoPath = baseUrl + 'saveinfocell.ashx';
editorMain.deletePath = baseUrl + 'deleteHandler.ashx';
editorMain.sidebar.loadWorkflowPath = baseUrl + 'getwork.ashx';
editorMain.sidebar.loadTrucPath = baseUrl + 'data/data.json';
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
            dataCode: ''
        });
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

/*#endregion depts*/

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
editorMain.sidebar.hideMachinery('truc');

setTimeout(function () {
    editorMain.sidebar.loadMachineryWorkflow(0);
}, 500);
//add generate
//editorMain.sidebar.addGeneralPalette(true);        
//editorMain.sidebar.setHeightVisibleMachinery();
