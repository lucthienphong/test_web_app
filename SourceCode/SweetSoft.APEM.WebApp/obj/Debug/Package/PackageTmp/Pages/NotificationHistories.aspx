<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificationHistories.aspx.cs"
    MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    Inherits="SweetSoft.APEM.WebApp.Pages.NotificationHistories" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:Literal ID="ltrDelete" runat="server"></asp:Literal>
                            <a href="javascript:CheckDelete();" class="btn btn-transparent new"><span class="flaticon-delete41"></span>Delete</a>
                            <asp:Button ID="btnDelete" runat="server"
                                Style="display: none" CssClass="hide"
                                OnClick="btnDelete_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="form-group">
                <div class="form-inline">
                    <asp:TextBox ID="txtKeyword" runat="server" placeholder="Enter some keyword..."
                        class="form-control ignore"></asp:TextBox>
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click">
                    <span class="glyphicon glyphicon-search"></span>&nbsp;
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SEARCH)%>
                    </asp:LinkButton>
                </div>
            </div>
            <SweetSoft:GridviewExtension ID="grvPageList" runat="server" AutoGenerateColumns="false"
                CssClass="table table-striped table-bordered table-checkable" GridLines="None"
                AllowPaging="true" AllowSorting="true"
                OnRowCommand="grvPageList_RowCommand"
                OnPageIndexChanging="grvPageList_PageIndexChanging"
                OnSorting="grvPageList_Sorting">
                <Columns>
                    <asp:TemplateField
                        HeaderText="Title" SortExpression="0" HeaderStyle-CssClass="sorting">
                        <ItemTemplate>
                            <a href='javascript:void(0);' class="datamsg"
                                data-id='<%#SweetSoft.APEM.Core.SecurityHelper.Encrypt(Eval("Notificationid").ToString())%>'>
                                <%#Eval("IsObsolete").ToString().ToLower()=="true"?Eval("Title").ToString():"<b>"+Eval("Title")+"</b>"%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField
                        HeaderText="Created by" SortExpression="1" HeaderStyle-CssClass="sorting">
                        <ItemTemplate>
                            <%#Eval("CreatedBy")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created on" SortExpression="2" HeaderStyle-CssClass="sorting">
                        <ItemTemplate>
                            <%#Eval("CreatedOn")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="text-center" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" class="uniform" />
                        </HeaderTemplate>
                        <ItemStyle CssClass="text-center" />
                        <ItemTemplate>
                            <input type="checkbox" id="chkIsDelete" class="uniform" runat="server" value='<%#Eval("Notificationid")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle BorderStyle="None" />
                <PagerSettings
                    Mode="NumericFirstLast"
                    PageButtonCount="5"
                    FirstPageText="&laquo;"
                    LastPageText="&raquo;"
                    NextPageText="&rsaquo;"
                    PreviousPageText="&lsaquo;"
                    Position="Bottom" />
                <PagerTemplate>
                    <SweetSoft:GridViewPager ID="GridViewPager1" runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </SweetSoft:GridviewExtension>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="Server">
    <script type="text/javascript">
        $(function () {
            InitMarkAsRead();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitMarkAsRead);
        });

        function InitMarkAsRead(event, args) {
            if (event) {
                var elem = event._postBackSettings.sourceElement;
                if (elem && elem.id && elem.id.length > 0) {
                    if (elem.id.indexOf('btnDelete') >= 0 || elem.id.indexOf('btnMarkAsRead') >= 0
                        || elem.id.indexOf('btnMarkAsUnRead') >= 0) {
                        if ($('#hdfdone').length > 0) {
                            if ($('#hdfdone').val() === '1') {
                                SweetSoftScript.ConfirmUnsaved.MarkAsSaved();
                                var contentMsg = '';
                                if (elem.id.indexOf('btnDelete') >= 0)
                                    contentMsg = parent.SweetSoftScript.ResourceText.DeleteNotificaitonSettingDone;
                                else {
                                    contentMsg = parent.SweetSoftScript.ResourceText.UpdateNotificaitonHistoryDone;
                                    parent.SweetSoftMessageManager.mainFunction.ForceGetMessage();
                                }
                                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                    contentMsg, 'alert');
                            }
                            else {
                                var contentMsg = '';
                                if (elem.id.indexOf('btnDelete') >= 0)
                                    contentMsg = parent.SweetSoftScript.ResourceText.ErrorDeleteNotificaitonSetting;
                                else {
                                    contentMsg = parent.SweetSoftScript.ResourceText.UpdateNotificaitonHistoryFail;
                                }
                                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow(parent.SweetSoftScript.ResourceText.notice,
                                   contentMsg, 'alert');
                            }
                            $('#hdfdone').remove();
                        }
                        else {

                        }
                    }
                }
            }


            InitSelectAll();
            var all = $('div[id$="grvPageList"] input[type="checkbox"][id$="chkIsObsoleteView"]');
            if (all.length > 0) {
                var countcheck = all.filter(':checked');
                if (countcheck.length === all.length) {
                    $('div[id$="grvPageList"] input[type="checkbox"][id$="chkMarkAll"]')
                        .prop('checked', true).trigger('change');
                }
            }

            var chk = $('div[id$="grvPageList"] input[type="checkbox"][id$="chkMarkAll"]');
            if (chk.length > 0) {
                chk.change(function () {
                    var all = $('div[id$="grvPageList"] input[type="checkbox"][id$="chkIsObsoleteView"]');
                    if (all.length > 0) {
                        var ischecked = $(this).is(':checked');
                        all.prop('checked', ischecked).trigger('change');
                    }
                });
            }

            var linkColl = $('div[id$="grvPageList"] a.datamsg');
            if (linkColl.length > 0) {
                linkColl.click(function () {
                    var t = $(this);
                    var id = t.attr('data-id');
                    if (id && id.length > 0) {
                        parent.SweetSoftMessageManager.mainFunction.LoadAndShowMessage(decodeURIComponent(id), function () {
                            t.text(t.text());
                        });
                    }
                });
            }

            var txt = $('input[type="text"][id$="txtKeyword"]');
            if (txt.length > 0) {
                txt.keydown(function (e) {
                    // "e" is the standard behavior (FF, Chrome, Safari, Opera),
                    // while "window.event" (or "event") is IE's behavior
                    var theEvent = e || window.event;
                    var key = theEvent.keyCode || theEvent.which;
                    key = String.fromCharCode(key);
                    if (theEvent.keyCode === 13) {
                        e.preventDefault();
                        e.stopPropagation();
                        var btn = txt.closest('.form-horizontal').find('a[id$="btnSearch"]');
                        if (btn.length > 0 && btn.attr('href').length > 0)
                            eval(btn.attr('href'));
                    }
                    else {

                    }
                });
            }
        }

        function InitSelectAll() {
            var chk = $('div[id$="grvPageList"] input[type="checkbox"][id$="chkSelectAll"]');
            if (chk.length > 0) {
                chk.change(function () {
                    var all = $('div[id$="grvPageList"] input[type="checkbox"][id$="chkIsDelete"]');
                    if (all.length > 0) {
                        var ischecked = $(this).is(':checked');
                        all.prop('checked', ischecked).trigger('change');
                    }
                });
            }
        }

        function CheckDelete() {
            var selected = $('input[type="checkbox"][id$="chkIsDelete"]:checked');
            if (selected.length > 0) {
                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow('',
                    selected.length === 1 ?
                    parent.SweetSoftScript.ResourceText.DeleteYourNotificationSingle :
                    parent.SweetSoftScript.ResourceText.DeleteYourNotificationPlural,
                  'confirmDelete', function () {
                      var btn = $('input[type="submit"][id$="btnDelete"].hide');
                      if (btn.length > 0)
                          btn.click();
                  });
            }
        }

        function CheckUnMark() {
            var selected = $('input[type="checkbox"][id$="chkIsObsoleteView"]:checked');
            if (selected.length > 0) {
                var btn = $('input[type="submit"][id$="btnMarkAsUnRead"].hide');
                if (btn.length > 0)
                    btn.click();
            }
        }

        function CheckMark() {
            var selected = $('input[type="checkbox"][id$="chkIsObsoleteView"]:checked');
            if (selected.length > 0) {
                var btn = $('input[type="submit"][id$="btnMarkAsRead"].hide');
                if (btn.length > 0)
                    btn.click();
            }
        }
    </script>
</asp:Content>
