﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="refProgressReproStatus.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.refProgressReproStatus" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-horizontal">
                        <asp:LinkButton ID="btnAdd" runat="server"
                            class="btn btn-transparent new"
                            OnClick="btnAdd_Click">
                                <span class="flaticon-new10"></span>
                                New</asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server"
                            class="btn btn-transparent new"
                            OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" Visible="false" Enabled="false" runat="server"
                            class="btn btn-transparent new pull-right" OnClick="btnCancel_Click">
                                <span class="flaticon-back57"></span>
                                Cancel</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="id"
                Text="" Width="100%"
                CssClass="form-control hidden" runat="server">
            </SweetSoft:CustomExtraTextbox>
            <div class="dataTables_wrapper form-inline sweet-input-mask">
                <SweetSoft:GridviewExtension ID="gvBacking" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="ReproStatusID"
                    OnPageIndexChanging="gvBacking_PageIndexChanging"
                    OnSorting="gvBacking_Sorting"
                    OnRowDeleting="gvBacking_RowDeleting"
                    OnRowEditing="gvBacking_RowEditing"
                    OnRowUpdating="gvBacking_RowUpdating"
                    OnRowCreated="gvBacking_RowCreated"
                    OnRowCancelingEdit="gvBacking_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Backing Name" SortExpression="0" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    Text='<%#Eval("ReproStatusName")%>'
                                    CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtReproStatusName"
                                        Text='<%#Eval("ReproStatusName")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Description")%>'
                                    CommandName="Edit"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDescription"
                                        Text='<%#Eval("Description")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="GoBackToRepro" SortExpression="2" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="text-center column-large">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkGoBackToReproView" CssClass="uniform"
                                    Checked='<%#Eval("GoBackToRepro")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkGoBackToRepro" CssClass="uniform"
                                    Checked='<%#Eval("GoBackToRepro")%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="3" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsObsolete" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton"
                                    runat="server"
                                    CssClass="btn btn-primary"
                                    CommandName="Update"
                                    Text="Update" Font-Underline="false">
                                                        <span class="fa fa-check"></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel"
                                    runat="server"
                                    CssClass="btn btn-danger"
                                    CommandName="Cancel"
                                    Text="Cancel">
                                                        <span class="fa fa-ban"></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
        InitCheckAll();

    </script>
</asp:Content>
