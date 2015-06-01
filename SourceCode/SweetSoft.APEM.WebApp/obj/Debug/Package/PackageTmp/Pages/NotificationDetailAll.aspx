<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="True" CodeBehind="NotificationDetailAll.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.NotificationDetailAll" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/Controls/NotificationSettingAll.ascx" TagPrefix="SweetSoft" TagName="NotificationSettingAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:LinkButton class="btn btn-transparent" ID="btnNSave"
                                runat="server" OnClick="btnNSave_Click">
                                <span class="flaticon-floppy1"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%></asp:LinkButton>
                            <a class="btn btn-transparent new" href="javascript:void(0);" onclick="ConfirmDelete('btnNDelete'); return false;">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%></a>
                            <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-transparent">
                                <span class="flaticon-back57"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CANCEL)%></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <%--<div class="row row_content">
            <div class="form-group">
                <div class="col-md-12">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NOTIFICATION)%>
                    </label>
                    <div class="form-group">
                        <div class="col-sm-3 col-xs-4">
                            <SweetSoft:CustomExtraTextbox ID="txtId" ReadOnly="true" RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                        <div class="col-sm-9 col-xs-8">
                            <SweetSoft:CustomExtraTextbox ID="txtTitle" RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="row row_content">
            <SweetSoft:NotificationSettingAll runat="server" ID="NotificationSetting" />
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
                                parent.SweetSoftMessageManager.mainFunction.ForceGetMessage();
                                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                parent.SweetSoftScript.ResourceText.DeleteNotificaitonSettingDone, 'alert', function () {
                                    location.href = '/pages/notification.aspx';
                                });
                            }
                            else {
                                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                   parent.SweetSoftScript.ResourceText.ErrorDeleteNotificaitonSetting, 'alert');
                            }
                            $('#hdfdone').remove();
                        }
                        else {
                            parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                parent.SweetSoftScript.ResourceText.NotExistNotificaitonSetting, 'alert');
                        }
                    }
                    else if (elem.id.indexOf('btnNSave') >= 0) {
                        if ($('#hdfdone').length > 0) {
                            if ($('#hdfdone').val() === '1') {
                                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                       parent.SweetSoftScript.ResourceText.SaveNotificaitonSetting, 'alert');
                                parent.SweetSoftMessageManager.mainFunction.ForceGetMessage();
                            }
                            $('#hdfdone').remove();
                        }
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
            InitCheckAll();
        }

        function ConfirmDelete(btnid) {
            var type = $('input[type="text"][id$="txtID"]').val();
            if (type && type.length > 0) {
                //console.log(type, parent.SweetSoftScript.ResourceText.DeleteNotificaitonMessage.replace('{0}', type));
                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(
                    parent.SweetSoftScript.ResourceText.DeleteNotificaitonTitle,
                  parent.SweetSoftScript.ResourceText.DeleteNotificaitonMessage.replace('{0}', type),
                  'confirmDelete', function () {
                      parent.SweetSoftScript.mainFunction.CloseConfirmWindow();
                      setTimeout(function () {
                          var btn = $('input[type="submit"][id$="' + btnid + '"]');
                          if (btn.length > 0)
                              btn.click();
                      }, 500);
                  });
            }
        }

        function InitCheckAll() {
            $("input[type='checkbox'][id$='chkStaffAll']").unbind('change').change(function () {
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
                        inpColl.prop('checked', false);
                    var btn = $(this).closest('.chkdept').find('input[type="submit"][id$="btnSelect"]');
                    if (btn.length > 0)
                        btn.click();
                }
            });

            $('input[type="checkbox"][id$="chkselall"]').change(function () {
                if ($(this).is(':checked')) {
                    var inpColl = $('div[id$="upMainSelect"] .chkdept .item input[type="checkbox"]:visible');
                    if (inpColl.length > 0)
                        inpColl.prop('checked', true);
                    $('input[type="checkbox"][id$="chkselnone"]:visible').prop('checked', false);
                    var btn = $(this).closest('.chkdept').find('input[type="submit"][id$="btnSelect"]');
                    if (btn.length > 0)
                        btn.click();
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

                            var btn = $(this).closest('.chkdept').find('input[type="submit"][id$="btnSelect"]');
                            if (btn.length > 0)
                                btn.click();
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
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitJS);
        });
    </script>
</asp:Content>