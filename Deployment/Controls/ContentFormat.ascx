<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SweetSoft.APEM.WebApp.Controls.ContentFormat" CodeBehind="ContentFormat.ascx.cs" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<style type="text/css">
    .format textarea
    {
        display: none!important;
    }
</style>
<div class="form-group format">
    <label class="control-label">
        Content format
    </label>
    <p class="help-block">This is content of modal message box.</p>
    <CKEditor:CKEditorControl EnableViewState="false" ID="txtNContentFormat" runat="server" Height="200">
    </CKEditor:CKEditorControl>
    <br />
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-group">
                Test values:
            </div>
            <div class="form-group">
                <label class="control-label">{0}: Object id</label>
                <input type="text" class="form-control " placeholder="Enter object id for test" runat="server" id="txtObjectIdTest" />
            </div>
            <div class="form-group">
                <label class="control-label">{1}: Object name</label>
                <input type="text" class="form-control " placeholder="Enter object name for test" runat="server" id="txtObjectNameTest" />
            </div>
            <div class="form-group">
                <label class="control-label">{2}: Username <small>(Current logged in username)</small></label>
                <input type="text" class="form-control " readonly="readonly" placeholder="Enter username for test" runat="server" id="txtUsernameTest" />
            </div>
            <div class="form-group">
                <asp:Button runat="server" CssClass="btn btn-primary"
                    ID="btnTestNContentFormat" Text="Test content format" OnClick="btnTestEmailContentFormat_Click" />
            </div>
            <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
            <div class="form-group">
                <label class="control-label">Result</label>
                <div id="contenttest" class="well">
                    <asp:Literal ID="ltrTestResult" runat="server" EnableViewState="false" Text="[Test content]"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
</div>
