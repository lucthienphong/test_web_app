<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="ScanCylinderBarcode.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.ScanCylinderBarcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <div style="margin-top: 5%;">
        <div class="col-md-5">
            <div class="ui-dialog ui-widget ui-widget-content ui-corner-all panel panel-primary ui-front ui-dialog-buttons ui-draggable"
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
                                            <span class="pull-left">Number item</span>
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList ID="ddlNumberItem" runat="server"
                                                data-style="btn btn-info btn-block"
                                                data-width="100%" data-live-search="false"
                                                data-toggle="dropdown"
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4">
                                            <span class="pull-left">Barcode</span>
                                        </label>
                                        <div class="col-sm-8">
                                            <div class="input-group" id="txtholder">
                                                <span class="input-group-addon">...</span>
                                                <input type="text" class="form-control" id="txtBarCode" placeholder="" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="ui-dialog-buttonpane ui-widget-content" style="margin: 0;">
                    <div class="container-fluid">
                        <div class="pull-right" style="margin-bottom: 15px">
                            <%--<button type="button" id="btnSBCOK" class="saveButtonClass btn btn-default btn-sm ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button">
                                <span class="ui-button-text">Ok</span>
                            </button>--%>
                            <button type="button" id="btnSBCCancel" class="cancelButtonClass btn btn-default btn-sm ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button">
                                <span class="ui-button-text">Clear</span>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-5 col-md-offset-2" style="display: none">
            <div class="panel panel-primary">
                <div class="panel-heading">Scanned list</div>
                <div class="panel-body">
                    <table class="table" id="tbllist">
                        <thead>
                            <tr>
                                <th>Index</th>
                                <th>Barcode</th>
                                <th>Department</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%--<tr>
                        <td>1</td>
                        <td>000-1223</td>
                        <td>Bangalore</td>
                    </tr>--%>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <%--for js know that this page no need alert when closing--%>
    <input type="hidden" id="hdfIgnore" name="hdfIgnore" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="Server">
    <script type="text/javascript">
        //var duplicateBarcode = '<%=SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.EXISTS)%>';
        var duplicateBarcode = 'Duplicate barcode !';

        if (typeof SweetSoftScript === 'undefined')
            SweetSoftScript = {};
        SweetSoftScript.jobbarcode = {
            showLoading: function (callback) {
                var div = $('#prepare_window_loading');
                if (div.length > 0) {
                    div.fadeIn('fast', function () {
                        if (typeof callback === 'function')
                            callback();
                    });
                }
                else if (typeof callback === 'function')
                    callback();
            },
            hideLoading: function (callback) {
                var div = $('#prepare_window_loading');
                if (div.length > 0) {
                    div.fadeOut('normal', function () {
                        if (typeof callback === 'function')
                            callback();
                    });
                }
                else if (typeof callback === 'function')
                    callback();
            },
            appendProcessData: function (obj, indx) {
                var tb = $('#tbllist');
                var divlist = tb.closest('.col-md-5');
                if (divlist.is(':hidden'))
                    divlist.show();
                tb.find('tbody').append('<tr>'
                    + '<td>' + indx + '</td>'
                    + '<td>' + obj.barcode + '</td>'
                    + '<td>' + obj.dept + '</td>'
                    + '<td>' + (((obj.status && obj.status.toLowerCase() === 'inprogress')) ? 'Cylinder In' : 'Cylinder Out') + '</td>'
                    + '</tr>');
            },
            processData: function (data, indx) {
                //console.log(data);
                if (data && data.length > 0) {
                    var obj;
                    var isScan = false;
                    if (data.indexOf('{') === 0) {
                        try {
                            console.log(data);
                            obj = JSON.parse(data);
                        }
                        catch (ex) {
                            console.log('not, ' + ex);
                        }
                        if (typeof obj !== 'undefined')
                            isScan = true;
                    }
                    else if (data.indexOf('[') >= 0) {
                        try {
                            console.log(data);
                            obj = eval(data);
                        }
                        catch (ex) {
                            console.log('not, ' + ex);
                        }
                    }

                    var numberItem = $('select[id$="ddlNumberItem"]').val();
                    if (typeof obj !== 'undefined') {
                        if (isScan === true)
                            SweetSoftScript.jobbarcode.appendProcessData(obj, indx);

                        //console.log('obj : ', obj);
                        //if (parseInt(numberItem) > 1) {


                        if (parseInt(numberItem) !== parseInt(indx))
                            $('#txtholder > span').text(parseInt(indx) + 1);
                        //}

                        if ($.isArray(obj) === true) {
                            if (obj.length > 1) {
                                //append last scan to table
                                SweetSoftScript.jobbarcode.appendProcessData(obj[obj.length - 1], indx);
                            }
                            else
                                SweetSoftScript.jobbarcode.appendProcessData(obj[0], 1);

                            parent.SweetSoftScript.mainFunction.OpenModalWindow(undefined,
                                'Scan barcode complete.', 'alert');

                            /*
                            //reset data
                            var btnCancel = $('#btnSBCCancel');
                            if (btnCancel.length > 0)
                                btnCancel.click();
                            */

                            $('#txtBarCode').val('');
                            $('#txtholder > span').text('...');
                            $('select[id$="ddlNumberItem"]')
                                .val('')
                                .prop('disabled', false)
                                .selectpicker('refresh');

                        }
                        else
                            $('#txtBarCode').val('').focus();
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
                            //else
                            //    parent.openWindow(undefined, parent.SweetSoftScript.ResourceText.JobInfo, '/pages/job.aspx?id=' + arr[1]);
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
            },
            getAllBarcode: function () {
                var code = '';
                var tb = $('#tbllist');
                var trColl = tb.find('tbody td:nth-child(2)');
                if (trColl.length > 0) {
                    trColl.each(function () {
                        code += $.trim($(this).text()) + ',';
                    });
                    var val = $('#txtBarCode').val();
                    if (val && val.length > 0)
                        code += val;
                    else {
                        if (code.length > 1)
                            code = code.substring(0, code.length - 1);
                    }
                }
                else
                    code = $('#txtBarCode').val();
                return code;
            },
            callOKFunction: function () {
                var numberItem = $('select[id$="ddlNumberItem"]').val();
                if (numberItem && numberItem.length > 0) {
                    // Save code here 
                    var val = $('#txtBarCode').val();
                    var indx = $('#txtholder > span').text();
                    if (val && val.length > 0) {

                        if (indx === '1') {
                            $('select[id$="ddlNumberItem"]')
                                .prop('disabled', true)
                                .selectpicker('refresh');
                        }

                        var isExist = false;
                        var tb = $('#tbllist');
                        var trColl = tb.find('tbody td:nth-child(2)');
                        if (trColl.length > 0) {
                            trColl.each(function () {
                                if ($.trim($(this).text()) == $.trim(val)) {
                                    isExist = true;
                                    return false;
                                }
                            });
                        }

                        if (isExist === true) {
                            parent.SweetSoftScript.mainFunction.OpenModalWindow(undefined, duplicateBarcode, 'alert', function () {
                                $('#txtBarCode').focus();
                            });
                        }
                        else {
                            if (parseInt(indx) === parseInt(numberItem))
                                val = SweetSoftScript.jobbarcode.getAllBarcode();

                            SweetSoftScript.jobbarcode.showLoading(function () {
                                parent.SweetSoftScript.commonFunction.ajaxRequest('/barcode/cylinder.aspx',
                                    'code=' + val + '&num=' + numberItem +
                                    '&indx=' + (indx && indx.length > 0 ? indx : ''),
                                    function (data) {
                                        SweetSoftScript.jobbarcode.hideLoading(function () {
                                            SweetSoftScript.jobbarcode.processData(data, indx);
                                        });
                                    });
                            });
                        }
                    }
                }
            },
            init: function () {
                SweetSoftScript.jobbarcode.initDropdown();
                SweetSoftScript.jobbarcode.initTxtBarCode();

                /*
                var btnOK = $('#btnSBCOK');
                if (btnOK.length > 0) {
                    btnOK.click(function () {
                        SweetSoftScript.jobbarcode.callOKFunction();
                    });
                }
                */

                var btnCancel = $('#btnSBCCancel');
                if (btnCancel.length > 0) {
                    btnCancel.click(function () {
                        $('#txtBarCode').val('');
                        $('#txtholder > span').text('...');
                        $('select[id$="ddlNumberItem"]')
                            .val('')
                            .prop('disabled', false)
                            .selectpicker('refresh');

                        var tb = $('#tbllist');
                        var divlist = tb.closest('.col-md-5');
                        if (divlist.is(':hidden') === false)
                            divlist.hide();
                        tb.find('tbody').empty();
                    });
                }
            },
            initTxtBarCode: function () {

                $('#txtBarCode').keypress(function (e) {
                    var theEvent = e || window.event;
                    var key = theEvent.keyCode || theEvent.which;
                    key = String.fromCharCode(key);

                    if (theEvent.keyCode === 13) {
                        e.preventDefault();

                        SweetSoftScript.jobbarcode.callOKFunction();
                        return false;
                    }
                });
                setTimeout("$('#txtBarCode').focus();", 200);

            },
            initDropdown: function () {
                var sel = $('select[id$="ddlNumberItem"]');
                if (sel.length > 0) {
                    sel.change(function () {
                        var val = $(this).val();
                        if (val && val.length > 0) {
                            $('#txtBarCode').val('').focus();
                            $('#txtholder > span').text('1');
                        }
                        else
                            $('#txtholder > span').text('...');

                        var tb = $('#tbllist');
                        var divlist = tb.closest('.col-md-5');
                        if (divlist.is(':hidden') === false)
                            divlist.hide();
                        tb.find('tbody').empty();
                    }).trigger('change');
                }
            }
        };

        $(function () {
            SweetSoftScript.jobbarcode.init();
        });

    </script>
</asp:Content>
