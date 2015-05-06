<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="SweetSoft.APEM.WebApp.barcode.JobProcessing" Codebehind="JobProcessing.ascx.cs" %>
<!--dialog  -->
<div id="dialog-form-scanbarcode" title="Scan Barcode">
    <div class="container-fluid">
        <div class="row" style="margin-top: 15px;">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-sm-4">
                            <span class="pull-left">Barcode</span></label>
                        <div class="col-sm-8">
                            <input type="text" id="txtBarCode" value="" class="form-control" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="MessageModel" tabindex="-1" role="dialog"
    aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div id="MessageHeader" class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">
                    <span id="spTitle"></span>
                </h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <div id="spMessage"></div>
                <div class="html"></div>
            </div>
            <div class="modal-footer">
                <input type="button" class="btn btn-default" id="btnConfirmSubmit" />
                <input type="button" class="btn btn-default" id="btnSaveAndContinue" />
                <input type="button" class="btn btn-default" id="btnConfirmClose" />
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="selectDepartment" tabindex="-1" role="dialog"
    aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">
                    <span id="spSelectDepartmentTitle">Select a department</span>
                </h4>
            </div>
            <div class="modal-body" style="text-align: center;">
                <select id="mdddlDepartment" class="form-control"></select>
            </div>
            <div class="modal-footer">
                <input type="button" class="btn btn-default" id="mdOK" />
                <input type="button" class="btn btn-default" id="mdClose" />
            </div>
        </div>
    </div>
</div>
