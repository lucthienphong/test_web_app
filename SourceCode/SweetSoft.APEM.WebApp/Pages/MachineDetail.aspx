<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="MachineDetail.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.MachineDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="col-xs-12">
                <div class="form-horizontal">
                    <asp:LinkButton ID="btnAdd" runat="server"
                        class="btn btn-transparent new"
                        OnClick="btnAdd_Click">
                                <span class="flaticon-floppy1"></span>
                                Save</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Visible="false"
                        class="btn btn-transparent new"
                        OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server"
                        class="btn btn-transparent new" OnClick="btnCancel_Click">
                                <span class="flaticon-back57"></span>
                                Back to list</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-6 col-sm-6 sweet-input-mask">
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.MACHINE_NAME)%>
                </label>
                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" 
                    ID="txtMachineName" ToolTip="Machine Name"
                    runat="server" 
                    CssClass="form-control"></SweetSoft:CustomExtraTextbox>
            </div>
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.MACHINE_CODE)%>
                </label>
                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" 
                    ID="txtMachineCode" ToolTip="Machine Code"
                    runat="server" 
                    CssClass="form-control"></SweetSoft:CustomExtraTextbox>
            </div>
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PERFORMANCE)%>
                </label>
                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" 
                    ID="txtPerformance" ToolTip="Performance"
                    runat="server" 
                    CssClass="form-control"></SweetSoft:CustomExtraTextbox>
            </div>
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.MAINTERNANCE)%>
                </label>
                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" 
                    ID="txtMainternance" ToolTip="Mainternance"
                    runat="server" 
                    CssClass="form-control"></SweetSoft:CustomExtraTextbox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 sweet-input-mask">
            <div class="form">
                <div class="form-group">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.MANUFACTURER)%>
                    </label>
                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" 
                        ID="txtManufacturer" ToolTip="Manufacturer"
                        runat="server" 
                        CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                </div>
                <div class="form-group">
                    <label>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PRODUCEYEAR)%>
                    </label>
                    <SweetSoft:ExtraInputMask ID="txtProductYear" RenderOnlyInput="true" Required="false" CssClass="text-left" ToolTip="Product Year"
                        runat="server" MaskType="Integer" Text="2015" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>

                </div>
                <div class="form-group">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DEPARTMENT)%>
                    </label>
                    <asp:DropDownList ID="ddlDepartment" runat="server" ToolTip="Department"
                        data-style="btn btn-info btn-block"
                        data-width="100%" data-live-search="true"
                        data-toggle="dropdown"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">

                    <label style="margin-top: 20px">
                        <asp:CheckBox ID="chkIsObsolete" runat="server" CssClass="uniform" ToolTip="Obsolete"/>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.IS_OBSOLETE)%>
                    </label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>
        $(document).ajaxStart(function () {
            $('#prepare_window_loading').css("display", "block")
        })
    </script>
</asp:Content>

