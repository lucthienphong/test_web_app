<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
     AutoEventWireup="True" CodeBehind="ScanJobBarcode.aspx.cs" 
    Inherits="SweetSoft.APEM.WebApp.Pages.ScanJobBarcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        body, html{
            height:100%;
        }
        body{
            background:url(../img/background.gif) no-repeat 0 0;
            background-size:cover;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
        <div class="col-md-4 col-md-offset-4" style="margin-top: 5%;">
            <div class="ui-dialog ui-widget ui-widget-content panel panel-primary ui-corner-all ui-front ui-dialog-buttons ui-draggable"
                tabindex="-1" role="dialog" aria-describedby="dialog-form-scanbarcode"
                aria-labelledby="ui-id-1" style="position: relative; z-index: 1">
                <div class="panel-heading ui-widget-header ui-corner-all" style="cursor: default">
                    <span class="ui-dialog-title">Scan Barcode</span>
                </div>
                <div id="dialog-form-scanbarcode" class="ui-dialog-content ui-widget-content">
                    <div class="container-fluid">
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4">
                                            <span class="pull-left">Barcode</span>
                                        </label>
                                        <div class="col-sm-8">
                                            <input type="text" id="txtBarCode" value="" class="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="ui-dialog-buttonpane ui-widget-content" style="margin-top: 0">
                    <div class="container-fluid">
                        <div class="pull-right" style="margin-bottom: 15px">
                            <button type="button" id="btnSBCOK" class="saveButtonClass btn btn-default btn-sm ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button">
                                <span class="ui-button-text">Ok</span>
                            </button>
                            <%--<button type="button" id="btnSBCCancel" class="cancelButtonClass btn btn-default btn-sm ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button">
                            <span class="ui-button-text">Cancel</span>
                        </button>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <%--for js know that this page no need alert when closing--%>
    <input type="hidden" id="hdfIgnore" name="hdfIgnore" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" Runat="Server">
    <script type="text/javascript">
        if (typeof SweetSoftScript === 'undefined')
            SweetSoftScript = {};
        SweetSoftScript.jobbarcode = {
            init: function () {

                var btnOK = $('#btnSBCOK');
                if (btnOK.length > 0) {
                    btnOK.click(function () {
                        // Save code here 
                        var val = $('#txtBarCode').val();
                        if (val && val.length > 0) {
                            parent.SweetSoftScript.commonFunction.ajaxRequest('/barcode/job.aspx',
                                   'code=' + val, function (data) {
                                       //console.log(data);

                                       if (data && data.length > 0) {
                                           var obj;
                                           if (data.indexOf('[{') >= 0) {
                                               try {
                                                   obj = eval(data);

                                               }
                                               catch (ex) {
                                                   console.log('not, ' + ex);
                                               }
                                           }

                                           if (typeof obj !== 'undefined') {
                                               console.log('obj : ', obj);
                                           }
                                           else {
                                               var arr = data.split('||');

                                               if ($.trim(arr[0]) === '1') {
                                                   var num = parseInt(arr[1]);
                                                   if (isNaN(num)) {
                                                       $('#txtBarCode').val('');
                                                       parent.SweetSoftScript.mainFunction.OpenModalWindow(undefined, arr[1], 'alert');
                                                       if (arr.length === 3)
                                                           parent.openWindow(undefined, parent.SweetSoftScript.ResourceText.JobInfo, '/pages/job.aspx?id=' + arr[2]);
                                                   }
                                                   else
                                                       parent.openWindow(undefined, parent.SweetSoftScript.ResourceText.JobInfo, '/pages/job.aspx?id=' + arr[1]);
                                               }
                                               else {
                                                   parent.SweetSoftScript.mainFunction.OpenModalWindow(undefined, arr[0],
                                                       'confirmRetype', function () {
                                                           $('#txtBarCode').val('').focus();
                                                       }, function () {

                                                       },
                                                    parent.SweetSoftScript.ResourceText.Retype);
                                               }
                                           }
                                       }
                                       else {
                                           parent.SweetSoftScript.mainFunction.OpenModalWindow(undefined, parent.SweetSoftScript.ResourceText.ErrorProcess, 'alert');
                                       }
                                   });
                        }
                    });
                }

                setTimeout("$('#txtBarCode').focus();", 200);

                $('#txtBarCode').keypress(function (e) {
                    var theEvent = e || window.event;
                    var key = theEvent.keyCode || theEvent.which;
                    key = String.fromCharCode(key);

                    if (theEvent.keyCode === 13) {
                        e.preventDefault();
                        var btnok = $('#btnSBCOK');
                        if (btnok.length > 0)
                            btnok.click();
                        return false;
                    }
                });
            }
        };

        $(function () {
            SweetSoftScript.jobbarcode.init();
        });
    </script>
</asp:Content>
