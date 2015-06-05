<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="refSupplier.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.refSupplier" %>

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
        <div class="col-md-12 sweet-input-mask">
            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="id"
                Text="" Width="100%"
                CssClass="form-control hidden" runat="server">
            </SweetSoft:CustomExtraTextbox>
            <div class="table-responsive">
                <SweetSoft:GridviewExtension ID="gvSupplier" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="SupplierID"
                    OnPageIndexChanging="gvSupplier_PageIndexChanging"
                    OnSorting="gvSupplier_Sorting"
                    OnRowDeleting="gvSupplier_RowDeleting"
                    OnRowEditing="gvSupplier_RowEditing"
                    OnRowUpdating="gvSupplier_RowUpdating"
                    OnRowCreated="gvSupplier_RowCreated"
                    OnRowCancelingEdit="gvSupplier_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Supplier Name" SortExpression="0" HeaderStyle-CssClass="sorting column-180" ItemStyle-CssClass=" column-180">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    Text='<%#Eval("Name")%>'
                                    CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtSupplierName"
                                        Text='<%#Eval("Name")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Person" SortExpression="1" HeaderStyle-CssClass="sorting column-150" ItemStyle-CssClass=" column-150">
                            <ItemTemplate>
                                <asp:Label ID="lbContactPerson" runat="server" Text='<%#Eval("ContactPerson")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtContactPerson"
                                        Text='<%#Eval("ContactPerson")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" SortExpression="2"  HeaderStyle-CssClass="sorting column-250" ItemStyle-CssClass=" column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Address")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <asp:TextBox ID="txtSupplierAddress" TextMode="MultiLine" Rows="3"
                                        Text='<%#Eval("Address")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </asp:TextBox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tel" SortExpression="3"  HeaderStyle-CssClass="sorting column-60" ItemStyle-CssClass=" column-60">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Tel")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:ExtraInputMask RenderOnlyInput="true" ID="txtTel"
                                        Text='<%#Eval("Tel")%>' Width="100%" Required="true"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:ExtraInputMask>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fax" SortExpression="4"  HeaderStyle-CssClass="sorting column-60" ItemStyle-CssClass=" column-60">
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%#Eval("Fax")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:ExtraInputMask RenderOnlyInput="true" ID="txtFax"
                                        Text='<%#Eval("Fax")%>' Width="100%"
                                        CssClass="form-control" Required="true" runat="server">
                                    </SweetSoft:ExtraInputMask>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="2" HeaderStyle-CssClass="sorting"
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
