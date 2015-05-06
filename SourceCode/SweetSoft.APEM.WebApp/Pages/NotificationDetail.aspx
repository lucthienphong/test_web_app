<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="True" CodeBehind="NotificationDetail.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.NotificationDetail" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/Controls/NotificationSetting.ascx" TagPrefix="SweetSoft" TagName="NotificationSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .dataTable
        {
            margin-top: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:LinkButton class="btn btn-transparent" ID="btnNSave"
                        runat="server" OnClick="btnNSave_Click">
                                <span class="flaticon-floppy1"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%></asp:LinkButton>

                    <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-transparent">
                                <span class="flaticon-back57"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURTO_LIST)%></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row row_content">
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NOTIFICATION)%>
                </label>
                <div class="row">
                    <div class="col-sm-3 col-xs-4">
                        <SweetSoft:CustomExtraTextbox ID="txtId" ReadOnly="true" RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                    <div class="col-sm-9 col-xs-8">
                        <SweetSoft:CustomExtraTextbox ID="txtTitle" RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="tabbable">
                <ul class="nav nav-tabs" role="tablist">
                    <li class="active"><a href="#insert" role="tab" data-toggle="tab">
                        <strong>Insert
                        </strong>
                    </a></li>
                    <li><a href="#update" role="tab" data-toggle="tab">
                        <strong>Update
                        </strong>
                    </a></li>
                    <li><a href="#delete" role="tab" data-toggle="tab">
                        <strong>Delete
                        </strong>
                    </a></li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="tab-content">
                <div class="tab-pane active" id="insert">
                    <div class="container-fluid">
                        <SweetSoft:NotificationSetting runat="server" CommandType="insert" ID="NotificationSettingInsert" />
                    </div>
                </div>
                <div class="tab-pane " id="update">
                    <div class="container-fluid">
                        <SweetSoft:NotificationSetting runat="server" CommandType="update" ID="NotificationSettingUpdate" />
                    </div>
                </div>
                <div class="tab-pane " id="delete">
                    <div class="container-fluid">
                        <SweetSoft:NotificationSetting runat="server" CommandType="delete" ID="NotificationSettingDelete" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="Server">
    <script type="text/javascript">
        function CallChangeCheckbox(el) {
            var ischeck = $(el).is(':checked');
            var div = $(el).closest('.form-group').next();
            //console.log(div);
            if (div.length > 0) {
                if (ischeck) {
                    var sel = $('select[id$="ddlAvailablePage"]');
                    var divother = sel.closest('.form-group').next();
                    if (sel.length > 0) {
                        var val = sel.val();
                        if ($.trim(val) !== '-1') {
                            if (divother.length > 0)
                                divother.hide();
                        }
                        else {
                            if (divother.length > 0 && sel.is(':visible')) {
                                divother.show().find('input[type="text"]').focus();
                            }
                        }
                    }
                    div.show();
                }
                else {
                    div.hide();
                    $('#divother').hide();
                }
            }
        }

        function CallChangeSelect(sel) {
            var div = $(sel).closest('.form-group').next();
            var val = $(sel).val();
            if ($.trim(val) !== '-1') {
                if (div.length > 0)
                    div.hide();
            }
            else {
                if (div.length > 0) {
                    div.show().find('input[type="text"]').focus();
                }
            }
        }

        function InitJS(event, args) {
            //console.log('event : ', event);
            //console.log('args : ', args);
            if (event) {
                var elem = event._postBackSettings.sourceElement;
                if (elem && elem.id && elem.id.length > 0) {
                    if (elem.id.indexOf('btnNDelete') >= 0) {
                        if ($('#hdfdone').length > 0) {
                            if ($('#hdfdone').val() === '1') {
                                parent.SweetSoftScript.mainFunction.OpenModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                    parent.SweetSoftScript.ResourceText.DeleteNotificaitonSettingDone, 'alert');
                            }
                            else {
                                parent.SweetSoftScript.mainFunction.OpenModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                   parent.SweetSoftScript.ResourceText.ErrorDeleteNotificaitonSetting, 'alert');
                            }
                            $('#hdfdone').remove();
                        }
                        else {
                            parent.SweetSoftScript.mainFunction.OpenModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                parent.SweetSoftScript.ResourceText.NotExistNotificaitonSetting, 'alert');
                        }
                    }
                    else if (elem.id.indexOf('btnNSave') >= 0) {
                        parent.SweetSoftScript.mainFunction.OpenModalWindow(parent.SweetSoftScript.ResourceText.notice,
                               parent.SweetSoftScript.ResourceText.SaveNotificaitonSetting, 'alert');
                    }
                }
            }

            var chkacion = $('#mainright input[type="checkbox"]').not('.chk input[type="checkbox"],div[id$="upMainSelect"] .chkdept input[type="checkbox"]');
            //console.log(chkacion);
            if (chkacion.length > 0) {
                chkacion.change(function () {
                    CallChangeCheckbox(this);
                });
                chkacion.each(function () {
                    CallChangeCheckbox(this);
                });
            }

            var sel = $('select[id$="ddlAvailablePage"]');
            if (sel.length > 0) {
                sel.change(function () {
                    CallChangeSelect(this);
                });
                CallChangeSelect(sel);
            }
            InitCheckbox();
        }

        function ConfirmDelete(btnid, type) {
            //console.log(type, parent.SweetSoftScript.ResourceText.DeleteNotificaitonMessage.replace('{0}', type));
            parent.SweetSoftScript.mainFunction.OpenModalWindow(
                parent.SweetSoftScript.ResourceText.DeleteNotificaitonTitle,
              parent.SweetSoftScript.ResourceText.DeleteNotificaitonMessage.replace('{0}', type),
              'confirmDelete', function () {
                  parent.SweetSoftScript.mainFunction.CloseConfirmWindow();
                  setTimeout(function () {
                      var btn = $('#' + btnid);
                      if (btn.length > 0)
                          btn.click();
                  }, 500);
              });
        }

        addRequestHanlde(InitCheckAll);
        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                //checkboxother.trigger('click');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }

        function InitCheckbox() {
            $('input[type="checkbox"][id$="chkselnone"]').change(function () {
                if ($(this).is(':checked')) {
                    var inpColl = $('div[id$="upMainSelect"] .chkdept .item input[type="checkbox"]:visible,input[type="checkbox"][id$="chkselall"]:visible');
                    if (inpColl.length > 0)
                        inpColl.prop('checked', false).trigger('change');
                }
            });
            $('input[type="checkbox"][id$="chkselall"]').change(function () {
                if ($(this).is(':checked')) {
                    var inpColl = $('div[id$="upMainSelect"] .chkdept .item input[type="checkbox"]:visible');
                    if (inpColl.length > 0)
                        inpColl.prop('checked', true).trigger('change');
                    $('input[type="checkbox"][id$="chkselnone"]:visible').prop('checked', false).trigger('change');
                }
            });
            var inpColl = $('div[id$="upMainSelect"] .chkdept input[type="checkbox"]');
            if (inpColl.length > 0) {
                inpColl.each(function () {
                    var id = $(this).attr('id');
                    if (id && id.length > 0 && (id.indexOf('chkselnone') < 0 && id.indexOf('chkselall') < 0)) {
                        $(this).change(function () {
                            var count = $('div[id$="upMainSelect"] .chkdept .item input[type="checkbox"]:visible').length;
                            var checkedall = $('div[id$="upMainSelect"] .chkdept .item input[type="checkbox"]:visible:checked');

                            if (checkedall.length === count) {
                                if ($(this).is(':checked')) {
                                    var chknone = $('input[type="checkbox"][id$="chkselnone"]:visible');
                                    if (chknone.is(':checked'))
                                        $('input[type="checkbox"][id$="chkselnone"]:visible').prop('checked', false).trigger('change');
                                }
                                else {
                                    var chkall = $('input[type="checkbox"][id$="chkselall"]:visible');
                                    if (chkall.is(':checked')) { }
                                    else
                                        chkall.prop('checked', true).trigger('change');
                                }
                            }
                            else {
                                //console.log(checkedall.length, count);
                                if (checkedall.length == 0) {
                                    var chknone = $('input[type="checkbox"][id$="chkselnone"]:visible');
                                    if (chknone.is(':checked')) { }
                                    else
                                        $('input[type="checkbox"][id$="chkselnone"]:visible').prop('checked', true).trigger('change');
                                }
                                else {
                                    $('input[type="checkbox"][id$="chkselnone"]:visible,input[type="checkbox"][id$="chkselall"]:visible').prop('checked', false).trigger('change');
                                }
                            }
                        });
                    }

                    var arr = $(this).add($(this).next());
                    arr.wrapAll('<span class="uniform col-md-6 col-sm-6 col-xs-12"></span>');
                });

                $('div[id$="upMainSelect"] .chkdept .uniform input[type=checkbox]').stop().uniform();
            }
        }

        $(function () {
            InitJS();
            //InitCheckAll();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitJS);
        });
    </script>
</asp:Content>
