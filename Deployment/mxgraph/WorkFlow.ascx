<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="SweetSoft.APEM.WebApp.Controls.WorkFlow" Codebehind="WorkFlow.ascx.cs" %>
<link href="../mxgraph/css/grapheditor.css" rel="stylesheet" />
<link href="../mxgraph/css/custom.css" rel="stylesheet" type="text/css" />
<link href="../css/bootstrap.min.css" rel="stylesheet" />
<style type="text/css">
    .btntest {
        position: absolute;
        left: 230px;
        top: 100px;
        z-index: 11;
    }

    .geItem.active {
        background: #B8E281;
    }

    .titletruc {
        position: absolute;
        right: 0;
        font-weight: bold;
        font-size: 15px;
        float: right;
        margin-right: 20px;
        max-width: 500px;
        display: inline-block;
        white-space: normal;
        z-index: 11;
        top: 4px;
    }

    .label {
        color: #0445BB;
    }
</style>

<div class="modal fade" id="wfMessage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div id="MessageHeader" class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="lblMessageTitle" runat="server"></h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <span id="lbConfirmTitle"></span>
            </div>
            <div class="modal-footer">
                <input type="button" class="btn btn-default" id="btnConfirmSubmit" runat="server" />
                <input type="button" class="btn btn-default" id="btnSaveAndContinue" runat="server" />
                <input type="button" class="btn btn-default" id="btnConfirmClose" runat="server" />
            </div>
        </div>
    </div>
</div>


<script type="text/javascript">
    // Public global variables
    var MAX_REQUEST_SIZE = 10485760;
    var MAX_AREA = 10000 * 10000;
    var baseUrlAjax = location.protocol + '//' + location.host + '/Pages/';
    var baseUrl = location.protocol + '//' + location.host + '/mxgraph/';
    // URLs for save and export
    var EXPORT_URL = baseUrl + 'export.ashx';
    var SAVE_URL = baseUrl + 'savefinal.ashx';
    var RESOURCES_PATH = '../mxgraph/src/resources';
    var RESOURCE_BASE = RESOURCES_PATH + '/grapheditor';

    var STENCIL_PATH = '../mxgraph/stencils';
    var IMAGE_PATH = '../mxgraph/src/images';
    var STYLE_PATH = '../mxgraph/css';
    var CSS_PATH = '../mxgraph/css';
    var OPEN_FORM = 'open.html';

    // Specifies connection mode for touch devices (at least one should be true)
    var tapAndHoldStartsConnection = false;
    var showConnectorImg = true;

    // Sets the base path, the UI language via URL param and configures the
    // supported languages to avoid 404s. The loading of all core language
    // resources is disabled as all required resources are in grapheditor.
    // properties. Note that in this example the loading of two resource
    // files (the special bundle and the default bundle) is disabled to
    // save a GET request. This requires that all resources be present in
    // each properties file since only one file is loaded.
    //mxLoadResources = false;
    mxBasePath = '../mxgraph/src';
    var langId = '<%=SweetSoft.CMS.Common.LanguageHelper.CurrentLanguageCode%>';
    mxLanguages = [langId === '1' ? "en-US" : "vi"];
    mxLanguage = langId === '1' ? "en-US" : "vi";

    function CheckPageWhenClose() {
        return true;
    }

</script>

<script src="../mxgraph/src/js/mxClient-2.5.0.2.js"></script>

<script type="text/javascript" src="../js/core/jquery.min.js"></script>
<script type="text/javascript" src="../js/core/bootstrap.min.js"></script>
<script type="text/javascript" src="../mxgraph/js/Editor-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Graph-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/EditorUi-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Actions-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Menus-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Sidebar-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Toolbar-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/Dialogs-2.4.1.0.js"></script>
<script type="text/javascript" src="../mxgraph/js/jscolor/jscolor.js"></script>
<script type="text/javascript" src="../mxgraph/js/json2.js"></script>
<script type="text/javascript" src="../mxgraph/js/SweetSoftScript.js"></script>
<script type="text/javascript" src="../mxgraph/js/SweetSoftScript-<%=SweetSoft.CMS.Common.LanguageHelper.CurrentLanguageCode==1?"en":"vi"%>.js"></script>

<asp:Literal ID="ltrDataTruc" runat="server" EnableViewState="false"></asp:Literal>
<asp:Literal ID="ltrDataPB" runat="server" EnableViewState="false"></asp:Literal>
<asp:Literal ID="ltrDataGraphictype" runat="server" EnableViewState="false"></asp:Literal>
<asp:Literal ID="ltrDataMainWF" runat="server" EnableViewState="false"></asp:Literal>

<script type="text/javascript">
    SweetSoftScript.Setting.agreeBtnId = '<%= btnConfirmSubmit.ClientID%>';
    SweetSoftScript.Setting.closeBtnId = '<%= btnConfirmClose.ClientID%>';
    SweetSoftScript.Setting.saveBtnId = '<%= btnSaveAndContinue.ClientID%>';
    SweetSoftScript.Setting.labelMessage = 'lbConfirmTitle';
</script>

<script type="text/javascript" src="../mxgraph/js/main.js"></script>

