<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="Department.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Department" %>

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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvDepartmentList" runat="server" AutoGenerateColumns="false" ToolTip="Department List"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="DepartmentID"
                    OnRowCommand="grvDepartmentList_RowCommand"
                    OnPageIndexChanging="grvDepartmentList_PageIndexChanging"
                    OnSorting="grvDepartmentList_Sorting"
                    OnRowDeleting="grvDepartmentList_RowDeleting"
                    OnRowEditing="grvDepartmentList_RowEditing"
                    OnRowUpdating="grvDepartmentList_RowUpdating"
                    OnRowCancelingEdit="grvDepartmentList_RowCancelingEdit"
                    OnRowDataBound="grvDepartmentList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Departmnet" SortExpression="0" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    Text='<%#Eval("DepartmentName")%>'
                                    CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="position: relative;">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDepartmentName"
                                        Text='<%#Eval("DepartmentName")%>' Width="100%" ToolTip="Department Name"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Process Type" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbProcessType" CssClass="control-label"
                                    Text='<%#Eval("ProcessType")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="position: relative;">
                                    <asp:DropDownList ID="ddlProcessType" runat="server" ToolTip="Process Type"
                                        data-style="btn btn-info" data-width="100%" Required="true"
                                        data-toggle="dropdown" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Type" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbProductType" CssClass="control-label"
                                    Text='<%#Eval("ProductType")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div runat="server" id="divProductType">
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="UsedInWorkflow" SortExpression="3" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkUsedInWorkflowView" CssClass="uniform" ToolTip="Show in Work Flow"
                                    Checked='<%#Convert.ToBoolean(Eval("ShowInWorkFlow"))%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkUsedInWorkflow" CssClass="uniform" ToolTip="Show in Work Flow"
                                    Checked='<%#Convert.ToBoolean(Eval("ShowInWorkFlow"))%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="TimelineOrder" SortExpression="4" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:Label ID="lbTimelineOrder" CssClass="control-label"
                                    Text='<%#Eval("TimelineOrder")%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="col-md-12">
                                    <SweetSoft:ExtraInputMask ID="txtTimelineOrder" runat="server"
                                        RenderOnlyInput="true" Required="true" ToolTip="Timeline Order"
                                        RequiredText="Require field"
                                        MaskType="Numeric" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="0"
                                        ShowMaskOnHover="false" Greedy="false"
                                        Text='<%#Eval("TimelineOrder")%>' Width="100%">
                                    </SweetSoft:ExtraInputMask>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="5" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform" ToolTip="Is Obsolete"
                                    Checked='<%#Convert.ToBoolean(Eval("IsObsolete"))%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsObsolete" CssClass="uniform" ToolTip="Is Obsolete"
                                    Checked='<%#Convert.ToBoolean(Eval("IsObsolete"))%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform" ToolTip="Is Delete"
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
