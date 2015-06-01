<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="SystemCofiguration.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.SystemCofiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group" style="margin-bottom: 0">
                            <button id="btnSave" runat="server" onserverclick="btnSave_ServerClick" class="btn btn-transparent">
                                <span class="flaticon-floppy1"></span>
                                Save</button>

                            <button id="btnCancel" runat="server" onserverclick="btnCancel_ServerClick" class="btn btn-transparent">
                                <span class="flaticon-back57"></span>
                                Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12 tabbable">
            <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#Info" role="tab" data-toggle="tab">
                    <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CONTACT_INFO)%></strong>
                </a></li>
                <li><a href="#SMTP" role="tab" data-toggle="tab">
                    <strong>SMTP</strong></a></li>
                <li><a href="#Setting" role="tab" data-toggle="tab">
                    <strong>Settings</strong></a></li>
            </ul>
        </div>
    </div>
    <div class="tab-content">
        <div class="tab-pane active" id="Info">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-xs-6">
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.COMPANY_NAME)%></label>
                            <div>
                                <asp:TextBox ID="txtCompanyName" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Company Code</label>
                            <div>
                                <asp:TextBox ID="txtCompanyCode" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.COMPANY_ADDRESS)%></label>
                            <div>
                                <asp:TextBox ID="txtCompanyAddress" runat="server" TextMode="MultiLine" Rows="4"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.COMPANY_PHONE)%></label>
                                        <div>
                                            <asp:TextBox ID="txtPhoneNumber" runat="server"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.COMPANY_FAX)%></label>
                                    <div>
                                        <asp:TextBox ID="txtFax" runat="server"
                                            CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.COMPANY_WEBSITE)%></label>
                            <div>
                                <asp:TextBox ID="txtWebsite" runat="server"
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
                    </div>
                    <div class="col-md-6 col-xs-6">
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BANK_ACCOUNT_NUMBER)%></label>
                            <div>
                                <asp:TextBox ID="txtAccountNumber" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BANK_NAME)%></label>
                            <div>
                                <asp:TextBox ID="txtBankName" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BANK_ADDRESS)%></label>
                            <div>
                                <asp:TextBox ID="txtBankAddress" runat="server" TextMode="MultiLine" Rows="4"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BANK_SWIFT_CODE)%></label>
                            <div>
                                <asp:TextBox ID="txtSwiftCode" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                ISDN
                            <div>
                                <asp:TextBox ID="txtISDN" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BUG_REPORT_EMAIL)%></label>
                            <div>
                                <asp:TextBox ID="txtBugReportEmail" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="SMTP">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-xs-6 col-md-offset-3 col-xs-offset-3">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_SERVER)%></label>
                                <div>
                                    <asp:TextBox ID="txtSmtpMailServerAddress" runat="server"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_PORT)%></label>
                                <div>
                                    <asp:TextBox ID="txtSmtpPort" runat="server"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div>
                                    <label>
                                        <asp:CheckBox CssClass="uniform" ID="chkSmtpUsingSSL"
                                            runat="server" />
                                        <span style="font-weight: normal;">
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_SSL_REQUIRED)%>
                                        </span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_ACCOUNT)%></label>
                                <div>
                                    <asp:TextBox ID="txtSmtpSenderAccount" runat="server"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_PASSWORD)%></label>
                                <div>
                                    <asp:TextBox ID="txtSmtpSenderPassword" runat="server"
                                        TextMode="Password"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SMTP_TEST_EMAIL)%></label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtTestingEmail" runat="server"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                    <span class="input-group-addon">
                                        <asp:LinkButton ID="btnTestEmail"
                                            Text="Send test email"
                                            runat="server" OnClick="btnTestEmail_Click">
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Setting">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-xs-6">
                        <div class="form-group">
                            <label>
                                Pi value</label>
                            <div>
                                <asp:TextBox ID="txtPiValue" runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Sale department</label>
                            <div>
                                <asp:DropDownList ID="ddlSaleRep" runat="server"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Job coordinator department</label>
                            <div>
                                <asp:DropDownList ID="ddlJobCoordinator" runat="server"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Base country</label>
                            <div>
                                <asp:DropDownList ID="ddlBaseCountry" runat="server"
                                    data-live-search="true"
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
                            <label>
                                Base currency</label>
                            <div>
                                <asp:DropDownList ID="ddlBaseCurrency" runat="server"
                                    data-live-search="true"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Pricing master template</label>
                            <div>
                                <asp:DropDownList ID="ddlCustomer" runat="server"
                                    data-live-search="true"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Default Overseas Tax</label>
                            <div>
                                <asp:DropDownList ID="ddlTax" runat="server"
                                    data-live-search="true"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
