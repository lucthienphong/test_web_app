<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
     AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.UserProfile" %>

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
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CANCEL)%></asp:LinkButton>
                        </div>
                    </div>
                </div>
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
                    <asp:Label runat="server" CssClass="error help-block" ID="lblErrorEmail"></asp:Label>
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
        </div>
        <div class="col-md-6 col-xs-6">
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
                <label>Current password</label>
                <div>
                    <asp:TextBox ID="txtCurrentPassword" runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:Label runat="server" ID="lblCrrPass" CssClass="error help-block" EnableViewState="false"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <label>
                    New password</label>
                <div>
                    <asp:TextBox ID="txtNewPassword" runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:Label runat="server" ID="lblErrorNewPass" CssClass="error help-block" EnableViewState="false"></asp:Label>
                </div>
            </div>
             <div class="form-group">
                <label>
                    Re-password</label>
                <div>
                    <asp:TextBox ID="txtRePassword" runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:Label runat="server" ID="lblRePass" CssClass="error help-block" EnableViewState="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
</asp:Content>
