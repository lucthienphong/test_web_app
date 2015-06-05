<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
     AutoEventWireup="True" CodeBehind="Notification.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Notification" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <a href="/pages/NotificationDetailAll.aspx" class="btn btn-transparent new"><span class="flaticon-new10"></span> <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NEW)%></a>
                            <a href="javascript:CheckDelete();" class="btn btn-transparent new"><span class="flaticon-delete41"></span> Delete</a>
                            <asp:Button ID="btnDelete" runat="server"
                                 style="display:none" CssClass="hide"
                                OnClick="btnDelete_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <br />
                <SweetSoft:GridviewExtension ID="grvPageList" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true"
                    OnRowCommand="grvPageList_RowCommand"
                    OnPageIndexChanging="grvPageList_PageIndexChanging"
                    OnSorting="grvPageList_Sorting" >
                    <Columns>
                        <asp:TemplateField HeaderText="Page id" ItemStyle-CssClass="column-one" SortExpression="0"
                             HeaderStyle-CssClass="sorting text-center">
                            <ItemTemplate>
                                <a href='<%#Eval("id").ToString().StartsWith(SweetSoft.APEM.Core.Manager.RealtimeNotificationManager.keyName) ? "NotificationDetailAll.aspx" : "NotificationDetail.aspx"%>?id=<%#Eval("id")%>'>
                                    <asp:Literal ID="ltrPageId" runat="server" Text='<%#Eval("id")%>'></asp:Literal></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Title" ItemStyle-CssClass="column-large" SortExpression="1"
                             HeaderStyle-CssClass="sorting text-center">
                            <ItemTemplate>
                                <%#Eval("title")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Created by" SortExpression="2" HeaderStyle-CssClass="sorting text-center"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <%#Eval("createdby")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Created on" SortExpression="3" HeaderStyle-CssClass="sorting text-center"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <%#Eval("createdon")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField
                            HeaderText="Obsolete" SortExpression="4" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsObsolete" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="chkIsDelete" class="uniform" runat="server"/>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" Runat="Server">
    <script type="text/javascript">
        $(function () {
            addRequestHanlde(InitCheckAll);
            InitCheckAll();
        });

        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
            parent.SweetSoftMessageManager.mainFunction.ForceGetMessage();
        }

        function CheckDelete() {
            var selected = $('input[type="checkbox"][id$="chkIsDelete"]:checked');
            if (selected.length > 0) {
                parent.SweetSoftScript.mainFunction.OpenSimpleModalWindow('',
                parent.SweetSoftScript.ResourceText.DeleteNotificaitonForPageMessage,
                  'confirmDelete', function () {
                      var btn = $('input[type="submit"][id$="btnDelete"].hide');
                      if (btn.length > 0)
                          btn.click();
                  });
            }
        }
    </script>
</asp:Content>
