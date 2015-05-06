<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="RolePermission.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.RolePermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <asp:Label ID="lbRoleName" style="color:#fff; display: block;
                                position: absolute; height: 24px; top: 50%; margin-top: 12px;" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline pull-right">
                        <div class="form-group">
                            <asp:LinkButton ID="btnSave" runat="server"
                                class="btn btn-transparent new"
                                OnClick="btnSave_Click">
                                <span class="flaticon-new10"></span>
                                Save</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="widget-content table-container">
            <div class="dataTables_wrapper form-inline">
                <asp:GridView ID="grvRolePermisstion" runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="FunctionID, ParentID" GridLines="None"
                    OnDataBound="grvRolePermisstion_DataBound"
                    CssClass="table table-striped table-bordered table-checkable dataTable">
                    <Columns>
                        <asp:TemplateField HeaderText="Title"
                            HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server"
                                    Text='<%# Eval("Title")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CheckAll"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCheckAll" runat="server"
                                    Checked='<%# Eval("CheckAll")%>' CssClass="uniform"
                                    onclick="CheckBoxCheck(this);"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowAdd"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllowAdd" runat="server"
                                    Checked='<%# Eval("AllowAdd")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowEdit"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllowEdit" runat="server"
                                    Checked='<%# Eval("AllowEdit")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowDelete"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllowDelete" runat="server"
                                    Checked='<%# Eval("AllowDelete")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowViewPrice"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="AllowUpdateStatus" runat="server"
                                    Checked='<%# Eval("AllowUpdateStatus")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowOther"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllowOther" runat="server"
                                    Checked='<%# Eval("AllowOther")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AllowLockUnlock"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAllowLockUnlock" runat="server"
                                    Checked='<%# String.IsNullOrEmpty(Eval("AllowLockUnlock").ToString()) ? false : Eval("AllowLockUnlock")%>'
                                    CssClass="uniform"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            ChangeParent();
        });

        function ChangeParent() {
            $("div[class='dataTables_wrapper form-inline']").find("td[colspan='8']").attr("Style", "font-weight: bold;color: black");
        }

        function CheckBoxCheck(rb) {
            var gv = document.getElementById('#<%=grvRolePermisstion.ClientID%>');
            var row = rb.parentNode.parentNode.parentNode.parentNode.parentNode;
            var rbs = $(row).find("input");
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "checkbox") {
                    if (rbs[i] != rb) {
                        rbs[i].checked = rb.checked;
                        $.uniform.update(rbs[i]);
                    }
                }
            }
        }
    </script>
</asp:Content>
