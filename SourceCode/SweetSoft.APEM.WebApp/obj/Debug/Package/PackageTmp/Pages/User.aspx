<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group" style="margin-bottom:0">
                            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" class="btn btn-transparent">
                                <span class="flaticon-floppy1"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%></asp:LinkButton>
                        
                            <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-transparent">
                                <span class="flaticon-back57"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <%--<div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group pull-right">
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
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-6 col-xs-6">
            <div class="form-group">
                <label>
                    Staff No</label>
                <div>
                    <asp:TextBox ID="txtStaffNo" runat="server"
                        CssClass="form-control"
                        ReadOnly="true">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label>
                            First Name</label>
                        <div>
                            <asp:TextBox ID="txtFirstName" runat="server"
                                CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label>
                            Last Name</label>
                        <div>
                            <asp:TextBox ID="txtLastName" runat="server"
                                CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Tel number</label>
                <div>
                    <asp:TextBox ID="txtTelNumber" runat="server"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Email</label>
                <div>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Address</label>
                <div>
                    <asp:TextBox ID="txtAddress" runat="server"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Department</label>
                <div>
                    <asp:DropDownList ID="ddlDepartment" runat="server"
                        data-style="btn btn-info btn-block"
                        data-width="100%"
                        data-toggle="dropdown"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xs-6">
            <div class="form-group">
                <div>
                    <label>
                        <asp:CheckBox CssClass="uniform" ID="chkObsolete"
                            runat="server" />
                        <span style="font-weight: normal;">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.IS_OBSOLETE)%>
                        </span>
                    </label>
                </div>
            </div>
            <div class="form-group">
                <div>
                    <label>
                        <asp:CheckBox CssClass="uniform" ID="chkHasAccount"
                            AutoPostBack="true"
                            OnCheckedChanged="chkHasAccount_CheckedChanged"
                            runat="server" />
                        <span style="font-weight: normal;">Has an account?
                        </span>
                    </label>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Username</label>
                <div>
                    <asp:TextBox ID="txtUsername" runat="server"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Display name</label>
                <div>
                    <asp:TextBox ID="txtDisplayName" runat="server"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Password</label>
                <div>
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Retype password</label>
                <div>
                    <asp:TextBox ID="txtRepass" runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Select role
                </label>
                <div>
                    <asp:CheckBoxList ID="chkList" runat="server">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
</asp:Content>
