<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="NotificationSettingAll.ascx.cs" 
    Inherits="SweetSoft.APEM.WebApp.Controls.NotificationSettingAll" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/Controls/SelectReceivers.ascx" TagPrefix="SweetSoft" TagName="SelectReceivers" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-6 col-sm-6">
                    <div class="form">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">
                                    ID
                                </label>
                                <input type="text" class="form-control " disabled="disabled" id="txtID" runat="server" />
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Title
                                </label>
                                <p class="help-block">This is title of modal message box.</p>
                                <input type="text" class="form-control " id="txtNTitle" runat="server" />
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Content
                                </label>
                                <p class="help-block">This is content of modal message box.</p>                                
                                <CKEditor:CKEditorControl EnableViewState="false" ID="txtNContentFormat" runat="server" Height="200">
                                </CKEditor:CKEditorControl>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6" id="mainright">
                    <div class="form">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Literal ID="ltrDelete" runat="server"></asp:Literal>
                                <asp:Button ID="btnNDelete" Style="display: none"
                                     runat="server" Text="" OnClick="btnNDelete_Click" />
                            </div>
                            <div class="form-group chk" style="margin-bottom:0">
                                <label>
                                    <asp:CheckBox ID="chkIsObsolete" runat="server" CssClass="uniform" />
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.IS_OBSOLETE)%>
                                </label>
                            </div>
                            <SweetSoft:SelectReceivers runat="server" ID="SelectReceivers1" />
                            <div class="form-group hide" style="margin-bottom:0;">
                                <input type="checkbox" class="uniform" id="chkOpenPage" runat="server" 
                                    value="SweetSoftMessageManager.mainFunction.OpenPage" />

                                <label class="control-label" for="<%=chkOpenPage.ClientID %>">
                                    Open modal page
                                </label>
                                <p class="help-block" style="padding-left: 15px">Addition for open specific page.</p>
                            </div>
                            <div class="col-md-12 hide">
                                <div class="form-group">
                                    <label class="control-label">
                                        Button text
                                    </label>
                                    <p class="help-block">This is the text of button for trigger open page</p>
                                    <input type="text" class="form-control " value="Open" id="txtNButtonText" runat="server"/>
                                </div>
                                <div class="form-group">
                                    <label class="control-label">
                                        Title text
                                    </label>
                                    <p class="help-block">This is the title of window frame</p>
                                    <input type="text" class="form-control " value="" id="txtNTitleWindow"
                                         runat="server"/>
                                </div>
                                <div class="form-group hide">
                                    <label class="control-label">
                                        Dismiss action
                                    </label>
                                    <p class="help-block">This notification not show when....</p>
                                    <select id="ddlDismissEvent" runat="server" data-style="btn btn-info btn-block"
                                        data-width="100%" data-live-search="false"
                                        data-toggle="dropdown" class="form-control">
                                        <option value="">View</option>
                                        <option value="insert">Insert</option>
                                        <option value="update">Update</option>
                                        <option value="delete">Delete</option>
                                    </select>
                                </div>
                                <div class="form-group hide">
                                    <label class="control-label">
                                        Select or enter page url for open
                                    </label>
                                    <select id="ddlAvailablePage" runat="server" data-style="btn btn-info btn-block"
                                        data-width="100%" data-live-search="true"
                                        data-toggle="dropdown" class="form-control">
                                    </select>
                                </div>
                                <div class="form-group" id="divother" style="display: none">
                                    <label class="control-label">
                                        Other url
                                    </label>
                                    <input type="text" class="form-control form-control " runat="server" id="txtOtherUrl">
                                </div>
                                <div class="form-group hide">
                                    <input type="checkbox" class="uniform" id="chkOpenMesage" runat="server" value="SweetSoftScript.mainFunction.OpenSimpleModalWindow" />

                                    <label class="control-label" for="<%=chkOpenMesage.ClientID %>">
                                        Show new message after open
                                    </label>
                                </div>
                                <div class="form-group" style="display: none">
                                    <label class="control-label">
                                        Content:
                                    </label>
                                    <CKEditor:CKEditorControl EnableViewState="false" ID="txtOtherMessage" runat="server" Height="200">
                                    </CKEditor:CKEditorControl>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>