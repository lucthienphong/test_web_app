<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMasterPage.Master"
    AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="middle">
            <div class="row">
                <div class="col-sm-12">
                    <img src="/img/fulllogo.png" class="center-block img-responsive" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-6 col-sm-offset-3">
                    <div class="panel panel-login">
                        <div class="panel-heading"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FORGOT_PASSWORD)%></div>
                        <div class="panel-body">
                            <div class="login-form">
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-4">
                                                            <span class="pull-left">
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.USERNAME)%></span>
                                                        </label>
                                                        <div class="col-sm-8">
                                                             <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server"></asp:TextBox>
                                                             <asp:RequiredFieldValidator ID="requireValidName" runat="server" ErrorMessage="Please enter username" ControlToValidate="txtUserName" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-4">
                                                            <span class="pull-left">
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.EMAIL)%></span>
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="requireValidEmail" runat="server" ErrorMessage="Please enter email address" ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator runat="server" ID="validEmailRegExp" ControlToValidate="txtEmail"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Email address is invalid" Display="Dynamic" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-4">
                                                            <span class="pull-left">Security Code</span>
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox Style="float: left" ID="txtValidCode" Width="100px" runat="server" CssClass="form-control"></asp:TextBox>


                                                            <asp:Image ID="imCaptcha" Style="margin-left: 10px; float: left" ImageUrl="Captcha.ashx" runat="server" Width="60px" Height="32px" />
                                                            <%--<asp:ImageButton runat="server" Width="16px" Height="16px" ID="refreshValidCode" ImageUrl="/images/refresh.png" ToolTip="Change picture of security code" OnClick="ChangeCaptchaImage" CausesValidation="false" />--%>
                                                            <asp:LinkButton runat="server" ID="btnRefreshValidCode" ToolTip="Đổi mã bảo vệ"
                                                                OnClick="ChangeCaptchaImage" CausesValidation="false">
                                                                <span class="glyphicon glyphicon-refresh" style="color: #fff;font-size: 25px;float: left;margin-top: 2px;margin-left: 10px;"></span>
                                                            </asp:LinkButton>
                                                        </div>


                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-sm-4">
                                                            
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label runat="server" ID="lbError" style="color:red" EnableViewState="false"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-default" />
                                                            <asp:Button ID="btnClearText" runat="server" Text="Clear" OnClick="btnClearText_Click" CssClass="btn btn-default" CausesValidation="false" />
                                                        </div>
                                                    </div>

                                                    <asp:Label ID="lbMessage" Style="color: #fff;" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
