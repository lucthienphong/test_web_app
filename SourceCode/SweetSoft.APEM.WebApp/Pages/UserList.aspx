<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.UserList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group" style="margin-bottom: 0">
                            <a id="btnAdd" href="User.aspx"
                                class="btn btn-transparent new">
                                <span class="flaticon-new10"></span>
                                New</a>

                            <asp:LinkButton ID="btnDelete" runat="server"
                                class="btn btn-transparent new"
                                OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                        </div>
                        <!--<div class="form-group">
                                <button class="btn btn-transparent">
                                    <span class="flaticon-floppy1"></span>
                                    Save</button>
                            </div>
                            <div class="form-group">
                                <button class="btn btn-transparent">
                                    <span class="flaticon-delete41"></span>
                                    Delete</button>
                            </div>
                            <div class="form-group">
                                <button class="btn btn-transparent">
                                    <span class="flaticon-back57"></span>
                                    Cancel</button>
                            </div>-->

                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <%-- <div class="form-group pull-right">
                            <button class="btn btn-transparent">
                                <span class="flaticon-printer60"></span>
                                Print</button>
                        </div>
                        <div class="form-group pull-right">
                            <button class="btn btn-transparent">
                                <span class="flaticon-xlsx"></span>
                                Excel</button>
                        </div>
                        <div class="form-group pull-right">
                            <button class="btn btn-transparent">
                                <span class="flaticon-pdf19"></span>
                                PDF</button>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvUserList" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="StaffID"
                    OnRowCommand="grvUserList_RowCommand"
                    OnPageIndexChanging="grvUserList_PageIndexChanging"
                    OnSorting="grvUserList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="StaffNo" SortExpression="0" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandArgument='<%#Eval("StaffID")%>' CommandName="Detail"
                                    Text='<%#Eval("StaffNo")%>' data-id='<%#Eval("StaffID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FullName" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbFullName" runat="server"
                                    Text='<%#Eval("FullName")%>'
                                    CommandName="Edit"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbEmail" runat="server"
                                    Text='<%#Eval("Email")%>'
                                    CommandName="Edit"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbDepartmentName" runat="server"
                                    Text='<%#Eval("DepartmentName")%>'
                                    CommandName="Edit"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="4" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
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
                        <SweetSoft:GridViewPager runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>
        addRequestHanlde(InitCheckAll);
        //addRequestHanlde(InitDetail);


        InitCheckAll();
        //InitDetail();

        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                console.log(checkboxother.length);
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

        //function InitDetail() {
        //    var linkColl = $('div[id$="grvUserList"] a[id$="btnEdit"]');
        //    if (linkColl.length > 0) {
        //        linkColl.click(function () {
        //            parent.openWindow($('a[data-title]:eq(0)'), 'Staff', '/Pages/User.aspx?ID=' + $(this).attr('data-id'));
        //            return false;
        //        });
        //    }
        //}

        //$('button[id="btnAdd"]').click(function () {
        //    parent.openWindow($('a[data-title]:eq(0)'), 'Staff', '/Pages/User.aspx');
        //    return false;
        //});
    </script>
</asp:Content>
