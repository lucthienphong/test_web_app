<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="padding-top:20px;">
        <div class="col-md-6 col-lg-6 col-md-offset-3 col-lg-offset-3">
            <div class="widget">
                <div class="widget-content form-container">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-5 col-xs-5">
                                <img src="/img/lock.png" alt="access denied" class="img-responsive" />
                            </div>
                            <div class="col-md-7 col-xs-7">
                                <label style="color: #428bca; font-size: 20px; padding-top: 60px;">
                                    ERROR: ACCESS DENIED!
                                </label>
                                <label>
                                    You have no permission to access this page. Please contact your system administrator to obtain additional permissions.
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
</asp:Content>
