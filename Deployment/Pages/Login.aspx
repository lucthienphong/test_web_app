<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Login" %>

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
                        <div class="panel-heading"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.LOGIN)%></div>
                        <div class="panel-body">
                            <div class="login-form">
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-8">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3">
                                                            <span class="pull-left">
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.USERNAME)%></span>
                                                        </label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3">
                                                            <span class="pull-left">
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PASSWORD)%></span>
                                                        </label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-9 col-sm-offset-3">

                                                            <label style="font-weight: normal">
                                                                <asp:CheckBox ID="checkRemember" CssClass="uniform" runat="server" />
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMEMBER)%>
                                                            </label>

                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="form-group">
                                                        <div class="col-sm-9 col-sm-push-3">
                                                            <button class="btn btn-default pull-right" id="btnLogin" runat="server" onserverclick="btnLogin_ServerClick">
                                                                <span class="fa fa-unlock fa-1x"></span>
                                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.LOGIN)%></button>
                                                            <p class="form-control-static">
                                                                <a href="/ForgotPassword.aspx" style="color: white; text-decoration: none;">Forgot your password ?</a>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <asp:Label ID="lbMessage" Style="color: #fff;" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <img src="/img/keys-icon.png" class="img-responsive" style="margin-top: -50px" />
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
