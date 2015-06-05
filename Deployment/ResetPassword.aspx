<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMasterPage.Master"
    AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.ResetPassword" %>

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
                        <div class="panel-heading">
                            Requirements change password
                        </div>
                        <div class="panel-body">
                            <div class="login-form">
                                <asp:UpdatePanel ID="UpdatePanel" runat="server">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-12">
                                                            <asp:Label runat="server" ID="lblMessage" EnableViewState="false" CssClass="pull-left"></asp:Label>
                                                        </label>
                                                    </div>
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
