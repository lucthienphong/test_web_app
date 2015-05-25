<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="MachineList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.MachineList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
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
    <div class="row row_content">
        <div class="col-xs-12">
            <div class="table-responsive">
                <asp:GridView runat="server"
                     ID="gvMachine" AutoGenerateColumns="false" 
                    AllowSorting="true"
                    OnSorting="gvMachine_Sorting"
                    CssClass="table table-striped table-bordered table-checkable dataTable">
                    <Columns>
                        <asp:TemplateField HeaderText="Code" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine name" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <a href='MachineDetail.aspx?ID=<%# Eval("ID")  %>'>
                                    <%#Eval("Name") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Performance">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Performance") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department" SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Obsolete">
                            <ItemTemplate>
                                <asp:CheckBox CssClass="uniform" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsObsolete"))%>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column text-center">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>
        $(function () {
            InitCheckAll();
        })
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
            return false;
        }
    </script>
</asp:Content>
